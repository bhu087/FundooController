/////------------------------------------------------------------------------
////<copyright file="IAccountManager.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////-------------------------------------------------------------------------

namespace FundooManager.Account
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using FundooModel.Account;

    /// <summary>
    /// Account manager interface
    /// </summary>
    public interface IAccountManager
    {
        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="register">register parameter</param>
        /// <returns>return Register</returns>
        Task<User> RegisterUser(User register);

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="login">parameter login</param>
        /// <returns>return token string</returns>
        Task<string> LoginUser(Login login);

        /// <summary>
        /// Forget password
        /// </summary>
        /// <param name="email">parameter email</param>
        /// <returns>returns string value</returns>
        Task<string> ForgetPassword(string email);

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="email">parameter email</param>
        /// <returns>returns string value</returns>
        Task<string> ResetPassword(string email, string password);
    }
}
