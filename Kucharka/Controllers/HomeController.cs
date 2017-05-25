using Semestralka.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Semestralka.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            List<RecipeDO> recipes = await RecipeDO.GetRecipesAsync();

            ViewBag.Recipes = recipes;

            return View();
        }
	}
}