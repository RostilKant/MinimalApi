using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Todo> Todos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new DbTodoSeeder());
    }

}

public class DbTodoSeeder : IEntityTypeConfiguration<Todo>
{       
     public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.HasData(
                new ()
                {
                    Id = 1,
                    Title = "1",
                    Description = "Desc1",
                    IsCompleted = false
                },
                new ()
                {
                    Id = 2,
                    Title = "2",
                    Description = "Desc2",
                    IsCompleted = false
                },
                new ()
                {
                    Id = 3,
                    Title = "3",
                    Description = "Desc3",
                    IsCompleted = false
                }
            );
        }
}