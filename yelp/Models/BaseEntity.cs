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
        public const string REGEX_NAMES_MESSAGE = "Name fields may only contain letters, spaces, hyphens (-), commas(,) and periods (.)";
        public const string REGEX_LATITUDE = @"^([1-8]?\d(\.\d+)?\s[NS]?|90(\.0+)?\s[NS]?)$";
        public const string REGEX_LATITUDE_MESSAGE = "Latitude values must be between 0 and 90 degrees (decimal) North or South. Negative values are not valid. Submissions must be in decimal format with a space and letter (N or S) at the end.";
        public const string REGEX_LONGITUDE = @"^(180(\.0+)?\s[EW]|((1[0-7]\d)|([1-9]?\d))(\.\d+)?\s[EW])$";
        public const string REGEX_LONGITUDE_MESSAGE = "Longitude values must be between 0 and 180 degrees (decimal) East or West. Negative values are not valid. Submissions must be in decimal format with a space and letter (E or W) at the end.";
        public const string REGEX_ZIPCODE = @"^\d{5}(?:[-\s]\d{4})?$";
        public const string REGEX_ZIPCODE_MESSAGE = "Zip codes must have five digits (and may optionally include a four digit extension)";
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