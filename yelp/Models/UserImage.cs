using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace yelp.Models
{
    public class UserImage : BaseEntity
    {
        public int UserImageId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string ImageLink { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default Constructor without parameters
        public UserImage()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
    }
}
