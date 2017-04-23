using Semestralka.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
            using(KucharkaEntities kucharkaEntities = new KucharkaEntities())
            {
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

        public ActionResult Login() 
        {
            return View();
        }
	}
}