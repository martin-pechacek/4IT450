using Semestralka.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace Semestralka.DataObjects
{
    public class RecipeIngredientsDO
    {
        public int Amount { get; set; }
        public int IngredientID { get; set; }
        public int RecipeID { get; set; }

        public static async Task<List<RecipeIngredientsDO>> GetIngredientsAsync(int id)
        {          

            using (Entities context =
                new Entities())
            {
                return await context.Recipe_Ingredient
                  .Where(x => x.id_recipe == id)
                  .Select(x => new RecipeIngredientsDO()
                  {
                      IngredientID = x.id_ingredient,
                      RecipeID = x.id_recipe,
                      Amount = x.amount
                  }).ToListAsync();
            }
        }
    }
}