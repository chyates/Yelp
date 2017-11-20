using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace yelp.Models
{
    public class BusCategoryType : BaseEntity
    {
        public int BusCategoryTypeId { get; set; }
        public string CategoryType { get; set; }
        public int CategoryId { get; set; }
        public BusCategory Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default Constructor without parameters
        public BusCategoryType()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
    }

    public class BusCategoryTypeViewModel : BaseEntity
    {
        [Display(Name = "Category Type")]
        [Required]
        [MinLength(2)]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_NAMES, ErrorMessage = Constants.REGEX_NAMES_MESSAGE)]
        public string CategoryType { get; set; }
    }
}
