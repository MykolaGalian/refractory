using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models.Post
{
    public class RefractoryEditViewModel
    {
        public string RefractoryDescription { get; set; }

        public string RefractoryBrand { get; set; }

        [Display(Name = "RefractoryType:")]
        [MaxLength(50)]
        public string RefractoryType { get; set; }

        public float Density { get; set; }
        public float MaxWorkTemperature { get; set; }

        public float Lime { get; set; }
        public float Alumina { get; set; }
        public float Silica { get; set; }
        public float Magnesia { get; set; }
        public float Carbon { get; set; }
        public float Price { get; set; }

    }
}