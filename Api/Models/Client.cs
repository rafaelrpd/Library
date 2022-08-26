using System;
using System.Collections.Generic;

namespace Api.Models
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

    public class ClientDTO
    {
        public string Cpf { get; set; } = null!;
        public string? Name { get; set; }
    }

    public class ClientDetailsDTO
    {
        public string Cpf { get; set; } = null!;
        public string? Name { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public ICollection<BorrowedBookDTO>? BorrowedBooks { get; set; }
    }
}
