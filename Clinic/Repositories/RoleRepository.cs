using Clinic.Areas.Authentication.Models;
using Clinic.DAL;
using Clinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Repositories
{
    public class RoleRepository
    {
        private static ClinicDbContext db = new ClinicDbContext();

        public static void AddRole(string roleName)
        {
            Role role = new Role(roleName);

            db.Roles.Add(role);
            db.SaveChanges();
        }
    }
}