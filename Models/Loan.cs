using Library.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public Loan()
        {

        }
    }
}
