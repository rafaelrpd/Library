namespace Library.Entities.Models
{
    public class BoletoPaymentDetails : PaymentDetails
    {
        public string UserName { get; set; }
        public string IdentificationNumber { get; set; }
        public Guid IdentificationType { get; set; }
        public string UserEmail { get; set; }
        public string Description { get; set; }
        public decimal UnitValue { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public DateTime DueDate { get; set; }
        public int Installments { get; set; }
    }
}
