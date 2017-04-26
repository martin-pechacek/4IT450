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
                //parse userid from string to int
                int id = int.Parse(userId);
                //Find user by user id
                User user = context.Users.Single(x => x.id_user == id);

                //Store id and username into identityUser variable
                identityUser.Id = userId;
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

        public async Task<string> GetPasswordHashAsync(IdentityUser identityUser)
        {
            await Task.Delay(0);

            using (KucharkaEntities context = new KucharkaEntities())
            {
                //parse userid from string to int
                int userId = int.Parse(identityUser.Id);

                //Find user by user id
                User user = context.Users.Single(x => x.id_user == userId);

                //if user was found return hashed password
                if (user != null)
                {
                    return user.password;
                }

                throw new ApplicationException("Invalid user");
            }
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.Password = passwordHash;

            return Task.FromResult<Object>(null);
        }

        public async Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            await Task.Delay(0);

            //if user exists return security stamp
            if(user != null){
                return Guid.NewGuid().ToString("D");
            }

            throw new ApplicationException("Invalid user.");
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            throw new NotImplementedException();
        }
    }
}