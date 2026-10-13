using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace homework.ViewModel
{
    public class UpdateProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public float Price { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public IFormFile? ImageFile { get; set; }
        public string? CurrentImageUrl { get; set; }
    }
}
