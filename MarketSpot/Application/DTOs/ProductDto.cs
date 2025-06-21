using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketSpot.Application.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Category { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string AuthorId { get; set; }
        public string ProductFilePath { get; set; }
        public string ImageFilePath { get; set; }

    }
}
