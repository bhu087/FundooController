
using FundooModel.Account;
using FundooRepository.DbContexts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepository.Repo.Account
{
    public class AccountRepo : IAccountRepo
    {
        public readonly UserDbContext context;
        public readonly IConfiguration config;
        public AccountRepo(UserDbContext userDbContext, IConfiguration configuration)
        {
            this.context = userDbContext;
            this.config = configuration;

        }

        public async Task<Register> RegisterUser(Register register)
        {
            try
            {
                Register user = await Task.Run(() => this.GetAccountByEmail(register.Email));
                if (user == null)
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

        public async Task<string> ForgetPassword(string email)
        {
            try
            {
                var user = await Task.Run(() => this.GetAccountByEmail(email));
                if (user != null)
                {
                    string subject = "Password from Fundoo", body = "Email: " + user.Email + "\nPassword: " + user.Password;
                    this.SendMail(subject, body);
                    return await Task.Run(() => "Success");
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception();
            }
        }

        public async Task<string> ResetPassword(string email)
        {
            try
            {
                var user = GetAccountByEmail(email);
                if (user != null)
                {
                    string subject = "Reset Link for Fundoo", body = "https://localhost:44337/swagger/index.html";
                    this.SendMail(subject, body);
                    return await Task.Run(() => "Success");
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
            try
            {
                var user = this.context.AccountRegisters.Where(emailId => emailId.Email == email).SingleOrDefault();
                if (user == null)
                {
                    return null;
                }
                if (user != null && user.Email.Equals(email))
                {
                    return user;
                }
                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        public void SendMail(string subject, string body)
        {
            try
            {
                string AccountEmail = config["NetworkCredentials:AccountEmail"];
                string AccountPass = config["NetworkCredentials:AccountPass"];
                MailMessage mail = new MailMessage();
                mail.To.Add("bhu087@gmail.com");
                mail.From = new MailAddress("bhush097@gmail.com");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(AccountEmail, AccountPass)
                };
                smtp.Send(mail);
            }
            catch
            {
                throw new Exception();
            }
        }

        public string PasswordEncryption(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        public static string PasswordDecryption(string encryptPassword)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptPassword);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }
    }
}
