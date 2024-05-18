using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    [Table("ProductImages")]
    public class ProductImages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]//Aceasta inseamna ca campul Title nu poate fi Empty
        [Display(Name = "ProductTitle")]
        [StringLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
        public string ProductTitle { get; set; }

        [Required]
        [Display(Name = "ImageName")]
        [StringLength(500, ErrorMessage = "Description cannot be shorter than 500 characters.")]
        public string ImageName { get; set; }

        [Required]
        [Display(Name = "ImagePath")]
        [StringLength(1000)]
        public string ImagePath { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishTime { get; set; }

    }
}
