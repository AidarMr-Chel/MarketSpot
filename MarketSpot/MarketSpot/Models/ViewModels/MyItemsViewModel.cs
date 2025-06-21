namespace Web.Models.ViewModels
{
    public class MyItemsModel
    {
        public string AuthorName { get; set; } = "";
        public List<ProductViewModel> Products { get; set; } = new();
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public double Rating { get; set; }
        public int RatingCount { get; set; }
    }

}
