using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public class UserRole
    {
        [Key]
        [Column(Order = 1)]
        [Required]
        public Guid UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        [Required]
        public Guid RoleId { get; set; }

        public UserRole() { }

        public UserRole(Guid UserId, Guid RoleId) 
        {
            this.UserId = UserId;
            this.RoleId = RoleId;
        }
    }
}