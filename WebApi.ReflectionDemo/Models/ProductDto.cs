using System.ComponentModel.DataAnnotations;

namespace WebApi.ReflectionDemo.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(1, 10000, ErrorMessage = "Fiyat 1 ile 10000 arasında olmalıdır")]
        public decimal Price { get; set; }
    }
}
