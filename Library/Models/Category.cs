using System;
using System.Collections.Generic;

namespace Library.Models
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
}
