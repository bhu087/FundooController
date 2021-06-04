/////------------------------------------------------------------------------
////<copyright file="IAccountRepo.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////--------------------------------------------------------------------------

namespace FundooRepository.Repo.AccountRepository
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using FundooModel.Account;

    /// <summary>
    /// Repository account interface
    /// </summary>
    public interface IAccountRepo
    {
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="register">register parameter</param>
        /// <returns>returns Register</returns>
        Task<User> RegisterUser(User register);

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="login">parameter for login</param>
        /// <returns>returns string value</returns>
        Task<string> LoginUser(Login login);

        /// <summary>
        /// Forget Password
        /// </summary>
        /// <param name="email">parameter email</param>
        /// <returns>returns String value</returns>
        Task<string> ForgetPassword(string email);

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="email">email parameter</param>
        /// <returns>returns success result</returns>
        Task<string> ResetPassword(string email, string password);
    }
}
