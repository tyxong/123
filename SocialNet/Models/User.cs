using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialNet.Models
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            Dialogs = new List<Dialog>();
            WallPosts = new List<WallPost>();
            Photos = new List<Photo>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime LastVisit { get; set; }
        public string CurrentCity { get; set; }
        public string Mobile { get; set; }
        public string Adress { get; set; }
        public string RelationShip { get; set; }
        public string Friends { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public ICollection<Dialog> Dialogs { get; set; }

        public ICollection<WallPost> WallPosts { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}