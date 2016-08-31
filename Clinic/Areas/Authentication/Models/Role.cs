using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Clinic.Areas.Authentication.Models
{
    public class Role : IRole
    {
        [Key]
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }

        public Role() { }

        public Role(string RoleName)
        {
            this.RoleId = Guid.NewGuid();
            this.RoleName = RoleName;
        }

        [NotMapped]
        public string Id
        {
            get 
            { 
                return this.RoleId.ToString(); 
            }
        }

        [NotMapped]
        public string Name
        {
            get
            {
                return this.RoleName;
            }
            set
            {
                this.RoleName = value;
            }
        }
    }
}