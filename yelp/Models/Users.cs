using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace yelp.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public List<Review> Reviews { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Default Constructor without parameters
        public User()
        {
            Reviews = new List<Review>();
        }
        
        // Constructor based on UserRegViewModel
        public User(UserRegViewModel RegUser)
        {
            this.FirstName = RegUser.FirstName;
            this.LastName = RegUser.LastName;
            this.ZipCode = RegUser.ZipCode;
            this.Email = RegUser.Email;
            this.Password = RegUser.Password;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }
    }

    public class UserRegViewModel : BaseEntity
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "First name")]
        [Required]
        [MinLength(2, ErrorMessage = "First Name must have at least 2 characters")]
        [MaxLength(45)]
        [RegularExpression(Constants.REGEX_NAMES, ErrorMessage = Constants.REGEX_NAMES_MESSAGE)]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        [Required]
        [MinLength(2, ErrorMessage = "Last Name must have at least 2 characters")]
        [MaxLength(255)]
        [RegularExpression(Constants.REGEX_NAMES, ErrorMessage = Constants.REGEX_NAMES_MESSAGE)]
        public string LastName { get; set; }
        [Display(Name = "Zip Code")]
        [Required]
        [MinLength(5)]
        [MaxLength(10)]
        [RegularExpression(Constants.REGEX_ZIPCODE, ErrorMessage = Constants.REGEX_ZIPCODE_MESSAGE)]
        public string ZipCode { get; set; }
        [Display(Name = "Email")]
        [Required]
        [MaxLength(255)]
        [EmailAddress(ErrorMessage = "The email address is not in the proper format: address@domain.ext")]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required]
        [MinLength(8, ErrorMessage = "Password must have at least 8 characters")]
        [MaxLength(255)]
        [DataType(DataType.Password)]
        [RegularExpression(Constants.REGEX_ALL_EXCEPT_SENSITIVE, ErrorMessage = Constants.REGEX_ALL_MESSAGE)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation do not match")]
        [DataType(DataType.Password)]
        [RegularExpression(Constants.REGEX_ALL_EXCEPT_SENSITIVE, ErrorMessage = Constants.REGEX_ALL_MESSAGE)]
        public string ConfirmPassword { get; set; }
    }

    public class UserLoginViewModel : BaseEntity
    {
        [Display(Name = "Email")]
        [Required]
        [MaxLength(255)]
        [EmailAddress(ErrorMessage = "The email address is not in the proper format: address@domain.ext")]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class LoginRegFormModel : BaseEntity
    {
        public UserLoginViewModel loginVM { get; set; }
        public UserRegViewModel registerVM { get; set; }
    }
}