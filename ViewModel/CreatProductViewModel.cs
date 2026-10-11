using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace homework.ViewModel
{
    public class CreateProductViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public float Price { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}