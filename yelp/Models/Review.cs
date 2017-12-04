using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace yelp.Models
{
    public class Review : BaseEntity
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public User user { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default Constructor without parameters
        public Review()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
        public Review(ReviewViewModel NewReview)
        {
            this.Rating = NewReview.Rating;
            this.ReviewText = NewReview.ReviewText;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
    }

    public class ReviewViewModel : BaseEntity
    {
        [Display(Name = "Review")]
        [Required]
        [MinLength(10)]
        [RegularExpression(Constants.REGEX_ALL_EXCEPT_SENSITIVE, ErrorMessage = Constants.REGEX_ALL_MESSAGE)]
        public string ReviewText { get; set; }

        [Display(Name = "Rating")]
        [Range(0,5)]
        public int Rating { get; set; }
    }
}
