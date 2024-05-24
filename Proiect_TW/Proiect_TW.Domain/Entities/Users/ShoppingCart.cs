using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]//Aceasta inseamna ca campul Title nu poate fi Empty
        [Display(Name = "UserEmail")]
        [StringLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
        public string UserEmail { get; set; }

        [Required]
        [Display(Name = "ProductTitle")]
        [StringLength(150, ErrorMessage = "Description cannot be longer than 150 characters.")]
        public string ProductTitle { get; set; }
        
        [Required]
        [Display(Name = "Size")]
        [StringLength(10, ErrorMessage = "Description cannot be longer than 10 characters.")]
        public string Size { get; set; }        
        
        [Required]
        [Display(Name = "Count")]
        public int Count { get; set; }        


        [DataType(DataType.Date)]
        public DateTime PublishTime { get; set; }

        [Required]
        [Display(Name = "Ip")]
        [StringLength(50, ErrorMessage = "Description cannot be longer than 50 characters.")]
        public string Ip { get; set; }

    }
}
