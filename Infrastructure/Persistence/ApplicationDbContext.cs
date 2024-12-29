using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Movie>? movies { get; set; }
    public DbSet<Genre> genres { get; set; }
    public DbSet<Customer>? customers { get; set; }
}