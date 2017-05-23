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
    public class IngredientsController : Controller
    {
        //
        // GET: /Ingredients/
        public async Task<ActionResult> Index()
        {
            List<IngredientsDO> ingredients = await IngredientsDO.GetIngredientsAsync();

            ViewBag.Ingredients = ingredients;

            return View();
        }

        /**
         *  Method for inserting new ingredient into database
        **/
        [HttpPost]
        public async Task<ActionResult> Index(Ingredient ingredient)
        {
            await Task.Delay(0);

            try
            {
                using (Entities context = new Entities())
                {
                    context.Ingredients.Add(ingredient);
                    //save changes in database
                    context.SaveChanges();
                }                
            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
            return RedirectToAction("Index", "Ingredients");
        }

        /**
         *  Remove ingredient
        **/
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                using (Entities context = new Entities())
                {
                    //find ingredient
                    var ingredient = context.Ingredients.Single(x => x.id_ingredient == id);
                    //remove ingredient
                    context.Ingredients.Remove(ingredient);
                    //save changes in database
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ingredience nelze smazat. Používá se u některého z receptů");
            }

            List<IngredientsDO> ingredients = await IngredientsDO.GetIngredientsAsync();

            ViewBag.Ingredients = ingredients;

            return View("Index");
        }


        /**
         *  Show edit ingredient page
         **/
        public async Task<ActionResult> Edit(int? id, string edit, IngredientModel model) 
        {
            try
            {
                Ingredient ingredient = await IngredientsDO.GetIngredientAsync(Convert.ToInt16(id));

                ViewBag.Ingredient = ingredient;

                if (edit != null)
                {
                    using (Entities context = new Entities())
                    {
                        //find ingredient
                        var ingredientRecord = context.Ingredients.Single(x => x.id_ingredient == id);

                        ingredientRecord.name_ingredient = model.Name_ingredient;
                        ingredientRecord.unit = model.Unit;
                        context.SaveChanges();
                    }

                    return RedirectToAction("Index", "Ingredients");
                }

                return View();
            }

            catch (Exception ex)
            {
                return RedirectToAction("Index", "Ingredients");
            }
        }
	}
}