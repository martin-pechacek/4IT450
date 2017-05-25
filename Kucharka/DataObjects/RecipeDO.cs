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
        public short RecipeID { get; set; }
        public string RecipeName { get; set; }
        public int CategoryID { get; set; }
        public string Instructions { get; set; }

        /**
         *  Return recipe object depeding on id
        **/
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
        /**
         *  Return list of all recipes in database
        **/
        public static async Task<List<RecipeDO>> GetRecipesAsync() 
        {
            using(Entities context = new Entities())
            {
                return await context.Recipes
                    .Select(x => new RecipeDO()
                    {
                        RecipeID = x.id_recipe,                        
                        RecipeName = x.name_recipe
                    }).ToListAsync();
            }
        }
    }
}