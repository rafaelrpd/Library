using System;
using System.Collections.Generic;

namespace Api.Models
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
    public class AuthorDTO
    {
        public int AuthorId { get; set; }
        public string? Name { get; set; }
    }

    public class AuthorPostDTO
    {
        public string? Name { get; set; }
    }

    public class AuthorDetailsDTO
    {
        public int AuthorId { get; set; }
        public string? Name { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public ICollection<BookDTO>? Books { get; set; }
    }
}
