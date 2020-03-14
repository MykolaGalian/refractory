using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("Refractory")]
    public class Refractory : MyEntity
    {
        [MaxLength(10000)]   
        public string RefractoryDescription { get; set; }  //body
        [MaxLength(1000)]
        public string RefractoryBrand { get; set; }  //title
        public string RefractoryPicture { get; set; }
        [Required]
        public DateTime? DateCreate { get; set; }
        public DateTime? LastEdit { get; set; }
        public string RefractoryType { get; set; }  //tag
        public bool IsBlocked { get; set; }

        public float Density { get; set; }
        public float MaxWorkTemperature { get; set; }
        
        public float Lime { get; set; }
        public float Alumina { get; set; }
        public float Silica { get; set; }
        public float Magnesia { get; set; }
        public float Carbon { get; set; }
        public float Price { get; set; }


        [ForeignKey("UserInfoId")]
        public virtual UserInfo UserInfo { get; set; }
        public int UserInfoId { get; set; }
        

        public virtual ICollection<Comment> Comments { get; set; }
        
       
    }
}
