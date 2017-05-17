using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semestralka.Models
{
    public class RecipeDetailModel
    {
        public RecipeModel RecipeModel { get; set; }
        public IngredientModel IngredientModel { get; set; }
    }
}