using Semestralka.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Semestralka.DataObjects
{
    public class IngredientsDO
    {
        public int IngredientID { get; set; }
        public string IngredientName { get; set; }
        public string Unit { get; set; }

        /**
         *  Return list of all ingredients saved in database
        **/
        public static async Task<List<IngredientsDO>> GetIngredientsAsync()
        {
            using (Entities context = new Entities())
            {
                return await context.Ingredients
                  .Select(x => new IngredientsDO()
                    {
                        IngredientID = x.id_ingredient,
                        IngredientName = x.name_ingredient,
                    }).ToListAsync();
            }
        }

        /**
        *  Return unit of ingredient
        **/
        public static async Task<IngredientsDO> GetUnitAsync(int id)
        {
            await Task.Delay(0);

            IngredientsDO ingredientDO = new IngredientsDO();

            using (Entities context = new Entities())
            {
                Ingredient ingredient = context.Ingredients.Single(x => x.id_ingredient == id);
                ingredientDO.Unit = ingredient.unit;
            }

            return ingredientDO;
        }

        /**
        *  Return ingredient name
        **/
        public static async Task<string> GetNameAsync(int id)
        {
            await Task.Delay(0);

            string ingredientName = "";

            using (Entities context = new Entities())
            {
                Ingredient ingredient = context.Ingredients.Single(x => x.id_ingredient == id);
                ingredientName = ingredient.name_ingredient;
            }

            return ingredientName;
        }

        /**
         *  Return unit of first ingredient in database. Used when recipe page is loading
        **/
        public static async Task<IngredientsDO> GetFirstUnitAsync()
        {
            await Task.Delay(0);

            IngredientsDO ingredientDO = new IngredientsDO();

            using (Entities context = new Entities())
            {
                Ingredient ingredient = context.Ingredients.First();
                ingredientDO.Unit = ingredient.unit;
            }

            return ingredientDO;
        }
    }
}