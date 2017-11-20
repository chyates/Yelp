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
        public int CategoryTypeId { get; set; }
        public BusCategoryType CategoryType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public int Phone { get; set; }
        public int Website { get; set; }
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
        [Display(Name = "Name")]
        [Required]
        [MinLength(2, ErrorMessage = "Names must have at least 2 characters")]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_NAMES, ErrorMessage = Constants.REGEX_NAMES_MESSAGE)]
        public string Name { get; set; }
    }
}
