/////------------------------------------------------------------------------
////<copyright file="AccountController.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////-------------------------------------------------------------------------

namespace FundooController.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using FundooManager.Account;
    using FundooModel.Account;
    using LoggerService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    
    /// <summary>
    /// Account controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// Account manager
        /// </summary>
        private readonly IAccountManager manager;

        /// <summary>
        /// Logger interface
        /// </summary>
        private readonly ILoggerManager logger;

        /// <summary>
        /// Account controller constructor
        /// </summary>
        /// <param name="accountManager">Account manager</param>
        /// <param name="loggerManager">Logger manager</param>
        public AccountController(IAccountManager accountManager, ILoggerManager loggerManager)
        {
            this.manager = accountManager;
            this.logger = loggerManager;
        }
        
        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="register">Parameter Register model</param>
        /// <returns>Action results</returns>
        [HttpPost]
        public ActionResult RegisterUser(User register)
        {
            try
            {
                Task<User> response = this.manager.RegisterUser(register);
                if (response.Result != null)
                {
                    this.logger.LogInfo("Registered Successfully " + response.Result.Name + " Status : OK");
                    this.logger.LogDebug("Debug Successfull : Register User");
                    return this.Ok(new { Status = true, Message = "Registered Successfully", Data = response.Result });
                }

                this.logger.LogError("Not Registered " + response.Result + " Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Not Registered", Data = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogWarn("Exception " + e + " Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        /// <summary>
        /// Login method
        /// </summary>
        /// <param name="login">Parameter Login</param>
        /// <returns>Action results</returns>
        [HttpPost("Login")]
        public async Task<ActionResult> LoginUser(Login login)
        {
            string response = await this.manager.LoginUser(login);
            try
            {
                if (response != null)
                {
                    HttpContext.Session.SetString("Token", response);
                    this.logger.LogInfo("Logged in Successfully  Status : OK");
                    this.logger.LogDebug("Debug Successfull : Logged in User");
                    return this.Ok(new { Status = true, Message = "Logged in successfully", Data = response });
                }

                this.logger.LogError("Not Registered Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Not Logged in", Data = response });
            }
            catch (Exception e)
            {
                this.logger.LogError("Exception : " + e + " Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        /// <summary>
        /// Forget Email
        /// </summary>
        /// <param name="email">Parameter email</param>
        /// <returns>Action result</returns>
        [HttpPost]
        [Route("Forget/{email}")]
        public ActionResult ForgetPassword(string email)
        {
            Task<string> response = this.manager.ForgetPassword(email);
            try
            {
                if (response.Result != null)
                {
                    this.logger.LogInfo("Password send to your Email successfully  Status : OK");
                    this.logger.LogDebug("Debug Successfull : Forget password");
                    return this.Ok(new { Status = true, Message = "Password send to your Email successfully", Data = response.Result });
                }

                this.logger.LogError("Account not exist for Email : " + email + " Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Account not exist", Data = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogError("Exception : " + e + " Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        /// <summary>
        /// Reset Email
        /// </summary>
        /// <param name="email">Parameter email</param>
        /// <returns>Action result</returns>
        [HttpPost]
        [Route("Reset/{password}")]
        public ActionResult ResetPassword(string password)
        {
            var token = HttpContext.Request?.Headers["token"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var email = tokenS.Claims.First(claim => claim.Type == "Email").Value;
            Task<string> response = this.manager.ResetPassword(email , password);
            try
            {
                if (response.Result != null)
                {
                    this.logger.LogInfo("Reset password successfully Status : OK");
                    this.logger.LogDebug("Debug Successfull : reset password");
                    return this.Ok(new { Status = true, Message = "Reset password successfully", Data = response.Result });
                }

                this.logger.LogError("invalid Link/Expired :  Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "invalid Link/Expired", Data = response.Result });
            }
            catch (Exception e)
            {
                this.logger.LogError("Exception : " + e + " Status : Bad Request");
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }
    }
}
