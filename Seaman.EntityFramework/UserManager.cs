using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Seaman.Core;
using Seaman.EntityFramework.Entity;

namespace Seaman.EntityFramework
{
    public class UserManager : UserManager<UserModel, RoleModel>, IUserManager
    {
        public UserManager(SeamanDbContext context, PasswordHasherFactory hasherFunc) : base(context, hasherFunc)
        {
        }
    }
    
    /// <summary>
    /// Class for managing users and roles
    /// </summary>
    public class UserManager<TUserModel, TRoleModel> : UserManagerBase<TUserModel, TRoleModel> 
        where TUserModel : UserBase 
        where TRoleModel : RoleBase
    {
        #region Constructors

        public UserManager(SeamanDbContext context, PasswordHasherFactory hasherFunc)
            : base(hasherFunc)
        {
            _context = context;
        }

        #endregion

        #region Public

        /// <summary>
        /// Mark user as deleted
        /// </summary>
        /// <param name="id"></param>
        public override void Remove(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
                user.IsDeleted = true;
            Save();
        }

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override TUserModel Add(TUserModel user)
        {
            user.UserName = user.UserName.ToLowerInvariant();
            var entity = _context.Users.Add(Mapper.Map<User>(user));
            Save();
            return Mapper.Map<TUserModel>(entity);
        }

        /// <summary>
        /// update user from model
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override TUserModel Update(TUserModel user)
        {
            user.UserName = user.UserName.ToLowerInvariant();
            var entity = _context.Users.Find(user.Id);
            Mapper.Map(user, entity);
            Save();
            return Mapper.Map<TUserModel>(entity);
        }

        /// <summary>
        /// get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if not found</returns>
        public override TUserModel Find(int id)
        {
            return Mapper.Map<TUserModel>(_context.Users.Find(id));
        }

        /// <summary>
        /// get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>null if not found</returns>
        public override TUserModel FindByName(string username)
        {
            username = username.ToLowerInvariant();
            return Mapper.Map<TUserModel>(_context.Users.FindLocalOrRemote(it => it.UserName == username));
        }

        /// <summary>
        /// lock/unlock user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lockUntil"></param>
        public override void ToggleUserLock(int id, DateTime? lockUntil)
        {
            var user = UserById(id);
            user.LockedUntil = lockUntil;
            Save();
        }

        /// <summary>
        /// set user password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="storeAs"></param>
        public override void SetUserPassword(int id, string password, string storeAs)
        {
            var user = UserById(id);
            var hasher = HasherFactory(storeAs);
            if (hasher == null)
                throw new SeamanException("hasher not found");
            user.Password = hasher.HashPassword(password);
            user.PasswordType = storeAs;
            Save();
        }

        /// <summary>
        /// Get user roles by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override IEnumerable<TRoleModel> GetUserRoles(int userId)
        {
            return Mapper.Map<List<TRoleModel>>(_context.Users.Where(it => it.Id == userId).SelectMany(it => it.Roles));
        }

        /// <summary>
        /// Get user roles by user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override IEnumerable<TRoleModel> GetUserRoles(string username)
        {
            username = username.ToLowerInvariant();

            return
                Mapper.Map<List<TRoleModel>>(
                    _context.Users.Where(it => it.UserName == username).SelectMany(it => it.Roles));
        }

        /// <summary>
        /// give role to user 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="objectId"></param>
        public override void AddRoleToUser(Int32 userId, Int32 roleId, Int32 objectId = 0)
        {
            var role = _context.Roles.Where(it=> it.Id == roleId).Include(it=>it.Users).FirstOrDefault();
            if (role == null)
                throw new SeamanException("role not found");
            if (role.Users.All(it => it.Id != userId))
            {
                var user = UserById(userId);
                if (user == null)
                    throw new SeamanException("user not found");
                //role.Users.Add(user);
                user.Roles.Add(role);
                Save();
            }
        }

        /// <summary>
        /// remove role from user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="objectId"></param>
        public override void RemoveRoleFromUser(Int32 userId, Int32 roleId, Int32 objectId = 0)
        {
            var role = _context.Roles.FindLocalOrRemote(it=>it.Id == roleId, q=>q.Include(it=>it.Users));
            if (role == null)
                throw new SeamanException("role not found");
            var user = role.Users.FirstOrDefault(it => it.Id == userId);
            if (user != null)
            {
                role.Users.Remove(user);
                Save();
            }
        }


        /// <summary>
        /// get available roles for object
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public override IEnumerable<TRoleModel> GetRoles(int objectId)
        {
            //TODO get roles for object
            return GetRoles();
        }

        /// <summary>
        /// get available roles
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TRoleModel> GetRoles()
        {
            return Mapper.Map<List<TRoleModel>>(_context.Roles);
        } 

        #endregion

        

        #region Protected

        /// <summary>
        /// Get record to check user by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override UserCheckRecord GetUserCheckRecord(Int32 id)
        {
            return Mapper.Map<UserCheckRecord>(UserById(id, false));
        }

        /// <summary>
        /// Get record to check user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        protected override UserCheckRecord GetUserCheckRecord(String username)
        {
            username = username.ToLowerInvariant();
            return Mapper.Map<UserCheckRecord>(UserByName(username, false));
        }

        /// <summary>
        /// get user by id and optionally throw exception if not found
        /// </summary>
        /// <param name="id"></param>
        /// <param name="thowIfNotFound"></param>
        /// <returns></returns>
        protected virtual User UserById(Int32 id, Boolean thowIfNotFound = true)
        {
            var user = _context.Users.FindLocalOrRemote(it=>it.Id == id, q=>q.Include(it=>it.Roles));
            if (thowIfNotFound && user == null)
                    throw new SeamanException("user not found");
            return user;
        }

        /// <summary>
        /// get user by username and optionally throw exception if not found
        /// </summary>
        /// <param name="username"></param>
        /// <param name="thowIfNotFound"></param>
        /// <returns></returns>
        protected virtual User UserByName(String username, Boolean thowIfNotFound = true)
        {
            username = username.ToLowerInvariant();
            var user = _context.Users.FindLocalOrRemote(it => it.UserName == username, q => q.Include(it => it.Roles));
            if (thowIfNotFound && user == null)
                throw new SeamanException("user not found");
            return user;
        }

        /// <summary>
        /// save changes
        /// </summary>
        protected virtual void Save()
        {
            _context.SaveChanges();
        }

        #endregion

        #region Private

        private readonly SeamanDbContext _context;

        #endregion
    }
}