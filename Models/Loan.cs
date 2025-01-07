using Library.Areas.Identity.Data;

namespace Library.Models
{
    public class Loan
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public Loan()
        {

        }
    }
}
