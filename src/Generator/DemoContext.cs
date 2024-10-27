using Microsoft.EntityFrameworkCore;

public class DemoContext: DbContext
{
    DbSet<People> Peoples { get; set; }
    private string _connectionString;

    public DemoContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(_connectionString);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<People>().HasKey(x => x.PersonId);
        //modelBuilder.Entity<People>().;
        modelBuilder.Entity<People>().ToTable("People");
    }
}