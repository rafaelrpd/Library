using System;
using System.Collections.Generic;

namespace Library.Models
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int AuthorId { get; set; }
        public string? Name { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
