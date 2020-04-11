using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Refractory
{
    public class RefractoryCalculationModel
    {
        public float a1 { get; set; }
        public float a2 { get; set; }
        public float b1 { get; set; }
        public float b2 { get; set; }        
        public float BrickLength { get; set; }
        public float TopDiameter { get; set; }
        public float BottomDiameter { get; set; }
        public float RowNumber { get; set; }
        public float Density { get; set; }
        public float Price { get; set; }
    }
}