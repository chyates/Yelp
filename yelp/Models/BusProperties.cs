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
        public string goodforTimeOfDay { get; set; }
        public string ambience { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default Constructor without parameters
        public BusProperties()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
    }

}
