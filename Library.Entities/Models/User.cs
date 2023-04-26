namespace Library.Entities.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public Guid DeletedBy { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Billing> Billings { get; set; }
        public virtual ICollection<TelephoneUser> TelephoneUsers { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual UserData UserData { get; set; }
    }
}
