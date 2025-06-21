namespace MarketSpot.Application.DTOs
{
    public class PaymentRequest
    {
        public Guid ProductId { get; set; }

        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string CVC { get; set; }
        public string FullName { get; set; }
    }
}