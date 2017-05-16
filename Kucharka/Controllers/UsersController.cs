using Microsoft.AspNet.Identity;
using Semestralka.DatabaseModels;
using Semestralka.Models;
using Semestralka.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Semestralka.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        /**
         *  Method for inserting user account into database
        **/
        [HttpPost]
        public ActionResult Index(User user)
        {
            using(Entities kucharkaEntities = new Entities())
            {
                IPasswordHasher hasher = new PasswordHasher();
                user.password = hasher.HashPassword(user.password);

                //sends data into database
                kucharkaEntities.Users.Add(user);
                //save changes in database
                kucharkaEntities.SaveChanges();
                //initialization variable for error message
                string message = string.Empty;

                //storing information message into variable depending on value returned from databse procedure
                switch (user.id_user)
                {
                    case -1:
                        message = "Username already exists.\\nPlease choose a different username.";
                        break;
                    case -2:
                        message = "Supplied email address has already been used.";
                        break;
                    default:
                        message = "Registration successful.\\nUser Id: " + user.id_user.ToString();
                        break;
                }  
            }
            return View();
        }

        /**
        *  Method for login view initialization 
        **/
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        /**
         *  Method for user login 
        **/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model) 
        {
            if(ModelState.IsValid)
            {
                //initialize new UserManager object
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore());

                //find user
                var user = await userManager.FindAsync(model.username, model.password);

                //if user exists proceed login. If not, show error message
                if(user != null)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.GetOwinContext().Authentication.SignIn(identity);

                    return RedirectToAction("Index", "Home");
                } 
                else
                {
                    ModelState.AddModelError(string.Empty, "Chybně zadané uživatelské jméno, nebo heslo");
                }
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
	}
}