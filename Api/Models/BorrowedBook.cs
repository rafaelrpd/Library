﻿using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class BorrowedBook
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = null!;
        public string BookId { get; set; } = null!;
        public DateTime? BorrowedDate { get; set; }
        public DateTime? LimitDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Client Client { get; set; } = null!;
    }

    public class BorrowedBookDTO
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = null!;
        public string BookId { get; set; } = null!;
        public DateTime? BorrowedDate { get; set; }
        public DateTime? LimitDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
    }

    public class BorrowedBookLendDTO
    {
        public string ClientId { get; set; } = null!;
        public string BookId { get; set; } = null!;
    }
    public class BorrowedBookReturnDTO
    {
        public int Id { get; set; }
    }

    public class BorrowedBookDetailsDTO
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = null!;
        public string BookId { get; set; } = null!;
        public DateTime? BorrowedDate { get; set; }
        public DateTime? LimitDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public BookDTO Book { get; set; } = null!;
        public ClientDTO Client { get; set; } = null!;
    }
}
