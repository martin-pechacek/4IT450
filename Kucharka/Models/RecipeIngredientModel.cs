using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Semestralka.Models
{
    public class RecipeIngredientModel
    {
        [Required]
        [Display(Name = "Množství")]
        public string Amount { get; set; }

        [Required]
        public int Id_recipe { get; set; }

        [Required]
        public int Id_ingredient { get; set; }
    }
}