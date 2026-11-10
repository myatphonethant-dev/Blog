using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApi.AppDbContext;

public partial class EfAppDbContext : DbContext
{
    public EfAppDbContext()
    {
    }

    public EfAppDbContext(DbContextOptions<EfAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
