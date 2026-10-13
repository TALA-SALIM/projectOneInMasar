using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace homework.Data;

<<<<<<< HEAD
public class ApplicationDbContext : IdentityDbContext<Models.User>
=======
public class ApplicationDbContext : IdentityDbContext
>>>>>>> 3ed1585bec5e2fdd0ff6b542e38bd61b94aedac6
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Models.Product> Products { get; set; }

    public DbSet<Models.Category> Categories { get; set; }
}
