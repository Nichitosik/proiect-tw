using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    [Table("Feedback")]
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]//Aceasta inseamna ca campul Title nu poate fi Empty
        [Display(Name = "Email")]
        [StringLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(1500, ErrorMessage = "Description cannot be longer than 1500 characters.")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishTime { get; set; }

        [StringLength(30)]
        public string Ip { get; set; }
    }
}
