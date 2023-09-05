using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public sealed class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DataBaseContext() {}
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageSource> MessageSources { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(builder =>
        {
            builder.HasOne(x => x.MessageSource).WithMany(x => x.Messages);
        });
        modelBuilder.Entity<MessageSource>(builder =>
        {
            builder.HasMany(x => x.Messages).WithOne(x => x.MessageSource);
            builder.HasMany(x => x.Workers).WithMany(x => x.MessageSources);
        });
        modelBuilder.Entity<Account>(builder =>
        {
            builder.HasOne(x => x.Employee);
        });
        modelBuilder.Entity<Report>(builder =>
        {
            builder.HasOne(x => x.Supervisor);
        });
        modelBuilder.Entity<Employee>().HasDiscriminator<string>("Employee")
            .HasValue<Worker>("worker")
            .HasValue<Supervisor>("supervisor");
    }
}