using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace yelp.Models
{
    public class ReviewVote : BaseEntity
    {
        public int ReviewVoteId { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool Useful { get; set; }
        public bool Funny { get; set; }
        public bool Cool { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default Constructor without parameters
        public ReviewVote()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }        
    }

}
