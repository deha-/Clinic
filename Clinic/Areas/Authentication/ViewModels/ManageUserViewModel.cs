using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Clinic.Areas.Authentication.ViewModels
{
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PESEL")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Invalid PESEL.")]
        public string PESEL { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        [RegularExpression(@"^[0-9]{2}-[0-9]{3}$", ErrorMessage = "You can not have that")]
        public string PostalCode { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string Password { get; set; }

        public ManageUserViewModel() { }

        public ManageUserViewModel(string FirstName, string LastName, string PESEL, string Address, string City, string PostalCode)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PESEL = PESEL;
            this.Address = Address;
            this.City = City;
            this.PostalCode = PostalCode;
        }
    }
}