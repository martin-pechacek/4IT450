using Semestralka.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace Semestralka.DataObjects
{
    public class UserDO
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool User_right { get; set; }

        /**
         *  Return list of all users in database
        **/
        public static async Task<List<UserDO>> GetUsersAsync()
        {
            using (Entities context = new Entities())
            {
                return await context.Users
                    .Select(x => new UserDO()
                    {
                        UserID = x.id_user,
                        Username = x.username,
                        User_right = x.user_right
                    }).ToListAsync();
            }
        }

        /**
         *  Return user depending on username
        **/
        public static User GetUserAsync(string username)
        {
            using (Entities context = new Entities())
            {
                return context.Users.Single(x => x.username == username);
            }
        }
    }
}