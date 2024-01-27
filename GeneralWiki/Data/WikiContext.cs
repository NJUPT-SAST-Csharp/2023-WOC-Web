using Microsoft.EntityFrameworkCore;
using GeneralWiki.Models;

namespace GeneralWiki.Data;

public class WikiContext:DbContext
{
    public WikiContext(DbContextOptions<WikiContext> options) : base(options)
    {
        
    }
    public DbSet<Entry>? Entries { get; set; }
    public DbSet<User>? Users { get; set; }
}