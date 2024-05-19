using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proiect_TW.Domain.Enums;

namespace Proiect_TW.Domain.Entities.User
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]//Aceasta inseamna ca campul Title nu poate fi Empty
        [Display(Name = "Title")]
        [StringLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Type")]
        [StringLength(20)]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Style")]
        [StringLength(20)]
        public string Style { get; set; }

        [Required]
        [Display(Name = "XS")]
        public bool XS { get; set; }

        [Required]
        [Display(Name = "S")]
        public bool S { get; set; }

        [Required]
        [Display(Name = "M")]
        public bool M { get; set; }

        [Required]
        [Display(Name = "L")]
        public bool L { get; set; }

        [Required]
        [Display(Name = "XL")]
        public bool XL { get; set; }

        [Required]
        [Display(Name = "XXL")]
        public bool XXL { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "AgeCategory")]
        public string AgeCategory { get; set; }

        [Required]
        [Display(Name = "Price")]
        public string Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishTime { get; set; }

        [StringLength(30)]
        public string Ip { get; set; }
    }
}
