using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(0.1, double.MaxValue , ErrorMessage ="Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than One item")]
        public int Quantity { get; set; }
        [Required]
        public string category { get; set; }
        [Required]
        public string Brand { get; set; }
    }
}