using FundooModel.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FundooModel.Notes
{
    public class Notes
    {
        [Key]
        public int NotesId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string Remainder { get; set; }
        public string Image { get; set; }
        public bool IsArchive { get; set; }
        public bool IsTrash { get; set; }
        public bool IsPin { get; set; }
        public string Color { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
