using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    { }

    protected AppDbContext()
    {}

    public DbSet<Todo> Todos { get; set; } = null!;
}