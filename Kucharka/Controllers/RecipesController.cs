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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;

namespace Semestralka.Controllers
{
    public class RecipesController : Controller
    {
        IngredientsOperations ingredientsOperations = new IngredientsOperations();

        //
        // GET: /Recipes/
        public async Task<ActionResult> Index(RecipeModel model, bool? error)
        {
            if(error == true)
            {
                ModelState.AddModelError(string.Empty, "Recept už existuje");
            }
            List<RecipeDO> recipes = await RecipeDO.GetRecipesAsync();
            
            List<CategoryDO> categories = await CategoryDO.GetCategoriesAsync();

            ViewBag.RecipesList = recipes;

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
        *  Method for inserting new recipe into database (without ingredients)
        **/
        [HttpPost]
        public async Task<ActionResult> Index(Recipe recipe, string add, RecipeModel model)
        {
            await Task.Delay(0);

            List<RecipeDO> recipes = await RecipeDO.GetRecipesAsync();

            List<CategoryDO> categories = await CategoryDO.GetCategoriesAsync();

            ViewBag.RecipesList = recipes;

            ViewBag.CategoryList = categories
                .Select(x => new SelectListItem()
                {
                    Text = x.CategoryName,
                    Value = x.CategoryID.ToString()
                })
                .ToList();

            var file = Request.Files[0];

            if (!(file.FileName.Contains(".jpg") || file.FileName.Contains(".gif") || file.FileName.Contains(".jpeg") || file.FileName.Contains(".png")))
            {
                if (file.ContentLength != 0)
                {
                    ModelState.AddModelError("Image", "Špatný formát souboru");

                    return View(model);
                }
            }


            try
            {
                using (Entities context = new Entities())
                {
                    //sends data into database
                    context.Recipes.Add(recipe);
                    //save changes in database
                    context.SaveChanges();

                    if (recipe.id_recipe == -1)
                    {
                        return RedirectToAction("Index", "Recipes", new { error = true });
                    }
                }
            }
            catch (Exception ex)
            {
                if (model.Name_recipe == null)
                {
                    ModelState.AddModelError("RecipeName", "Název receptu musí být vyplněný");
                }
                if (model.Instructions == null)
                {
                    ModelState.AddModelError("InstructionsArea", "Instrukce musejí být vyplněné");
                }

                return View();
            }


            CloudinaryOperations cloudinary = new CloudinaryOperations();

            cloudinary.Upload(file, recipe.id_recipe);

            return RedirectToAction("Recipe", "Recipes", new { id = recipe.id_recipe });
        }

        /**
         *  Method for adding ingredient to recipe and refreshing unit label in case selection new value in DropDown 
        **/
        [HttpPost]
        public async Task<ActionResult> Recipe(RecipeDetailModel model, string add, Recipe_Ingredient recipeIngredient)
        {
            int id = int.Parse(model.IngredientModel.Id_ingredient);

            //Get one recipe depending on id
            Recipe recipe = await RecipeDO.GetRecipeAsync(model.RecipeModel.Id_recipe);

            model.RecipeModel = new RecipeModel()
            {
                Id_recipe = recipe.id_recipe,
                Id_category = recipe.id_category,
                Name_recipe = recipe.name_recipe,
                Instructions = recipe.instructions
            };

            List<CategoryDO> categories = await CategoryDO.GetCategoriesAsync();

            Category category = await CategoryDO.GetCategoryAsync(model.RecipeModel.Id_category);

            ViewBag.Category = category;

            IngredientsDO unit = await IngredientsDO.GetUnitAsync(id);

            ViewBag.Unit = unit.Unit;

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
                    //find ingredient
                    try
                    {
                        var ingredient = context.Recipe_Ingredient.Single(x => x.id_recipe == recipeIngredient.id_recipe && x.id_ingredient == recipeIngredient.id_ingredient);
                    }
                    catch(Exception ex)
                    {
                        //this code block is in catch clause to prevent multiple adding same ingredient. (Single() returns null => ingredient was not added before)  

                        //send data into database
                        context.Recipe_Ingredient.Add(recipeIngredient);
                        //save change in database
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
        [AllowAnonymous]
        public async Task<ActionResult> Recipe(int? id)
        {

            //Initialize model
            RecipeDetailModel model = new RecipeDetailModel();
            try
            {
                //Get one recipe depending on id
                Recipe recipe = await RecipeDO.GetRecipeAsync((int)id);

                model.RecipeModel = new RecipeModel()
                {
                    Id_recipe = recipe.id_recipe,
                    Id_category = recipe.id_category,
                    Name_recipe = recipe.name_recipe,
                    Instructions = recipe.instructions
                };

                Category category = await CategoryDO.GetCategoryAsync(model.RecipeModel.Id_category);

                ViewBag.Category = category;

                //unit which shows next to the textbox for amount of ingredient. Used only for first loading
                IngredientsDO unit = await IngredientsDO.GetFirstUnitAsync();

                List<RecipeIngredientsDO> ingredientsList = await RecipeIngredientsDO.GetIngredientsAsync(recipe.id_recipe);

                //Used for displaying category name
                ViewBag.Category = await CategoryDO.GetCategoryAsync(recipe.id_category);

                ViewBag.IngredientsDropDown = ingredientsOperations.GetIngredientsDropDown(await IngredientsDO.GetIngredientsAsync());

                //Used for displaying amount
                ViewBag.Ingredients = ingredientsList;

                //Names of ingredients used in recipe
                ViewBag.IngredientsNames = await ingredientsOperations.GetIngredientsNames(ingredientsList);

                ViewBag.IngredientUnits = await ingredientsOperations.GetUnitNames(ingredientsList);

                //Unit shown next to the textbox for inserting amount
                ViewBag.Unit = unit.Unit;

                ViewBag.Recipe = recipe;
            }
            catch (Exception ex)
            {
                if (ex.StackTrace.Contains("GetRecipeAsync") || id == null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult RemoveRecipeIngredient(int recipeID, int ingredientID) 
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

        /**
         * Method for deleting recipe depending on id
        **/
        public ActionResult Remove(int id)
        {
            try
            {
                using (Entities context = new Entities())
                {
                    //find recipe
                    var recipe = context.Recipes.Single(x => x.id_recipe == id);
                    //remove recipe
                    context.Recipes.Remove(recipe);
                    //save changes in database
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            
            }


            return RedirectToAction("Index", "Recipes");
        }

        /**
         * Method for editing recipe name and instructions 
        **/
        public ActionResult Edit(RecipeDetailModel model)
        {
            try
            {
                using (Entities context = new Entities())
                {
                    //find recipe
                    var recipe = context.Recipes.Single(x => x.id_recipe == model.RecipeModel.Id_recipe);
                    //store new value
                    recipe.name_recipe = model.RecipeModel.Name_recipe;
                    recipe.instructions = model.RecipeModel.Instructions;
                    //save changes in db
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Pro metodu, která není statická, je požadován cíl."))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Recipe","Recipes", new { id = model.RecipeModel.Id_recipe});
        }
	}
}