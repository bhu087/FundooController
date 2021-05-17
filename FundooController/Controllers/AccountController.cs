using Manager.Account;
using Microsoft.AspNetCore.Mvc;
using ModelClass.Account;
using Repository.Repo.Account;
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
            Task<Login> response = manager.LoginUser(login);
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
        [Route("{email}")]
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

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
