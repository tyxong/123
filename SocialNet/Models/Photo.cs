using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNet.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public byte[] Picture { get; set; }
        public string ImageMimeType { get; set; }
        public bool IsAvatar { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}