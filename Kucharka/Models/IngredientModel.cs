﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Semestralka.Models
{
    public class IngredientModel
    {
        [Required]
        [Display(Name = "Ingredience")]
        public string Name_ingredient { get; set; }

        [Required]
        [Display(Name = "Jednotka")]
        public string Unit { get; set; }

        [Required]
        public string Id_ingredient { get; set; }
    }
}