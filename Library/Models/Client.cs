using System;
using System.Collections.Generic;

namespace Library.Models
{
    public partial class Client
    {
        public Client()
        {
            BorrowedBooks = new HashSet<BorrowedBook>();
        }

        public string Cpf { get; set; } = null!;
        public string? Name { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}
