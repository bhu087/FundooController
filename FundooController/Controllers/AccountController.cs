
using FundooManager.Account;
using FundooModel.Account;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountManager manager;
        public AccountController(IAccountManager accountManager)
        {
            manager = accountManager;
        }
        // POST api/values
        [HttpPost]
        public ActionResult RegisterUser(Register register)
        {
            try
            {
                Task<Register> response = manager.RegisterUser(register);
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Registered Successfully", Data = response.Result });
                }
                return this.BadRequest(new { Status = false, Message = "Not Registered", Data = response.Result});
            }
            catch(Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult LoginUser(Login login)
        {
            Task<string> response = manager.LoginUser(login);
            try
            {
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Logged in successfully", Data = response.Result });
                }
                return this.BadRequest(new { Status = false, Message = "Not Logged in", Data = response.Result });
            }
            catch(Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        [HttpPost]
        [Route("Forget/{email}")]
        public ActionResult ForgetPassword(string email)
        {
            Task<string> response = manager.ForgetPassword(email);
            try
            {
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Password send to your Email successfully", Data = response.Result });
                }
                return this.BadRequest(new { Status = false, Message = "Account not exist", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }

        [HttpPost]
        [Route("Reset/{email}")]
        public ActionResult ResetPassword(string email)
        {
            Task<string> response = manager.ResetPassword(email);
            try
            {
                if (response.Result != null)
                {
                    return this.Ok(new { Status = true, Message = "Reset Link sent to your Email successfully", Data = response.Result });
                }
                return this.BadRequest(new { Status = false, Message = "Account not exist", Data = response.Result });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Status = false, Message = "Exception", Data = e });
            }
        }
    }
}
