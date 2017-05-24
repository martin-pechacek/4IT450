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
        public async Task<ActionResult> Index(bool? error)
        {
            if(error == true)
            {
                ModelState.AddModelError(string.Empty, "Ingredience už existuje");
            }
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

                    if (ingredient.id_ingredient == -1)
                    {
                        return RedirectToAction("Index", "Ingredients", new { error = true });
                    }
                }   
            }
            catch(Exception ex) 
            {
                if(ingredient.name_ingredient == null)
                {
                    ModelState.AddModelError("IngredientName", "Název ingredience musí být vyplněný");
                }
                if(ingredient.unit == null)
                {
                    ModelState.AddModelError("Unit", "Jednotka musí být vyplněná");
                }
            }

            List<IngredientsDO> ingredients = await IngredientsDO.GetIngredientsAsync();

            ViewBag.Ingredients = ingredients;

            return View("Index");
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

                        if(ingredient.id_ingredient == -1)
                        {
                            return RedirectToAction("Index", "Ingredients", new { error = true });
                        }
                    }

                    return RedirectToAction("Index", "Ingredients");
                }

                return View();
            }

            catch (Exception ex)
            {
                if (model.Name_ingredient == null)
                {
                    ModelState.AddModelError("IngredientName", "Název ingredience musí být vyplněné");
                }
                if (model.Unit == null)
                {
                    ModelState.AddModelError("UnitError", "Jednotka musí být vyplněná");
                }
                if(ex.Message.Equals("Posloupnost neobsahuje žádné prvky."))
                {
                    return RedirectToAction("Index", "Ingredients");
                }

                return View();
            }
        }
	}
}