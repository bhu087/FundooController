﻿using Manager.Account;
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

        public Task<string> ForgetPassword(string email)
        {
            try
            {
                return repo.ForgetPassword(email);
            }
            catch
            {
                throw new Exception();
            }
        }

        public Task<string> ResetPassword(string email)
        {
            try
            {
                return repo.ResetPassword(email);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
