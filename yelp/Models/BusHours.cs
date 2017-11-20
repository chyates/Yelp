using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace yelp.Models
{
    public class BusHours : BaseEntity
    {
        public int BusHoursId { get; set; }
        public int BusinessId { get; set; }
        public Business Business { get; set; }
        public DateTime MonOpenTime { get; set; }
        public DateTime MonCloseTime { get; set; }
        public DateTime TueCloseTime { get; set; }
        public DateTime TueOpenTime { get; set; }
        public DateTime WedOpenTime { get; set; }
        public DateTime WedCloseTime { get; set; }
        public DateTime ThuOpenTime { get; set; }
        public DateTime ThuCloseTime { get; set; }
        public DateTime FriOpenTime { get; set; }
        public DateTime FriCloseTime { get; set; }
        public DateTime SatOpenTime { get; set; }
        public DateTime SatCloseTime { get; set; }
        public DateTime SunOpenTime { get; set; }
        public DateTime SunCloseTime { get; set; }
        public bool AlwaysOpen { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default Constructor without parameters
        public BusHours()
        {
            this.AlwaysOpen = false;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
    }

}
