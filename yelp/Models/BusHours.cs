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
        public bool MonClosed { get; set; }
        public DateTime? MonOpenTime { get; set; }
        public DateTime? MonCloseTime { get; set; }
        public bool TueClosed { get; set; }
        public DateTime? TueOpenTime { get; set; }
        public DateTime? TueCloseTime { get; set; }
        public bool WedClosed { get; set; }
        public DateTime? WedOpenTime { get; set; }
        public DateTime? WedCloseTime { get; set; }
        public bool ThuClosed { get; set; }
        public DateTime? ThuOpenTime { get; set; }
        public DateTime? ThuCloseTime { get; set; }
        public bool FriClosed { get; set; }
        public DateTime? FriOpenTime { get; set; }
        public DateTime? FriCloseTime { get; set; }
        public bool SatClosed { get; set; }
        public DateTime? SatOpenTime { get; set; }
        public DateTime? SatCloseTime { get; set; }
        public bool SunClosed { get; set; }
        public DateTime? SunOpenTime { get; set; }
        public DateTime? SunCloseTime { get; set; }
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

        public BusHours(BusHoursViewModel NewBizHoursVM)
        {
            this.MonOpenTime = NewBizHoursVM.MonOpenTime;
            this.MonCloseTime = NewBizHoursVM.MonCloseTime;
            this.TueOpenTime = NewBizHoursVM.TueOpenTime;
            this.TueCloseTime = NewBizHoursVM.TueCloseTime;
            this.WedOpenTime = NewBizHoursVM.WedOpenTime;
            this.WedCloseTime = NewBizHoursVM.WedCloseTime;
            this.ThuOpenTime = NewBizHoursVM.ThuOpenTime;
            this.ThuCloseTime = NewBizHoursVM.ThuCloseTime;
            this.FriOpenTime = NewBizHoursVM.FriOpenTime;
            this.FriCloseTime = NewBizHoursVM.FriCloseTime;
            this.SatOpenTime = NewBizHoursVM.SatOpenTime;
            this.SatCloseTime = NewBizHoursVM.SatCloseTime;
            this.SunOpenTime = NewBizHoursVM.SunOpenTime;
            this.SunCloseTime = NewBizHoursVM.SunCloseTime;
            this.AlwaysOpen = NewBizHoursVM.AlwaysOpen;
            this.MonClosed = NewBizHoursVM.MonClosed;
            this.TueClosed = NewBizHoursVM.TueClosed;
            this.WedClosed = NewBizHoursVM.WedClosed;
            this.ThuClosed = NewBizHoursVM.ThuClosed;
            this.FriClosed = NewBizHoursVM.FriClosed;
            this.SatClosed = NewBizHoursVM.SatClosed;
            this.SunClosed = NewBizHoursVM.SunClosed;            
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

    }

    public class BusHoursViewModel : BaseEntity
    {
        [Display(Name = "Monday Open")]
        [DataType(DataType.Time)]
        public DateTime? MonOpenTime { get; set; }

        [Display(Name = "Monday Closing Time")]
        [DataType(DataType.Time)]
        public DateTime? MonCloseTime { get; set; }

        [Display(Name = "Tuesday Closing Time")]
        [DataType(DataType.Time)]
        public DateTime? TueCloseTime { get; set; }

        [Display(Name = "Tuesday Open")]
        [DataType(DataType.Time)]
        public DateTime? TueOpenTime { get; set; }

        [Display(Name = "Wednesday Open")]
        [DataType(DataType.Time)]
        public DateTime? WedOpenTime { get; set; }

        [Display(Name = "Wednesday Closing Time")]
        [DataType(DataType.Time)]
        public DateTime? WedCloseTime { get; set; }

        [Display(Name = "Thursday Open")]
        [DataType(DataType.Time)]
        public DateTime? ThuOpenTime { get; set; }

        [Display(Name = "Thursday Closing Time")]
        [DataType(DataType.Time)]
        public DateTime? ThuCloseTime { get; set; }

        [Display(Name = "Friday Open")]
        [DataType(DataType.Time)]
        public DateTime? FriOpenTime { get; set; }

        [Display(Name = "Friday Closing Time")]
        [DataType(DataType.Time)]
        public DateTime? FriCloseTime { get; set; }

        [Display(Name = "Saturday Open")]
        [DataType(DataType.Time)]
        public DateTime? SatOpenTime { get; set; }

        [Display(Name = "Saturday Closing Time")]
        [DataType(DataType.Time)]
        public DateTime? SatCloseTime { get; set; }

        [Display(Name = "Sunday Open")]
        [DataType(DataType.Time)]
        public DateTime? SunOpenTime { get; set; }

        [Display(Name = "Sunday Closing Time")]
        [DataType(DataType.Time)]
        public DateTime? SunCloseTime { get; set; }

        [Display(Name = "Monday Closed")]
        public bool MonClosed { get; set; }

        [Display(Name = "Tuesday Closed")]
        public bool TueClosed { get; set; }

        [Display(Name = "Wednesday Closed")]
        public bool WedClosed { get; set; }

        [Display(Name = "Thursday Closed")]
        public bool ThuClosed { get; set; }

        [Display(Name = "Friday Closed")]
        public bool FriClosed { get; set; }

        [Display(Name = "Saturday Closed")]
        public bool SatClosed { get; set; }

        [Display(Name = "Sunday Closed")]
        public bool SunClosed { get; set; }

        [Display(Name = "Open 24 Hours")]
        public bool AlwaysOpen { get; set; }
    }
}
