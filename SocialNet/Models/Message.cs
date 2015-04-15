using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialNet.Models
{
    public class Message
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }

        public int UserId { get; set; }
        public User Author { get; set; }

        public int DialogId { get; set; }
        public Dialog Dialog { get; set; } 
    }
}