using System.ComponentModel.DataAnnotations;

namespace ProspectLabApp.Pages.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string? Brand { get; set; }
        public string? Title { get; set; }
        public string? Quantity { get; set; }
        public string? Discount { get; set; }
        public decimal Price { get; set; }
        public string LeafletName { get; set; }

        public Product()
        {
            Id = Guid.NewGuid();
        }
    }
}
