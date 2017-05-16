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

        public static async Task<List<RecipeDO>> GetRecipeAsync(int id)
        {
            using (Entities context =
                new Entities())
            {
                return await context.Recipes
                  .Where(x => x.id_recipe == id)
                  .Select(x => new RecipeDO()
                    {
                        RecipeName = x.name_recipe,
                        CategoryID = x.id_category,
                        Instructions = x.instructions
                    }).ToListAsync();
            }
        }
    }
}