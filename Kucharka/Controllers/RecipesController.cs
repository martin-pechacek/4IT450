using Semestralka.DatabaseModels;
using Semestralka.DataObjects;
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
    public class RecipesController : Controller
    {
        IngredientsOperations ingredientsOperations = new IngredientsOperations();

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

            List<RecipeIngredientsDO> ingredientsList = await RecipeIngredientsDO.GetIngredientsAsync(recipe.id_recipe);

            ViewBag.IngredientsDropDown = ingredientsOperations.GetIngredientsDropDown(await IngredientsDO.GetIngredientsAsync());

            //Used for displaying amount
            ViewBag.Ingredients = ingredientsList;

            //Names of ingredients used in recipe
            ViewBag.IngredientsNames = await ingredientsOperations.GetIngredientsNames(ingredientsList);

            ViewBag.IngredientUnits = await ingredientsOperations.GetUnitNames(ingredientsList);

            recipeIngredient.id_ingredient = Convert.ToInt16(id);
            recipeIngredient.id_recipe = Convert.ToInt16(model.RecipeModel.Id_recipe);

            //control if "add ingredient" button was pressed or was chosen another ingredient from dropdown list
            if (add != null)
            {
                using (Entities context = new Entities())
                {
                    //initialization variable for error message
                    string message = string.Empty;

                    //find ingredient
                    try
                    {
                        var ingredient = context.Recipe_Ingredient.Single(x => x.id_recipe == recipeIngredient.id_recipe && x.id_ingredient == recipeIngredient.id_ingredient);
                        message = "Ingredience již existuje.\\n";
                    }
                    catch(Exception ex)
                    {
                        //message = ex.Message;
                        //sends data into database
                        context.Recipe_Ingredient.Add(recipeIngredient);
                        //save changes in database
                        context.SaveChanges();
                    }
                }
                return RedirectToAction("Recipe", "Recipes", new { id = recipe.id_recipe });
            }
            else
            {
                return View(model);
            }
        }

        /**
        *  Shows recipe detail
        **/
        public async Task<ActionResult> Recipe(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else 
            { 

                //Initialize model
                RecipeDetailModel model = new RecipeDetailModel();
                //Get one re cipe depending on id
                Recipe recipe = await RecipeDO.GetRecipeAsync((int)id);
            
                model.RecipeModel = new RecipeModel()
                {
                    Id_recipe = recipe.id_recipe,
                    Id_category = Convert.ToString(recipe.id_category),
                    Name_recipe = recipe.name_recipe,
                    Instructions = recipe.instructions
                };

                //unit which shows next to the textbox for amount of ingredient. Used only for first loading
                IngredientsDO unit = await IngredientsDO.GetFirstUnitAsync();

                List<RecipeIngredientsDO> ingredientsList = await RecipeIngredientsDO.GetIngredientsAsync(recipe.id_recipe);            

                ViewBag.IngredientsDropDown = ingredientsOperations.GetIngredientsDropDown(await IngredientsDO.GetIngredientsAsync());

                //Used for displaying amount
                ViewBag.Ingredients = ingredientsList;

                //Names of ingredients used in recipe
                ViewBag.IngredientsNames = await ingredientsOperations.GetIngredientsNames(ingredientsList);

                ViewBag.IngredientUnits = await ingredientsOperations.GetUnitNames(ingredientsList);
            
                //Unit shown next to the textbox for inserting amount
                ViewBag.Unit = unit.Unit;

                ViewBag.Recipe = recipe;

                return View(model);
            }
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
                //initialization variable for error message
                string message = string.Empty;

                //store information message into variable depending on value returned from database procedure
                switch (ingredient.id_ingredient)
                {
                    case -1:
                        message = "Ingredience již existuje.\\n";
                        break;
                    default:
                        //save changes in database
                        kucharkaEntities.SaveChanges();
                        message = "Ingredience přidána.\\nId: " + ingredient.id_ingredient.ToString();
                        break;
                }
            }
            return RedirectToAction("Ingredients", "Recipes");
        }

        public ActionResult RemoveIngredient(int recipeID, int ingredientID) 
        {
            using (Entities context = new Entities())
            {
                //find ingredient
                var ingredient = context.Recipe_Ingredient.Single(x => x.id_recipe == recipeID && x.id_ingredient == ingredientID);
                //remove ingredient
                context.Recipe_Ingredient.Remove(ingredient);
                //save changes in database
                context.SaveChanges();
            }

            return RedirectToAction("Recipe", "Recipes", new { id = recipeID });
        }

        public ActionResult Ingredients()
        {
            return View();
        }
	}
}