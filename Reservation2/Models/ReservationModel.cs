using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservation2.Models
{
    public class ReservationModel : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane.")]
        [StringLength(20)]
        [Display(Name = "Imie")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        [StringLength(50)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; } = string.Empty;


        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Nieprawidłowy numer telefonu.")]
        [StringLength(15)]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;


        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Brak okresu pobytu")]
        [Display(Name = "Początek rezerwacji")]
        [DataType(DataType.Date)]
        public DateTime StartOfReservation { get; set; } = DateTime.Now;


        [Display(Name = "Koniec rezerwacji")]
        [DataType(DataType.Date)]
        public DateTime EndOfReservation { get; set; } = DateTime.Now;


        [Display(Name = "Zaliczka")]
        public int? Prepayment { get; set; } = 0;


        [Display(Name = "Zapłacono")]
        public int? Payment { get; set; } = 0;

        public bool IsPaid { get; set; } = false;


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate = DateTime.Now;


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpdateDate = DateTime.Now;
       

        [Display(Name = "Dodatkowe informacje")]
        public string? AdditionalInfo { get; set; }


        [Display(Name = "Notatka")]
        public string? Notes { get; set; }

       

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) 
        {
            var property = new[] { "EndOfReservation" };
            if (EndOfReservation < StartOfReservation)
            {
                yield return new ValidationResult("Koniec musi być przed startem.", property);
            }

        }

    }
    
}
