using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    [Table("OrderProducts")]
    public class OrderProducts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]//Aceasta inseamna ca campul Title nu poate fi Empty
        [Display(Name = "Email")]
        public int OrderId { get; set; }

        [Required]//Aceasta inseamna ca campul Title nu poate fi Empty
        [Display(Name = "Email")]
        [StringLength(150, ErrorMessage = "Email cannot be longer than 150 characters.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "ItemPrice")]
        public int ItemPrice { get; set; }
        
        [Required]
        [Display(Name = "Count")]
        public int Count { get; set; }


        [Required]
        [Display(Name = "MainImagePath")]
        [StringLength(150)]
        public string MainImagePath { get; set; }
    }
}
