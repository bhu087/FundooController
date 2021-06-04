/////------------------------------------------------------------------------
////<copyright file="AccountManager.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////-------------------------------------------------------------------------

namespace FundooManager.Account
{
    using System;
    using System.Threading.Tasks;
    using FundooModel.Account;
    using FundooRepository.Repo.AccountRepository;

    /// <summary>
    /// Account manager class
    /// </summary>
    public class AccountManager : IAccountManager
    {
        /// <summary>
        /// Account repository
        /// </summary>
        private IAccountRepo repo;

        /// <summary>
        /// Account manager constructor
        /// </summary>
        /// <param name="accountRepo">account repository</param>
        public AccountManager(IAccountRepo accountRepo)
        {
            this.repo = accountRepo;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="register">register parameter</param>
        /// <returns>return Register</returns>
        public Task<User> RegisterUser(User register)
        {
            try
            {
                return this.repo.RegisterUser(register);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="login">parameter login</param>
        /// <returns>return token string</returns>
        public Task<string> LoginUser(Login login)
        {
            try
            {
                return this.repo.LoginUser(login);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Forget password
        /// </summary>
        /// <param name="email">parameter email</param>
        /// <returns>returns string value</returns>
        public Task<string> ForgetPassword(string email)
        {
            try
            {
                return this.repo.ForgetPassword(email);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="email">parameter email</param>
        /// <returns>returns string value</returns>
        public Task<string> ResetPassword(string email, string password)
        {
            try
            {
                return this.repo.ResetPassword(email, password);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
