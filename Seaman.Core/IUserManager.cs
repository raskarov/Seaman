using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Seaman.Core
{
    public interface IUserManager : IUserManager<UserModel, RoleModel>
    {
        
    }
    public interface IUserManager<TUser, out TRole>
        where TUser : UserBase
        where TRole : RoleBase
    {
        /// <summary>
        /// mark user as deleted
        /// </summary>
        /// <param name="id"></param>
        void Remove(Int32 id);
        /// <summary>
        /// add new user
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TUser Add(TUser entity);
        /// <summary>
        /// update user from model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TUser Update(TUser entity);
        /// <summary>
        /// get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TUser Find(Int32 id);
        /// <summary>
        /// get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>null if not found</returns>
        TUser FindByName(String username);
        /// <summary>
        /// lock/unlock user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lockUntil"></param>
        void ToggleUserLock(Int32 id, DateTime? lockUntil);
        /// <summary>
        /// set user password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="storeAs"></param>
        void SetUserPassword(Int32 id, String password, String storeAs);
        /// <summary>
        /// verify user and password by user id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserCheckResult VerifyUserPassword(Int32 id, String password);
        /// <summary>
        /// verify user and password by username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserCheckResult VerifyUserPassword(String username, String password);
        /// <summary>
        /// verify user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserCheckResult VerifyUser(Int32 id);
        /// <summary>
        /// verify user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UserCheckResult VerifyUser(String username);
        /// <summary>
        /// get ClaimsIdentity for user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        ClaimsIdentity GetIdentity(Int32 id, String authenticationType);
        /// <summary>
        /// Get user roles by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<TRole> GetUserRoles(Int32 userId);
        /// <summary>
        /// Get user roles by user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        IEnumerable<TRole> GetUserRoles(String username);
        /// <summary>
        /// give role to user 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="objectId"></param>
        void AddRoleToUser(Int32 userId, Int32 roleId, Int32 objectId = 0);
        /// <summary>
        /// remove role from user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="objectId"></param>
        void RemoveRoleFromUser(Int32 userId, Int32 roleId, Int32 objectId = 0);
        /// <summary>
        /// get available roles for object
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        IEnumerable<TRole> GetRoles(Int32 objectId);
        /// <summary>
        /// get available roles
        /// </summary>
        /// <returns></returns>
        IEnumerable<TRole> GetRoles();


    }


    public enum Roles
    {
        NoRole,
        Embryologist,
        ReportGenerator,
        Admin
    }
    public class UserCheckRecord
    {
        public Int32 Id { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String PasswordType { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime? LockedUntil { get; set; }
    }
}