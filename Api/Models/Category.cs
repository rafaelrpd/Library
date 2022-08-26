using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }

        public int CategoryId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }

    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
    }

    public class CategoryPostDTO
    {
        public string? Name { get; set; }
    }

    public class CategoryDetailsDTO
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public ICollection<BookDTO>? Books { get; set; }
    }
}
