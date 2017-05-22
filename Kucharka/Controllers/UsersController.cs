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
            try
            {
                using (Entities context = new Entities())
                {
                    IPasswordHasher hasher = new PasswordHasher();
                    user.password = hasher.HashPassword(user.password);

                    //sends data into database
                    context.Users.Add(user);
                    //save changes in database
                    context.SaveChanges();
                    //initialization variable for error message
                    string message = string.Empty;

                    //storing information message into variable depending on value returned from databse procedure
                    switch (user.id_user)
                    {
                        case -1:
                            ModelState.AddModelError("Username", "Uživatelské jméno již existuje");
                            break;
                        default:
                            message = "Registration successful.\\nUser Id: " + user.id_user.ToString();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("password"))
                {
                    ModelState.AddModelError("Password", "Heslo musí být vyplněné");
                }
                if(user.username == null)
                {
                    ModelState.AddModelError("Username", "Uživatelské jméno musí být vyplněné");
                }
                if (user.firstname == null)
                {
                    ModelState.AddModelError("Firstname", "Jméno musí být vyplněné");
                }
                if (user.lastname == null)
                {
                    ModelState.AddModelError("Lastname", "Příjmení musí být vyplněné");
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
                try {
                    //find user
                    var user = await userManager.FindAsync(model.username, model.password);
                    
                    HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.GetOwinContext().Authentication.SignIn(identity);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Špatně zadané uživatelské jméno, nebo heslo");
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