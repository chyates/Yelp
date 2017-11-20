using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace yelp.Models
{
    public class ReviewImage : BaseEntity
    {
        public int ReviewImageId { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }
        public string ImageLink { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default Constructor without parameters
        public ReviewImage()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
    }
}
