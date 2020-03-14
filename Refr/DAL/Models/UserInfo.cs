using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Identity;

namespace DAL.Models
{
    [Table("UserInfo")]
    public class UserInfo : MyEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
        [ForeignKey("ApplicationUser")]
        public override int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
        
        [Index(IsUnique = true)]
        [MaxLength(30)]
        [MinLength(3)]
        public string Login { get; set; }
        public string UserAvatar { get; set; }
        [MaxLength(100)]
        public string Position { get; set; }  //adr
        public bool IsBlocked { get; set; }
        public DateTime? DateRegistration { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Refractory> Refractories { get; set; }


        public UserInfo()
        {
            Refractories = new List<Refractory>();
        }
    }
}
