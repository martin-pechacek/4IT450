using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Semestralka.Models
{
    public class AddUserModel
    {       
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string firstname { get; set; }

        [Required]
        public string lastname { get; set; }

        [Required]
        public bool user_right { get; set; }
    }
}