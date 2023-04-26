using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public Guid DeletedBy { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Billing> Billings { get; set; }
        public ICollection<Telephone> Telephones { get; set; }
        public ICollection<Book> Books { get; set; }
        public ICollection<Role> Roles { get; set; }
        public UserData UserData { get; set; }
    }
}
