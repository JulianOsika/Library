using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [Length(1, 100, ErrorMessage = "Tytuł może mieć od 1 do 100 znaków")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Autor jest wymagany")]
        [Length(1, 50)]
        public string Author { get; set; }

        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        public int LoanId { get; set; }
        public Loan? Loan { get; set; }
    }
}
