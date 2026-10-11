using System.ComponentModel.DataAnnotations;

namespace homework.Models
{
    public class Product : BaseEntity
    {
        
public int Id { get; set; }
[Required]
public string Name { get; set; }
[Required]
public string Description { get; set; }
public float Price { get; set; }

public int CategoryId { get; set; }

public Category Category { get; set; }
[Required]
public string ImageUrl { get; set; }


    }
}