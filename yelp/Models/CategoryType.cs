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

        public List<Business> Businesses { get; set; }

        // Default Constructor without parameters
        public BusCategoryType()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Businesses = new List<Business>();
        }
    }

    public class BusCategoryTypeViewModel : BaseEntity
    {
        [Display(Name = "Subcategory")]
        [Required]
        [MinLength(2)]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_NAMES, ErrorMessage = Constants.REGEX_NAMES_MESSAGE)]
        public string CategoryType { get; set; }

        [Display(Name = "Category")]
        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}
