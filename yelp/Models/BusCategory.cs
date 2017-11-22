using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace yelp.Models
{
    public class BusCategory : BaseEntity
    {
        public int BusCategoryId { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Business> Businesses { get; set; }
        public List<BusCategoryType> CategoryTypes { get; set; }

        // Default Constructor without parameters
        public BusCategory()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Businesses = new List<Business>();
            this.CategoryTypes = new List<BusCategoryType>();
        }
    }

    public class BusCategoryViewModel : BaseEntity
    {
        [Display(Name = "Category")]
        [Required]
        [MinLength(2)]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_NAMES, ErrorMessage = Constants.REGEX_NAMES_MESSAGE)]
        public string Category { get; set; }
    }
}
