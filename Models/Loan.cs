using Library.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Wybór książki jest wymagany.")]
        public int BookId { get; set; }

        [ValidateNever]
        public Book Book { get; set; }

        [Required(ErrorMessage = "Wybór użytkownika jest wymagany.")]
        public string UserId { get; set; }

        [ValidateNever]
        public User User { get; set; }

        [Required(ErrorMessage = "Data wypożyczenia jest wymagana")]
        [DataType(DataType.Date, ErrorMessage ="Zły format daty")]
        [CustomValidation(typeof(Loan), nameof(ValidateLoanDate))]
        public DateTime LoanDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Zły format daty")]
        [CustomValidation(typeof(Loan), nameof(ValidateReturnDate))]
        public DateTime? ReturnDate { get; set; }

        public Loan()
        {

        }

        public static ValidationResult? ValidateLoanDate(DateTime loanDate, ValidationContext context)
        {
            if (loanDate > DateTime.Now)
                return new ValidationResult("Data wypożyczenia nie może być w przyszłości");
            else return ValidationResult.Success;
        }

        public static ValidationResult? ValidateReturnDate(DateTime? returnDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as Loan;
            if (returnDate < instance.LoanDate)
                return new ValidationResult("Data zwrotu nie może być wcześniejsza niż wypożyczenia");
            else return ValidationResult.Success;
        }
    }
}
