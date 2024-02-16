using System.ComponentModel.DataAnnotations;

namespace GeneralWiki.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [MaxLength(255)]
    public string CategoryName { get; set; }

    public string? Description { get; set; }
    
    // public int? ParentCategoryId { get; set; }
    // public Category ParentCategory { get; set; }

    public List<Entry> Entries { get; set; } = [];
}