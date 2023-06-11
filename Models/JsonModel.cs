using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace JsonApi.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Define DbSet properties for your models/entities
    public DbSet<JsonModel> JsonModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JsonModel>()
            .Property(b => b.BsonData)
            .HasColumnType("jsonb");
        
        modelBuilder.Entity<JsonModel>()
            .HasKey(b => b.Id);
    }
}

public class UrlModel
{
    public string Url { get; set; }
}

public class JsonModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string BsonData { get; set; }
}
