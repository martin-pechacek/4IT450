using Semestralka.DataObjects;
using Semestralka.Other;
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

            List<string> imagesUrls = new List<string>();

            CloudinaryOperations cloudinary = new CloudinaryOperations();

            for(int i = 0; i < recipes.Count; i++)
            {
                recipes[i].ImageUrl = cloudinary.getImageUrl(recipes[i].RecipeID);
            }

            ViewBag.Recipes = recipes;

            return View();
        }
	}
}