using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


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
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string ImageLink { get; set; }
        public List<Review> Reviews { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [InverseProperty("Business")]
        public BusProperties BusinessProperty { get; set; }


        // Default Constructor without parameters
        public Business()
        {
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            Reviews = new List<Review>();
        }
        public Business(BusinessViewModel NewBusiness)
        {
            this.Name = NewBusiness.Name;
            this.CategoryId = NewBusiness.CategoryId;
            this.CategoryTypeId = NewBusiness.CategoryTypeId;
            this.Address = NewBusiness.Address;
            this.City = NewBusiness.City;
            this.State = NewBusiness.State;
            this.ZipCode = NewBusiness.ZipCode;

            string _phone = NewBusiness.Phone;
            _phone = _phone.Replace("(", "");
            _phone = _phone.Replace(")", "");
            _phone = _phone.Replace("-", "");
            _phone = _phone.Replace(" ", "");
            if (_phone[0] == '1')
            {
                _phone = _phone.Substring(1);
            }
            _phone = "(" + _phone.Substring(0,3) + ") " + _phone.Substring(3, 3) + "-" + _phone.Substring(6);
            this.Phone = _phone;
            this.Website = NewBusiness.Website;
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
            this.Reviews = new List<Review>();
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
        public string ZipCode { get; set; }

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
    }

    public class BizImageLinkImportModel : BaseEntity
    {
        [Display(Name = "File Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(251)]
        [RegularExpression(Constants.REGEX_ALL_EXCEPT_SENSITIVE, ErrorMessage = Constants.REGEX_ALL_MESSAGE)]
        public string FileName { get; set; }

        [Display(Name = "Extension")]
        [Required]
        [MinLength(2)]
        [MaxLength(4)]
        [RegularExpression(Constants.REGEX_IMG_EXT, ErrorMessage = Constants.REGEX_IMG_EXT_MESSAGE)]
        public string FileExtension { get; set; }
    }

    public class NewBizViewModel : BaseEntity
    {
        public BusinessViewModel BizVM { get; set; }
        public BusCategoryViewModel CategoryVM { get; set; }
        public BusCategoryTypeViewModel CategoryTypeVM { get; set; }
    }
}
