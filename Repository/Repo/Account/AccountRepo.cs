using ModelClass.Account;
using Repository.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public async Task<string> ForgetPassword(string email)
        {
            try
            {
                var user = await Task.Run(() => this.GetAccountByEmail(email));
                if (user != null)
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add("bhu087@gmail.com");
                    mail.From = new MailAddress("bhush097@gmail.com");
                    mail.Subject = "Password from Fundoo";
                    mail.Body = "Email : " + user.Email + "\nPassword : " + user.Password;
                    mail.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        Credentials = new NetworkCredential("bhush097@gmail.com", "ABCD12345")
                    };
                    smtp.Send(mail);
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
                var user = await Task.Run(() => this.GetAccountByEmail(email));
                if (user != null)
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add("bhu087@gmail.com");
                    mail.From = new MailAddress("bhush097@gmail.com");
                    mail.Subject = "Reset Link for Fundoo";
                    mail.Body = "https://localhost:44337/swagger/index.html";
                    mail.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        Credentials = new NetworkCredential("bhush097@gmail.com", "ABCD12345")
                    };
                    smtp.Send(mail);
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
            var user = this.context.AccountRegisters.Where(emailId => emailId.Email == email).SingleOrDefault();
            if (user.Email.Equals(email))
            {
                return user;
            }
            return null;
        }
    }
}
