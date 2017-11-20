using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace yelp.Models
{
    public class Business : BaseEntity
    {
        public int BusinessId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public BusCategory Category { get; set; }
        public int? CategoryTypeId { get; set; }
        public BusCategoryType CategoryType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string ImageLink { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default Constructor without parameters
        public Business()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
        public Business(BusinessViewModel NewBusiness)
        {
            this.Name = NewBusiness.Name;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
    }

    public class BusinessViewModel : BaseEntity
    {
        [Display(Name = "Business Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_NAMES, ErrorMessage = Constants.REGEX_NAMES_MESSAGE)]
        public string Name { get; set; }

        [Display(Name = "Category")]
        [RegularExpression(Constants.REGEX_DIGITS_ONLY, ErrorMessage = Constants.REGEX_DIGITS_MESSAGE)]
        public int CategoryId { get; set; }

        [Display(Name = "Subcategory")]
        [RegularExpression(Constants.REGEX_DIGITS_ONLY, ErrorMessage = Constants.REGEX_DIGITS_MESSAGE)]
        public int? CategoryTypeId { get; set; }

        [Display(Name = "Address")]
        [Required]
        [MinLength(2)]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_ALL_EXCEPT_SENSITIVE, ErrorMessage = Constants.REGEX_ALL_MESSAGE)]
        public string Address { get; set; }

        [Display(Name = "City")]
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression(Constants.REGEX_NAMES, ErrorMessage = Constants.REGEX_NAMES_MESSAGE)]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required]
        [MinLength(2)]
        [MaxLength(2)]
        [RegularExpression(Constants.REGEX_STATES, ErrorMessage = Constants.REGEX_STATES_MESSAGE)]
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        [Required]
        [MinLength(5)]
        [MaxLength(10)]
        [RegularExpression(Constants.REGEX_ZIPCODE, ErrorMessage = Constants.REGEX_ZIPCODE_MESSAGE)]
        public int ZipCode { get; set; }

        [Display(Name = "Phone")]
        [Required]
        [RegularExpression(Constants.REGEX_PHONE, ErrorMessage = Constants.REGEX_PHONE_MESSAGE)]
        public string Phone { get; set; }

        [Display(Name = "Website")]
        [Required]
        [MinLength(2)]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_ALL_EXCEPT_SENSITIVE, ErrorMessage = Constants.REGEX_ALL_MESSAGE)]
        public string Website { get; set; }

        [Display(Name = "Link")]
        [Required]
        [MinLength(2)]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_ALL_EXCEPT_SENSITIVE, ErrorMessage = Constants.REGEX_ALL_MESSAGE)]
        public string ImageLink { get; set; }

    }
}
