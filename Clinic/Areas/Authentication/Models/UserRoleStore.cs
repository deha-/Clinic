using Clinic.DAL;
using Clinic.Enums;
using Clinic.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Clinic.Areas.Authentication.Models
{
    public class UserRoleStore : IUserStore<User>, IUserRoleStore<User>, IRoleStore<Role>, IUserPasswordStore<User>
    {
        private ClinicDbContext db;

        public UserRoleStore(ClinicDbContext db)
        {
            this.db = db;
        }

        public Task CreateAsync(Role role)
        {
            Role r = db.Roles.Where(p => p.RoleName == role.RoleName).FirstOrDefault();
            if (r == null)
            {
                db.Roles.Add(role);
                db.SaveChanges();
            }

            return Task.FromResult(0);
        }

        public Task DeleteAsync(Role role)
        {
            Role r = db.Roles.Find(role.RoleId);
            if (r != null)
            {
                db.Roles.Remove(r);
                db.SaveChanges();
            }

            return Task.FromResult(0);
        }

        public Task<Role> FindByIdAsync(string roleId)
        {
            Role r = db.Roles.Find(Guid.Parse(roleId));

            return Task.FromResult(r);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            Role r = db.Roles.Where(p => p.RoleName == roleName).FirstOrDefault();

            return Task.FromResult(r);
        }

        public Task UpdateAsync(Role role)
        {
            Role r = db.Roles.Find(role.RoleId);
            if (r != null)
                db.Entry(r).CurrentValues.SetValues(role);
            db.SaveChanges();

            return Task.FromResult(0);
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            if (!this.IsInRoleAsync(user, roleName).Result)
            {
                Guid roleId = db.Roles.Where(p => p.RoleName == roleName).FirstOrDefault().RoleId;
                UserRole ur = new UserRole(user.UserId, roleId);
                db.UserRoles.Add(ur);
                db.SaveChanges();
            }

            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            IList<string> roles = db.Roles.Join(db.UserRoles, p => p.RoleId, u => u.RoleId, (p, u) => new { Role = p, UserRole = u }).Where(p => p.UserRole.UserId == user.UserId).Select(p => p.Role.RoleName).ToList();

            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            /*if (user.Roles == null)
                return Task.FromResult(false);
            else
            {
                if (user.Roles.Where(p => p.RoleName == roleName).Any())
                    return Task.FromResult(true);
                else
                    return Task.FromResult(false);
            }*/
            
            if (db.Roles.Join(db.UserRoles, p => p.RoleId, u => u.RoleId, (p, u) => new { Role = p, UserRole = u }).Where(p => p.UserRole.UserId == user.UserId && p.Role.RoleName == roleName).Any())
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            UserRole ur = db.UserRoles.Join(db.Roles, urol => urol.RoleId, rol => rol.RoleId, (urol, rol) => new { UserRole = urol, Role = rol }).Where(p => p.UserRole.UserId == user.UserId && p.Role.RoleName == roleName).Select(p => p.UserRole).FirstOrDefault();
            if (ur != null)
            {
                db.UserRoles.Remove(ur);
                db.SaveChanges();
            }

            return Task.FromResult(0);
        }

        public Task CreateAsync(User user)
        {
            Guid userId = db.Users.Add(user).UserId;
            db.SaveChanges();

            return Task.FromResult(0);
        }

        public Task DeleteAsync(User user)
        {
            User u = db.Users.Find(user.UserId);
            if (u != null)
            {
                db.Users.Remove(u);
                db.SaveChanges();
            }

            return Task.FromResult(0);
        }

        Task<User> IUserStore<User, string>.FindByIdAsync(string userId)
        {
            User u = db.Users.Find(Guid.Parse(userId));

            return Task.FromResult(u);
        }

        Task<User> IUserStore<User, string>.FindByNameAsync(string userName)
        {
            User u = db.Users.Where(p => p.Login == userName).FirstOrDefault();

            return Task.FromResult(u);
        }

        public Task UpdateAsync(User user)
        {
            User u = db.Users.Find(user.UserId);
            if (u != null)
                db.Entry(u).CurrentValues.SetValues(user);

            db.SaveChanges();

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(user.Password != null);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;

            return Task.FromResult(0);
        }
    }
}