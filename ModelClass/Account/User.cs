using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FundooModel.Account
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [RegularExpression(@"^[A-Z]{1}[a-z]{2,}$")]
        public string Name { get; set; }
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        public string Email { get; set; }
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^\w\s]).{8,}$")]
        public string Password { get; set; }
        public string ProfilePic { get; set; }
    }
}
