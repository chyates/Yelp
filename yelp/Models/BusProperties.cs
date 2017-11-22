using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace yelp.Models
{
    public class BusProperties : BaseEntity
    {
        public int BusPropertiesId { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }

        // General Properties for All Businesses
        public bool creditcards { get; set; }
        public bool bikeparking { get; set; }
        public bool wheelchair { get; set; }
        public bool kidfriendly { get; set; }
        public bool wifi { get; set; }
        public int price { get; set; }
        public string parkwhere { get; set; }
        public string goodforTimeOfDay { get; set; }

        // Non-Restaurant Properties
        public bool ByApointOnly { get; set; }


        // Restaurant Properties
        public bool reservations { get; set; }
        public bool delivery { get; set; }
        public bool takeout { get; set; }
        public bool groupfriendly { get; set; }
        public bool alcohol { get; set; }
        public bool outdoor { get; set; }
        public bool waiter { get; set; }
        public bool caters { get; set; }
        public string ambience { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default Constructor without parameters
        public BusProperties()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        public BusProperties(BizPropertiesViewModel BizPropVM)
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
    }

    public class BizPropertiesViewModel : BaseEntity
    {
        // General Properties for All Businesses
        [Display(Name = "Credit Card")]
        public bool creditcards { get; set; }
        [Display(Name = "Bike Parking")]
        public bool bikeparking { get; set; }
        [Display(Name = "Wheelchair")]
        public bool wheelchair { get; set; }
        [Display(Name = "Kid Friendly")]
        public bool kidfriendly { get; set; }
        [Display(Name = "Wifi")]
        public bool wifi { get; set; }
        [Display(Name = "Price")]
        [Range(0,4)]
        public int price { get; set; }
        [Display(Name = "Parking")]
        [Required]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_ALL_EXCEPT_SENSITIVE, ErrorMessage = Constants.REGEX_ALL_MESSAGE)]
        public string parkwhere { get; set; }
        [Display(Name = "Time of Day")]
        [Required]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_ALL_EXCEPT_SENSITIVE, ErrorMessage = Constants.REGEX_ALL_MESSAGE)]
        public string goodforTimeOfDay { get; set; }

        // Non-Restaurant Properties
        [Display(Name = "By Appointment")]
        public bool ByApointOnly { get; set; }

        // Restaurant Properties
        [Display(Name = "Reservations")]
        public bool reservations { get; set; }
        [Display(Name = "Delivery")]
        public bool delivery { get; set; }
        [Display(Name = "Takeout")]
        public bool takeout { get; set; }
        [Display(Name = "Group Friendly")]
        public bool groupfriendly { get; set; }
        [Display(Name = "Alcohol Served")]
        public bool alcohol { get; set; }
        [Display(Name = "Outdoor Seating")]
        public bool outdoor { get; set; }
        [Display(Name = "Wait Service")]
        public bool waiter { get; set; }
        [Display(Name = "Catering")]
        public bool caters { get; set; }
        [Display(Name = "Ambience")]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_ALL_EXCEPT_SENSITIVE, ErrorMessage = Constants.REGEX_ALL_MESSAGE)]
        public string ambience { get; set; }
    }
}
