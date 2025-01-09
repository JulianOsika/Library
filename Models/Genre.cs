using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa gatunku jest obowiązkowa")]
        [Length(1, 50)]
        public string Name { get; set; }
    }
}
