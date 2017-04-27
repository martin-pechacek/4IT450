using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Semestralka.Models
{
    public class AddCategoryModel
    {
        [Required]
        [Display(Name = "Kategorie")]
        public string name_category { get; set; }
    }
}