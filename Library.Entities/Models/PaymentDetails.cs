namespace Library.Entities.Models
{
    public class PaymentDetails
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
    }
}
