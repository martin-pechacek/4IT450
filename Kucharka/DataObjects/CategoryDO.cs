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

        public static async Task<List<CategoryDO>> GetCategoriesAsync()
        {
            using (KucharkaEntities context =
                new KucharkaEntities())
            {
                return await context.Categories
                    .Select(x => new CategoryDO()
                    {
                        CategoryName = x.name_category
                    }).ToListAsync();
            }
        }
    }
}