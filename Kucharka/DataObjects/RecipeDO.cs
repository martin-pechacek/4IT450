using Semestralka.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace Semestralka.DataObjects
{
    public class RecipeDO
    {
        public string RecipeName { get; set; }
        public int CategoryID { get; set; }
        public string Instructions { get; set; }

        public static async Task<Recipe> GetRecipeAsync(int id)
        {
            await Task.Delay(0);

            Recipe recipe = new Recipe();

            using (Entities context =
                new Entities())
            {
                recipe = context.Recipes.Single(x => x.id_recipe == id);
            }

            return recipe;
        }
    }
}