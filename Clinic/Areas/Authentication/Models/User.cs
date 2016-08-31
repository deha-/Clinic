using Clinic.DAL;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Clinic.Areas.Authentication.Models
{
    public class User : IUser
    {
        [Key]
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        //public DateTime? LastLoginDate { get; set; }
        public bool IsApproved { get; set; }

        [NotMapped]
        public string FirstName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
        [NotMapped]
        public string PESEL { get; set; }
        [NotMapped]
        public string Address { get; set; }
        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public string PostalCode { get; set; }

        [NotMapped]
        public virtual ICollection<Role> Roles { get; set; }

        public User() { }

        public User(string Login, string Password)
        {
            UserManager<User> userManager = new UserManager<User>(new UserRoleStore(new ClinicDbContext()));

            this.UserId = Guid.NewGuid();
            this.Login = Login;
            this.Password = userManager.PasswordHasher.HashPassword(Password);
            this.CreateDate = DateTime.Now;
            //this.LastLoginDate = null;
            this.IsApproved = false;
        }

        [NotMapped]
        public string Id
        {
            get
            {
                return this.UserId.ToString();
            }
        }

        [NotMapped]
        public string UserName
        {
            get
            {
                return this.Login;
            }

            set
            {
                this.Login = value;
            }
        }
    }
}