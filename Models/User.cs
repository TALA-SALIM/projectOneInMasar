using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using homework.Enums;

namespace homework.Models
{
    public class User : IdentityUser
    {
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserType UserType { get; set; }
    }
}