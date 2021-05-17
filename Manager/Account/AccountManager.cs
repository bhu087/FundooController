using Manager.Account;
using ModelClass.Account;
using Repository.Repo.Account;
using System;
using System.Threading.Tasks;

namespace Manager.Account
{
    public class AccountManager : IAccountManager
    {
        private IAccountRepo repo;
        public AccountManager(IAccountRepo accountRepo)
        {
            repo = accountRepo;
        }

        public Task<Register> RegisterUser(Register register)
        {
            try
            {
                return repo.RegisterUser(register);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<Login> LoginUser(Login login)
        {
            try
            {
                return repo.LoginUser(login);
            }

            catch
            {
                throw new Exception();
            }
        }

    }
}
