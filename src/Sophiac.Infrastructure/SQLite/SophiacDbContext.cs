namespace Sophiac.Infrastructure.SQLite;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Sophiac.Domain.Answers;
using Sophiac.Domain.Questions;
using Sophiac.Domain.Settings;
using Sophiac.Domain.TestRuns;
using Sophiac.Domain.TestSets;

public class SophiacDbContext : DbContext
{
    public SophiacDbContext(DbContextOptions<SophiacDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestSet> TestSets { get; set; }
    // public DbSet<TestRun> TestRuns { get; set; }
    public DbSet<SophiacSettings> SophiacSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=sophiac.db");
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        modelBuilder.Entity<TestSet>(entity =>
        {
            entity.Property(e => e.SingleChoiceQuestions)
                .HasColumnType("json")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<IList<SingleChoiceQuestion>>(v, jsonOptions));
        });
        modelBuilder.Entity<TestSet>(entity =>
        {
            entity.Property(e => e.MultipleChoiceQuestions)
                .HasColumnType("json")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<IList<MultipleChoicesQuestion>>(v, jsonOptions));
        });
        modelBuilder.Entity<TestSet>(entity =>
        {
            entity.Property(e => e.MappingQuestions)
                .HasColumnType("json")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<IList<MappingQuestion>>(v, jsonOptions));
        });
        
        base.OnModelCreating(modelBuilder);
    }
}

