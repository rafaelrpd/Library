namespace Library.Entities.Models
{
    public class UserData
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


        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Todo: Guardar o hash?
        public DateTime DateOfBirth { get; set; }
        public string IdentificationNumber { get; set; }
        public Guid IdentificationType { get; set; }
        public Guid Gender { get; set; }
        public Guid Nationality { get; set; }
        public Guid Occupation { get; set; }
        public Guid EducationLevel { get; set; }


        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
