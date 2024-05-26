using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.Domain.Entities.Users
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]//Aceasta inseamna ca campul Title nu poate fi Empty
        [Display(Name = "Email")]
        [StringLength(150, ErrorMessage = "Email cannot be longer than 150 characters.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "NameSurname")]
        [StringLength(150, ErrorMessage = "Name and Surname cannot be longer than 150 characters.")]
        public string NameSurname { get; set; }

        [Required]
        [Display(Name = "PhoneNumber")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "City")]
        [StringLength(20)]
        public string City { get; set; }

        [Required]
        [Display(Name = "Street")]
        [StringLength(150)]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Building")]
        [StringLength(20)]
        public string Building { get; set; }

        [Required]
        [Display(Name = "Appartment")]
        public int Appartment { get; set; }

        [Required]
        [Display(Name = "PostalCode")]
        [StringLength(20)]
        public string PostalCode { get; set; }
        
        [Required]
        [Display(Name = "PaymentMethod")]
        [StringLength(20)]
        public string PaymentMethod { get; set; }

        [Required]
        [Display(Name = "TotalPrice")]
        public int TotalPrice { get; set; }


        [DataType(DataType.Date)]
        public DateTime PublishTime { get; set; }

        [StringLength(30)]
        public string Ip { get; set; }
    }
}
