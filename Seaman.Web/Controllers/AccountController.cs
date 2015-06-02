using System;
using System.Linq;
using System.Web.Http;
using Microsoft.Owin.Security.Cookies;
using Seaman.Core;
using Seaman.Web.App_Start;
using Seaman.Web.Code;
using Seaman.Web.Models;

namespace Seaman.Web.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {

        #region Constructors

        public AccountController(IUserManager userManager)
        {
            _userManager = userManager;
        } 

        #endregion

        #region Public

        [Route("GetUser")]
        [HttpGet]
        public IHttpActionResult GetUser()
        {
            var auth = this.GetAuthentication();

            var userId = this.GetUserId();
            var verificationResult = _userManager.VerifyUser(userId);
            if (verificationResult.Success)
            {
                auth.SignOut(CookieAuthenticationDefaults.AuthenticationType);
                var user = _userManager.GetIdentity(verificationResult.UserId,
                    CookieAuthenticationDefaults.AuthenticationType);
                auth.SignIn(user);
                return Ok(GetUserModel(verificationResult));
            }
            return GetErrorResult(verificationResult);
        }

        // POST api/Account/Login
        [AllowAnonymous]
        [Route("Login")]
        public IHttpActionResult Login(LoginBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var auth = this.GetAuthentication();
            var result = _userManager.VerifyUserPassword(model.Username, model.Password);
            
            if (!result.Success)
                return GetErrorResult(result);

            auth.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            auth.SignIn(_userManager.GetIdentity(result.UserId, CookieAuthenticationDefaults.AuthenticationType));
            return Ok(GetUserModel(result));
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            this.GetAuthentication().SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var verificationResult = _userManager.VerifyUserPassword(this.GetUserId(), model.OldPassword);
            if (!verificationResult.Success)
                return GetErrorResult(verificationResult);

            _userManager.SetUserPassword(this.GetUserId(), model.NewPassword, DefaultHashMethod);
            return Ok();
        }
        
        //// POST api/Account/SetPassword
        //[Route("SetPassword")]
        //public IHttpActionResult SetPassword(SetPasswordBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var verificationResult = _userManager.VerifyUser(this.GetUserId());
        //    if (!verificationResult.Success)
        //        return GetErrorResult(verificationResult);
        //    _userManager.SetUserPassword(this.GetUserId(), model.NewPassword, DefaultHashMethod);
        //    return Ok();
        //}

        //// POST api/Account/Reset
        //[Route("Reset")]
        //public IHttpActionResult Reset(ResetPasswordBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    return Ok();
        //}



        //// POST api/Account/Register
        //[AllowAnonymous]
        //[Route("Register")]
        //public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

        //    IdentityResult result = await UserManager.CreateAsync(user, model.Password);

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}

        #endregion
        
        #region Private

        private readonly IUserManager _userManager;

        private const String DefaultHashMethod = PasswordHasherNames.Plain;

        private UserBindingModel GetUserModel(UserCheckResult result)
        {
            return new UserBindingModel
            {
                Id = result.UserId,
                Name = result.UserName,
                Roles =
                    _userManager.GetUserRoles(result.UserId)
                        .Select(it =>
                        {
                            var name = Enum.GetName(typeof (Roles), it.Id);
                            return name != null ? name.ToLowerInvariant() : null;
                        })
            };
        }       
        
        private IHttpActionResult GetErrorResult(UserCheckResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Success)
            {
                if (result.IsLocked)
                {
                    ModelState.AddModelError("", "User blocked");
                }
                if (result.IsDeleted || result.IsNotFound)
                {
                    ModelState.AddModelError("", "User not found");
                }
                if (result.IsWrongPassword)
                {
                    ModelState.AddModelError("", "Incorrect password");
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        

        #endregion
    }
}
