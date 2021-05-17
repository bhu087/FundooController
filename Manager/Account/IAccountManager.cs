using ModelClass;
using ModelClass.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Account
{
    public interface IAccountManager
    {
        Task<Register> RegisterUser(Register register);
        Task<Login> LoginUser(Login login);
        Task<string> ForgetPassword(string email);
    }
}
