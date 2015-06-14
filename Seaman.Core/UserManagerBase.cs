using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Seaman.Core
{
    /// <summary>
    /// abstract base class for implementing usermanager
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    public abstract class UserManagerBase<TUser, TRole> : IUserManager<TUser, TRole>
        where TUser : UserBase 
        where TRole : RoleBase
    {

        protected UserManagerBase(PasswordHasherFactory hasherFactory)
        {
            HasherFactory = hasherFactory;
        }

        
        /// <summary>
        /// mark user as deleted
        /// </summary>
        /// <param name="id"></param>
        public abstract void Remove(int id);

        /// <summary>
        /// add new user
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract TUser Add(TUser entity);

        /// <summary>
        /// update user from model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract TUser Update(TUser entity);

        /// <summary>
        /// find user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract TUser Find(int id);

        /// <summary>
        /// find user by name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public abstract TUser FindByName(string username);


        /// <summary>
        /// lock/unlock user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lockUntil"></param>
        public abstract void ToggleUserLock(int id, DateTime? lockUntil);

        /// <summary>
        /// set user password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="storeAs"></param>
        public abstract void SetUserPassword(int id, string password, string storeAs);


        /// <summary>
        /// verify user and password by user id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual UserCheckResult VerifyUserPassword(Int32 id, String password)
        {
            var userRecord = GetUserCheckRecord(id);
            return CheckUser(userRecord, password);
        }


        /// <summary>
        /// verify user and password by username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual UserCheckResult VerifyUserPassword(String username, String password)
        {
            var userRecord = GetUserCheckRecord(username);
            return CheckUser(userRecord, password);
        }

        /// <summary>
        /// verify user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual UserCheckResult VerifyUser(Int32 id)
        {
            var userRecord = GetUserCheckRecord(id);
            return CheckUser(userRecord);
        }


        /// <summary>
        /// verify user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public virtual UserCheckResult VerifyUser(String username)
        {
            var userRecord = GetUserCheckRecord(username);
            return CheckUser(userRecord);
        }

        /// <summary>
        /// get ClaimsIdentity for user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        public virtual ClaimsIdentity GetIdentity(int id, string authenticationType)
        {
            var user = Find(id);
            if (user == null)
                return null;
            var identity = new ClaimsIdentity(authenticationType, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName, ClaimValueTypes.String));

            identity.AddClaim(
                new Claim(IdentityProviderClaimType,
                    IdentityProviderClaimValue, ClaimValueTypes.String));

            var roles = GetUserRoles(id);
            foreach (var role in roles.Select(it=>(Roles)it.Id).Distinct())
            {
                identity.AddClaim(
                    new Claim(ClaimsIdentity.DefaultRoleClaimType,
                        role.ToString(), ClaimValueTypes.String));
            }
            
            return identity;
        }

        /// <summary>
        /// Get user roles by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public abstract IEnumerable<TRole> GetUserRoles(int userId);

        /// <summary>
        /// Get user roles by user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public abstract IEnumerable<TRole> GetUserRoles(string username);

        /// <summary>
        /// give role to user 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="objectId"></param>
        public abstract void AddRoleToUser(Int32 userId, int roleId, int objectId = 0);

        /// <summary>
        /// remove role from user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="objectId"></param>
        public abstract void RemoveRoleFromUser(Int32 userId, int roleId, int objectId = 0);
        /// <summary>
        /// get available roles for object
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public abstract IEnumerable<TRole> GetRoles(int objectId);
        /// <summary>
        /// get available roles
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<TRole> GetRoles();

        public abstract IEnumerable<TUser> GetUsers();

        #region Protected

         

        protected PasswordHasherFactory HasherFactory { get; set; }

        protected String IdentityProviderClaimType =
            "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";

        protected String IdentityProviderClaimValue = "ASP.NET Identity";

        /// <summary>
        /// Get record to check user by user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected abstract UserCheckRecord GetUserCheckRecord(Int32 id);

        /// <summary>
        /// Get record to check user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        protected abstract UserCheckRecord GetUserCheckRecord(String username);

        /// <summary>
        /// Check if user found, deleted, locked, password is correct
        /// </summary>
        /// <param name="userRecord"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected virtual UserCheckResult CheckUser(UserCheckRecord userRecord, string password)
        {
            var result = CheckUser(userRecord);
            if (userRecord != null)
            {
                var hasher = HasherFactory(userRecord.PasswordType);
                result.IsWrongPassword = hasher == null || !hasher.VerifyHashedPassword(userRecord.Password, password);
            }
            return result;
        }

        /// <summary>
        /// Check if user found, deleted, locked
        /// </summary>
        /// <param name="result"></param>
        /// <param name="userRecord"></param>
        /// <returns></returns>
        protected virtual UserCheckResult CheckUser(UserCheckResult result, UserCheckRecord userRecord)
        {
            if (userRecord == null)
            {
                result.IsNotFound = true;
                return result;
            }
            result.UserId = userRecord.Id;
            result.UserName = userRecord.UserName;
            result.IsDeleted = userRecord.IsDeleted;
            result.LockedUntil = userRecord.LockedUntil;
            result.FirstName = userRecord.FirstName;
            result.LastName = userRecord.LastName;
            return result;
        }
        /// <summary>
        /// Check if user found, deleted, locked
        /// </summary>
        /// <param name="userRecord"></param>
        /// <returns></returns>
        protected virtual UserCheckResult CheckUser(UserCheckRecord userRecord)
        {
            var result = new UserCheckResult();
            return CheckUser(result, userRecord);
        }

        

        #endregion

    }
}