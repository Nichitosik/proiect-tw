using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proiect_TW.Domain.Enums;

namespace Proiect_TW.Domain.Entities.User.DBModel
{
    internal class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]//Aceasta inseamna ca campul Title nu poate fi Empty
        [Display(Name = "Title")]
        [StringLength(150,  ErrorMessage = "Title cannot be longer than 150 characters.")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(500, MinimumLength = 100, ErrorMessage = "Description cannot be shorter than 100 characters.")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Type")]
        [StringLength(20)]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Style")]
        [StringLength(20)]
        public int Style { get; set; }

        [DataType(DataType.Date)]

        public DateTime PublishTime { get; set; }

        [StringLength(30)]
        public string Ip { get; set; }

    }
}
