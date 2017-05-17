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
        public string name_recipe { get; set; }

        [Required]
        [Display(Name = "Kategorie")]
        public string id_category { get; set; }

        [Required]
        [Display(Name = "Postup")]
        public string instructions { get; set; }

        [Required]
        public int id_recipe { get; set; }
    }
}