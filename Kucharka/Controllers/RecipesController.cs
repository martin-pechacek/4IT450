using Semestralka.DatabaseModels;
using Semestralka.DataObjects;
using Semestralka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Semestralka.Controllers
{
    public class RecipesController : Controller
    {
        //
        // GET: /Recipes/
        public async Task<ActionResult> Index(RecipeModel model)
        {
            List<CategoryDO> categories = await CategoryDO.GetCategoriesAsync();

            ViewBag.CategoryList = categories
                .Select(x => new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.CategoryID.ToString()
                })
                .ToList();

            return View(model);
        }

        /**
        *  Method for inserting new category into database
        **/
        [HttpPost]
        public async Task<ActionResult> Index(Recipe recipe)
        {
            await Task.Delay(0);

            using (Entities kucharkaEntities = new Entities())
            {
                //sends data into database
                kucharkaEntities.Recipes.Add(recipe);
                //save changes in database
                kucharkaEntities.SaveChanges();
                //initialization variable for error message
                string message = string.Empty;

                //store information message into variable depending on value returned from database procedure
                switch (recipe.id_recipe)
                {
                    case -1:
                        message = "Recept již existuje.\\n";
                        break;
                    default:
                        message = "Recept přidán.\\nId: " + recipe.id_category.ToString();
                        break;
                }
            }
            return RedirectToAction("Recipe", "Recipes", new { id = recipe.id_recipe });
        }

        [HttpPost]
        public async Task<ActionResult> Recipe(RecipeDetailModel model)
        {
            int id = int.Parse(model.IngredientModel.id_ingredient);

            IngredientsDO unit = await IngredientsDO.GetUnitAsync(id);

            ViewBag.Unit = unit.Unit;

            List<RecipeDO> recipe = await RecipeDO.GetRecipeAsync(model.RecipeModel.id_recipe);

            ViewBag.Recipe = recipe;

            List<IngredientsDO> ingredients = await IngredientsDO.GetIngredientsAsync();

            ViewBag.IngredientsList = ingredients
                .Select(x => new SelectListItem()
                {
                    Text = x.IngredientName,
                    Value = x.IngredientID.ToString()
                })
                .ToList();

            return View(model);
        }

        /**
        *  Shows recipe detail
        **/
        public async Task<ActionResult> Recipe(int id)
        {
            List<RecipeDO> recipe = await RecipeDO.GetRecipeAsync(id);

            ViewBag.Recipe = recipe;

            List<IngredientsDO> ingredients = await IngredientsDO.GetIngredientsAsync();

            ViewBag.IngredientsList = ingredients
                .Select(x => new SelectListItem()
                {
                    Text = x.IngredientName,
                    Value = x.IngredientID.ToString()
                })
                .ToList();

            var model = new RecipeDetailModel();
                        
            return View(model);
        }

        /**
         *  Method for inserting new ingredient into database
        **/
        [HttpPost]
        public async Task<ActionResult> Ingredients(Ingredient ingredient)
        {
            await Task.Delay(0);

            using (Entities kucharkaEntities = new Entities())
            {
                kucharkaEntities.Ingredients.Add(ingredient);
                //save changes in database
                kucharkaEntities.SaveChanges();
                //initialization variable for error message
                string message = string.Empty;

                //store information message into variable depending on value returned from database procedure
                switch (ingredient.id_ingredient)
                {
                    case -1:
                        message = "Ingredience již existuje.\\n";
                        break;
                    default:
                        message = "Ingredience přidána.\\nId: " + ingredient.id_ingredient.ToString();
                        break;
                }
            }
            return RedirectToAction("Ingredients", "Recipes");
        }

        public ActionResult Ingredients()
        {
            return View();
        }
	}
}