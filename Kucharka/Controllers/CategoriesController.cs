using Semestralka.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Semestralka.Controllers
{
    public class CategoriesController : Controller
    {
        //
        // GET: /Categories/
        public ActionResult Index()
        {
            return View();
        }

        /**
        *  Method for inserting new category into database
        **/
        [HttpPost]
        public ActionResult Index(Category category)
        {
            using (KucharkaEntities kucharkaEntities = new KucharkaEntities())
            {
                //sends data into database
                kucharkaEntities.Categories.Add(category);
                //save changes in database
                kucharkaEntities.SaveChanges();
                //initialization variable for error message
                string message = string.Empty;

                //storing information message into variable depending on value returned from databse procedure
                switch (category.id_category)
                {
                    case -1:
                        message = "Kategorie již existuje.\\nVyberte jiné jméno.";
                        break;
                    default:
                        message = "Kategorie přidána.\\nId: " + category.id_category.ToString();
                        break;
                }
            }
            return View();
        }
	}
}