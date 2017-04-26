using Microsoft.AspNet.Identity;
using System;
using System.Web;

namespace Semestralka.Other
{
    public class IdentityUser : IUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
