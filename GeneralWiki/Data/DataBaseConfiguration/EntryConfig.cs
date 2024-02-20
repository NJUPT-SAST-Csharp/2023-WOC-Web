using GeneralWiki.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralWiki.Data.DataBaseConfiguration;

public class EntryConfig:IEntityTypeConfiguration<Entry>
{
    public void Configure(EntityTypeBuilder<Entry> builder)
    {
        builder
            .HasMany(et => et.Tags) 
            .WithMany(e => e.Entries) 
            .UsingEntity(
                "EntryTag",
                l => l.HasOne(typeof(Tag)).WithMany().HasForeignKey("TagsId").HasPrincipalKey(nameof(Tag.TagId)),
                r => r.HasOne(typeof(Entry)).WithMany().HasForeignKey("EntryId").HasPrincipalKey(nameof(Entry.EntryId)),
                j => j.HasKey("EntryId", "TagsId"));
        builder
            .HasOne(et => et.Category)
            .WithMany(c => c.Entries)
            .HasForeignKey(e => e.CategoryId);
    }
}
