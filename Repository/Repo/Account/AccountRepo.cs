using ModelClass.Account;
using Repository.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo.Account
{
    public class AccountRepo : IAccountRepo
    {
        public readonly UserDbContext context;
        public AccountRepo(UserDbContext userDbContext)
        {
            this.context = userDbContext;
        }

        public async Task<Register> RegisterUser(Register register)
        {
            try
            {
                Register save = new Register
                {
                    Name = register.Name,
                    Email = register.Email,
                    Password = register.Password
                };
                this.context.AccountRegisters.Add(save);
                var result = await Task.Run(() => this.context.SaveChangesAsync());
                if (result == 1)
                {
                    return register;
                }
                return null;
            }
            catch
            {
                throw new Exception();
            }
            finally
            {

            }
        }

        public async Task<Login> LoginUser(Login login)
        {
            try
            {
                Register register = await Task.Run(() => GetAccountByEmail(login.Email));
                if (register != null)
                {
                    if (login.Password.Equals(register.Password))
                    {
                        return login;
                    }
                }
                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        public Register GetAccountByEmail(string email)
        {
            var user = this.context.AccountRegisters.Where(emailId => emailId.Email == email).SingleOrDefault();
            if (user.Email.Equals(email))
            {
                return user;
            }
            return null;
        }
    }
}
