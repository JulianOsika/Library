﻿using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public int LoanId { get; set; }
        public Loan? Loan { get; set; }
    }
}
