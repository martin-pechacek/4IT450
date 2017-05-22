using Semestralka.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;

namespace Semestralka.DataObjects
{
    public class CategoryDO
    {
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }

        public static async Task<List<CategoryDO>> GetCategoriesAsync()
        {
            using (Entities context =
                new Entities())
            {
                return await context.Categories
                    .Select(x => new CategoryDO()
                    {
                        CategoryID = x.id_category,
                        CategoryName = x.name_category
                    }).ToListAsync();
            }
        }

        public static async Task<Category> GetCategoryAsync(int id)
        {
            using (Entities context =
                new Entities())
            {
                return await context.Categories
                    .SingleAsync(x => x.id_category == id);
            }
        }
    }
}