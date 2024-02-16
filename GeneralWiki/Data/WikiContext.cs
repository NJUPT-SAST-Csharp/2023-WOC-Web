using GeneralWiki.Data.DataBaseConfiguration;
using GeneralWiki.Models;
using Microsoft.EntityFrameworkCore;

namespace GeneralWiki.Data;

public class WikiContext(DbContextOptions<WikiContext> options) : DbContext(options)
{
    public DbSet<Entry> Entries { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Picture> Pictures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EntryConfig());
        base.OnModelCreating(modelBuilder);
    }
}