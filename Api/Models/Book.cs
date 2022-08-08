using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class Book
    {
        public Book()
        {
            BorrowedBooks = new HashSet<BorrowedBook>();
        }

        public string Isbn { get; set; } = null!;
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string? Title { get; set; }
        public int? Quantity { get; set; }

        public virtual Author Author { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}
