using Clinic.Areas.Authentication.Models;
using Clinic.DAL;
using Clinic.Enums;
using Clinic.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Clinic.Repositories
{
    public class UserRepository
    {
        private static ClinicDbContext db = new ClinicDbContext();

        public static Guid AddUser(string login, string password)
        {
            User user = new User(login, password);
            Guid userId = db.Users.Add(user).UserId;
            db.SaveChanges();

            return userId;
        }

        public static Guid GetUserId(string UserName)
        {
            Guid userId = db.Users.Where(p => p.Login == UserName).FirstOrDefault().UserId;

            return userId;
        }

        public static void ActivateUser(string UserName)
        {
           User user = db.Users.Where(p => p.Login == UserName).FirstOrDefault();

           if (user != null)
           {
               user.IsApproved = true;
               db.SaveChanges();
           }
        }

        public static void ActivateUser(Guid UserId)
        {
            User user = db.Users.Find(UserId);

            if (user != null)
            {
                user.IsApproved = true;
                db.SaveChanges();
            }
        }
    }
}