namespace MarketSpot.Core.Entities
{
    public class PurchasedProduct
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } 
        public Guid ProductId { get; set; } 
        public DateTime PurchasedAt { get; set; }

        public Product Product { get; set; } 
    }
}