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
        *  Method for inserting new recipe into database
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
        public async Task<ActionResult> Recipe(RecipeDetailModel model, string add, Recipe_Ingredient recipeIngredient)
        {
            int id = int.Parse(model.IngredientModel.Id_ingredient);

            IngredientsDO unit = await IngredientsDO.GetUnitAsync(id);

            ViewBag.Unit = unit.Unit;

            Recipe recipe = await RecipeDO.GetRecipeAsync(model.RecipeModel.Id_recipe);

            ViewBag.Recipe = recipe;

            List<IngredientsDO> ingredients = await IngredientsDO.GetIngredientsAsync();

            ViewBag.IngredientsList = ingredients
                .Select(x => new SelectListItem()
                {
                    Text = x.IngredientName,
                    Value = x.IngredientID.ToString()
                })
                .ToList();

            recipeIngredient.id_ingredient = Convert.ToInt16(id);
            recipeIngredient.id_recipe = Convert.ToInt16(model.RecipeModel.Id_recipe);

            //control if "add ingredient" button was pressed or was chosen another ingredient from dropdown list
            if (add != null)
            {
                using (Entities kucharkaEntities = new Entities())
                {
                    //sends data into database
                    kucharkaEntities.Recipe_Ingredient.Add(recipeIngredient);
                    //save changes in database
                    kucharkaEntities.SaveChanges();
                    //initialization variable for error message
                    string message = string.Empty;
                    //information message
                    message = "Ingredience přidána.";

                    return View();
                }
            }
            else
            {
                return View(model);
            }
        }

        /**
        *  Shows recipe detail
        **/
        public async Task<ActionResult> Recipe(int id)
        {
            RecipeDetailModel model = new RecipeDetailModel();

            Recipe recipe = await RecipeDO.GetRecipeAsync(id);

            model.RecipeModel = new RecipeModel()
            {
                Id_recipe = recipe.id_recipe,
                Id_category = Convert.ToString(recipe.id_category),
                Name_recipe = recipe.name_recipe,
                Instructions = recipe.instructions
            };

            ViewBag.Recipe = recipe;

            List<IngredientsDO> IngredientsDropDown = await IngredientsDO.GetIngredientsAsync();

            ViewBag.IngredientsList = IngredientsDropDown
                .Select(x => new SelectListItem()
                {
                    Text = x.IngredientName,
                    Value = x.IngredientID.ToString()
                })
                .ToList();

            List<RecipeIngredientsDO> ingredients = await RecipeIngredientsDO.GetIngredientsAsync(recipe.id_recipe);

            List<string> ingredientsNamesList = new List<string>();

            for (int i = 0; i < ingredients.Count; i++ )
            {
                int ingredientID = ingredients.ElementAt(i).IngredientID;
                string ingredientName = await IngredientsDO.GetNameAsync(ingredientID);
                ingredientsNamesList.Add(ingredientName);
            }

            ViewBag.Ingredients = ingredients;
            ViewBag.IngredientsNames = ingredientsNamesList;

            IngredientsDO unit = await IngredientsDO.GetFirstUnitAsync();

            ViewBag.Unit = unit.Unit;

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