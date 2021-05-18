﻿using FundooModel.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepository.Repo.Account
{
    public interface IAccountRepo
    {
        Task<Register> RegisterUser(Register register);
        Task<Login> LoginUser(Login login);
        Task<string> ForgetPassword(string email);
        Task<string> ResetPassword(string email);
    }
}
