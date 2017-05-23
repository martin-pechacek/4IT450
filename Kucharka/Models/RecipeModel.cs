using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Semestralka.Models
{
    public class RecipeModel
    {
        [Required]
        [Display(Name = "Recept")]
        public string Name_recipe { get; set; }

        [Required]
        [Display(Name = "Kategorie")]
        public int Id_category { get; set; }

        [Required]
        [Display(Name = "Postup")]
        public string Instructions { get; set; }

        [Required]
        public int Id_recipe { get; set; }
    }
}