using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNet.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [Required]
        public string TextOfComment { get; set; }
        public DateTime DateOfComment { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }

        public int WallPostId { get; set; }
        public WallPost WallPost { get; set; }
    }
}