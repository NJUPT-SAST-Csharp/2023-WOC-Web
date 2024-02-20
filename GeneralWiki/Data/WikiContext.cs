using GeneralWiki.Data.DataBaseConfiguration;
using GeneralWiki.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GeneralWiki.Data;

public class WikiContext : IdentityDbContext
{
    public WikiContext(DbContextOptions<WikiContext> options) : base(options)
    {
    }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);// 确保首先调用基类的OnModelCreating
        modelBuilder.ApplyConfiguration(new EntryConfig());
    }
}