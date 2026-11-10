using BlogWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApi.AppDbContext;

public class EfAppDbContext : DbContext
{
    public EfAppDbContext(DbContextOptions<EfAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs => Set<Blog>();
}