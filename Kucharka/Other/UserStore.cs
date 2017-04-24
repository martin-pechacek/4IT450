using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Semestralka.DatabaseModels;

namespace Semestralka.Other
{
    public class UserStore : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>, IUserSecurityStampStore<IdentityUser>
    {

        public Task CreateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> FindByIdAsync(string userId)
        {
            await Task.Delay(0);

            IdentityUser identityUser = new IdentityUser();
       
            using (KucharkaEntities context = new KucharkaEntities())
            {
                //Find user by user id
                User user = context.Users.Single(x => x.id_user == int.Parse(userId));

                //Store id and username into identityUser variable
                identityUser.Id = Convert.ToString(user.id_user);
                identityUser.UserName = user.username;
            }

            return identityUser;
        }

        public async Task<IdentityUser> FindByNameAsync(string userName)
        {
            await Task.Delay(0);

            IdentityUser identityUser = new IdentityUser();

            using (KucharkaEntities context = new KucharkaEntities())
            {
                //Finds user by username
                User user = context.Users.Single(x => x.username == userName);

                //Store id and username into identityUser variable
                identityUser.Id = Convert.ToString(user.id_user);
                identityUser.UserName = user.username;
            }

            return identityUser;
        }

        public Task UpdateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            return user.
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
        }

        private IdentityUser ToIdentityUser(IdentityUser user)
        {
            return new IdentityUser
            {
                Id = user.Id,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                UserName = user.UserName
            };
        }
    }
}