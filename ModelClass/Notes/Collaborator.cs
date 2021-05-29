using FundooModel.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FundooModel.Notes
{
    public class Collaborator
    {
        [Key]
        public int CollaborateId { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public int NotesId { get; set; }
        public User User { get; set; }
    }
}
