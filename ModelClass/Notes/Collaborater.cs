using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FundooModel.Notes
{
    public class Collaborater
    {
        [Key]
        public int CollaborateId { get; set; }
        public int NotesId { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
    }
}
