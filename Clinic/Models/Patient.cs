using Clinic.Areas.Authentication.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public class Patient
    {
        [Key]
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public Patient() { }

        public Patient(string FirstName, string LastName, string PESEL, string Address, string City, string PostalCode)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PESEL = PESEL;
            this.Address = Address;
            this.City = City;
            this.PostalCode = PostalCode;
        }

        public Patient(Guid UserId, string FirstName, string LastName, string PESEL, string Address, string City, string PostalCode)
        {
            this.UserId = UserId;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.PESEL = PESEL;
            this.Address = Address;
            this.City = City;
            this.PostalCode = PostalCode;
        }
    }
}