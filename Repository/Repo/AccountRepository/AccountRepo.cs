/////------------------------------------------------------------------------
////<copyright file="AccountRepo.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////-------------------------------------------------------------------------

namespace FundooRepository.Repo.AccountRepository
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Experimental.System.Messaging;
    using FundooModel.Account;
    using FundooRepository.DbContexts;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using StackExchange.Redis;

    /// <summary>
    /// Account repository class
    /// </summary>
    public class AccountRepo : IAccountRepo
    {
        /// <summary>
        /// User DB context
        /// </summary>
        private readonly UserDbContext context;

        /// <summary>
        /// Configuration field
        /// </summary>
        private readonly IConfiguration config;

        /// <summary>
        /// database for cache
        /// </summary>
        private IDatabase database;

        /// <summary>
        /// Connection multiplexer
        /// </summary>
        private ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");

        /// <summary>
        /// Message Queue
        /// </summary>
        private MessageQueue messageQueue = new MessageQueue();

        /// <summary>
        /// Account repository constructor
        /// </summary>
        /// <param name="userDbContext">user database context</param>
        /// <param name="configuration">parameter configuration</param>
        public AccountRepo(UserDbContext userDbContext, IConfiguration configuration)
        {
            this.context = userDbContext;
            this.config = configuration;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="register">register parameter</param>
        /// <returns>returns Register</returns>
        public async Task<User> RegisterUser(User register)
        {
            try
            {
                User user = await Task.Run(() => this.GetAccountByEmail(register.Email.ToLower()));
                if (user == null)
                {
                    User save = new User
                    {
                        Name = register.Name,
                        Email = register.Email.ToLower(),
                        Password = this.PasswordEncryption(register.Password)
                    };
                    this.context.Users.Add(save);
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
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="login">parameter for login</param>
        /// <returns>returns string value</returns>
        public async Task<string> LoginUser(Login login)
        {
            try
            {
                User register = await Task.Run(() => this.GetAccountByEmail(login.Email.ToLower()));
                if (register != null)
                {
                    if (login.Password.Equals(this.PasswordDecryption(register.Password)))
                    {
                        string jwtToken = this.GenerateJWTtokens(register.UserID, register.Email.ToLower());
                        this.SaveToRedisCache("Login", jwtToken);
                        return jwtToken;
                    }
                }

                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Forget Password
        /// </summary>
        /// <param name="email">parameter email</param>
        /// <returns>returns String value</returns>
        public async Task<string> ForgetPassword(string email)
        {
            try
            {
                var user = await Task.Run(() => this.GetAccountByEmail(email.ToLower()));
                if (user != null)
                {
                    string subject = "Password from Fundoo", body = "Email: " + user.Email + "\nPassword: " + this.PasswordDecryption(user.Password);
                    this.MsmqService();
                    this.AddToQueue(email, subject, body);
                    return await Task.Run(() => "Success");
                }

                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="email">email parameter</param>
        /// <returns>returns success result</returns>
        public async Task<string> ResetPassword(string email)
        {
            try
            {
                var user = this.GetAccountByEmail(email.ToLower());
                if (user != null)
                {
                    string subject = "Reset Link for Fundoo", body = "https://localhost:44337/swagger/index.html";
                    this.MsmqService();
                    this.AddToQueue(email, subject, body);
                    return await Task.Run(() => "Success");
                }

                return null;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get Account By email ID
        /// </summary>
        /// <param name="email">parameter email</param>
        /// <returns>returns Register model</returns>
        public User GetAccountByEmail(string email)
        {
            try
            {
                var user = this.context.Users.Where(emailId => emailId.Email == email).SingleOrDefault();
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

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="subject">receiving subject</param>
        /// <param name="body">receiving body of mail</param>
        public void SendMail(string subject, string body)
        {
            try
            {
                string accountEmail = this.config["NetworkCredentials:AccountEmail"];
                string accountPass = this.config["NetworkCredentials:AccountPass"];
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
                    Credentials = new NetworkCredential(accountEmail, accountPass)
                };
                smtp.Send(mail);
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Password Encryption
        /// </summary>
        /// <param name="password">parameter password</param>
        /// <returns>returns encrypted data</returns>
        public string PasswordEncryption(string password)
        {
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            string encryptedPassword = Convert.ToBase64String(encode);
            return encryptedPassword;
        }

        /// <summary>
        /// Password Decryption
        /// </summary>
        /// <param name="encryptPassword">receiving encrypted password</param>
        /// <returns>returns decrypted password</returns>
        public string PasswordDecryption(string encryptPassword)
        {
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptPassword);
            int charCount = decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string decryptePassword = new string(decoded_char);
            return decryptePassword;
        }

        /// <summary>
        /// Generate JWT tokens
        /// </summary>
        /// <param name="userId">receiving ID</param>
        /// <param name="userEmail">receiving Name</param>
        /// <returns>returns JWT generated string</returns>
        public string GenerateJWTtokens(int userId, string userEmail)
        {
            string key = this.config["Jwt:Key"];
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Role, "User"),
                            new Claim("Id", userId.ToString()),
                            new Claim("Email", userEmail)
                        }),
                    Expires = DateTime.Now.AddDays(Convert.ToDouble(this.config["JwtExpireDays"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
                };
                var securityTokenHandler = new JwtSecurityTokenHandler();
                var securityToken = securityTokenHandler.CreateToken(tokenDescriptor);
                var token = securityTokenHandler.WriteToken(securityToken);
                return token;
            }
            catch
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Store to cache
        /// </summary>
        /// <param name="key">Key value</param>
        /// <param name="jwtToken">receiving JWT token to store cache</param>
        public void SaveToRedisCache(string key, string jwtToken)
        {
            string cacheKey = key;
            this.database = this.connectionMultiplexer.GetDatabase();
            this.database.StringSet(cacheKey, jwtToken);
        }

        /// <summary>
        /// Delete the entry in cache by particular key
        /// </summary>
        /// <param name="key">Key value</param>
        public void DeleteFromRedisCache(string key)
        {
            this.database.KeyDelete(key);
        }

        /// <summary>
        /// It checks for Queue is present or not.
        /// </summary>
        /// <returns>Return Queue</returns>
        public MessageQueue MsmqService()
        {
            string queuePath = @".\private$\FundooQueue";
            if (MessageQueue.Exists(queuePath))
            {
                this.messageQueue = new MessageQueue(queuePath);
                return this.messageQueue;
            }
            else
            {
                this.messageQueue = MessageQueue.Create(queuePath);
                return this.messageQueue;
            }
        }

        /// <summary>
        /// Add to queue
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public void AddToQueue(string email, string subject, string body)
        {
            EmailDetails emailDetails = new EmailDetails
            {
                Email = email,
                Subject = subject,
                Body = body
            };
            this.messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(EmailDetails) });

            this.messageQueue.ReceiveCompleted += this.ReceiveFromQueue;

            this.messageQueue.Send(emailDetails);

            this.messageQueue.BeginReceive();

            this.messageQueue.Close();
        }

        /// <summary>
        /// Receive from queue
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Evenet of Receive Completed</param>
        public void ReceiveFromQueue(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = this.messageQueue.EndReceive(e.AsyncResult);
                var emailDetails = (EmailDetails)msg.Body;
                this.SendMail(emailDetails.Subject, emailDetails.Body);
                using (StreamWriter file = new StreamWriter(@"I:\Utility\Fundoo.txt", true))
                {
                    file.WriteLine(emailDetails.Subject);
                }

                this.messageQueue.BeginReceive();
            }
            catch (MessageQueueException qexception)
            {
                Console.WriteLine(qexception);
            }
        }
    }
}
