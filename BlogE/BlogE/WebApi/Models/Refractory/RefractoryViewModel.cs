using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.Comment;
using WebApi.Models.User;

namespace WebApi.Models.Post
{
    public class RefractoryViewModel
    {
        public int Id { get; set; }
        public string RefractoryDescription { get; set; }

        public string RefractoryBrand { get; set; }
        public string RefractoryPicture { get; set; }
        public int UserInfoId { get; set; }
        public virtual ProfileViewModel UserInfo { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? LastEdit { get; set; }
       
        public ICollection<CommentViewModel> Comments { get; set; }
        
        public string RefractoryType { get; set; }
        public bool IsBlocked { get; set; }

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