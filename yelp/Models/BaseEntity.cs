using System;
using System.ComponentModel.DataAnnotations;

namespace yelp.Models
{
    public abstract class BaseEntity { }

    public static class Constants
    {
        public const string REGEX_ALL_EXCEPT_SENSITIVE = @"^[^'{}<>\\\u0022]+$";
        public const string REGEX_ALL_MESSAGE = @"This field can accept letters, numbers, and special characters except single quote, carat, curly and angle brackets, and backslash. Please correct and resubmit your submission.";
        public const string REGEX_USERNAMES = @"^[-_a-zA-Z0-9\.]+$";
        public const string REGEX_USERNAMES_MESSAGE = "Username may only contain letters, underscore (_), hyphen (-), and periods";
        public const string REGEX_NAMES = @"^[-a-zA-Z,\.\s]+$";
        public const string REGEX_NAMES_MESSAGE = "This field may only contain letters, spaces, hyphens (-), commas(,) and periods (.)";
        public const string REGEX_LATITUDE = @"^([1-8]?\d(\.\d+)?\s[NS]?|90(\.0+)?\s[NS]?)$";
        public const string REGEX_LATITUDE_MESSAGE = "Latitude values must be between 0 and 90 degrees (decimal) North or South. Negative values are not valid. Submissions must be in decimal format with a space and letter (N or S) at the end.";
        public const string REGEX_LONGITUDE = @"^(180(\.0+)?\s[EW]|((1[0-7]\d)|([1-9]?\d))(\.\d+)?\s[EW])$";
        public const string REGEX_LONGITUDE_MESSAGE = "Longitude values must be between 0 and 180 degrees (decimal) East or West. Negative values are not valid. Submissions must be in decimal format with a space and letter (E or W) at the end.";
        public const string REGEX_ZIPCODE = @"^\d{5}(?:[-\s]\d{4})?$";
        public const string REGEX_ZIPCODE_MESSAGE = "Zip codes must have five digits (and may optionally include a four digit extension)";
        public const string REGEX_DIGITS_ONLY = @"^\d{1,}$";
        public const string REGEX_DIGITS_MESSAGE = "This field can only accept digits 0-9";
        public const string REGEX_STATES = @"^((A[LKSZR])|(C[AOT])|(D[EC])|(F[ML])|(G[AU])|(HI)|(I[DLNA])|(K[SY])|(LA)|(M[EHDAINSOT])|(N[EVHJMYCD])|(MP)|(O[HKR])|(P[WAR])|(RI)|(S[CD])|(T[NX])|(UT)|(V[TIA])|(W[AVIY]))$";
        public const string REGEX_STATES_MESSAGE = "This is not a valid state.";
        public const string REGEX_PHONE = @"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$";
        public const string REGEX_PHONE_MESSAGE = "This is not a valid phone number";
        public const string REGEX_IMG_EXT = @"^(.*\.(jpg|jpeg|png|bmp|svg)$)$";
        public const string REGEX_IMG_EXT_MESSAGE = "The file is not in a valid format (.jpg, .jpeg, .png, .bmp, .svg).";




        public static string UppercaseFirst(string str)
        {
            // Check for empty string
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            // Return char and concat substring
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }

    public class CheckDateAfterToday : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dt = (DateTime)value;
            if (dt > DateTime.UtcNow)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "The date must be set in the future (after today's date)");
        }
    }

    public class CheckDateBeforeToday : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dt = (DateTime)value;
            if (dt < DateTime.UtcNow)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "The date must be set in the past (before today's date)");
        }
    }

}