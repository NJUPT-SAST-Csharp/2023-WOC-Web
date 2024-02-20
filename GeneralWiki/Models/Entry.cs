using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GeneralWiki.Models;

//wiki中展示的词条

public class Entry
{
    [Key] 
    public int? EntryId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string? ImgUrl { get; set; }
    
    public int? CategoryId { get; set; }

    public Category? Category { get; set; }
    [JsonIgnore]
    public ICollection<Tag> Tags { get; set; } = [];

    public IEnumerable<string> GetTagNames()
    {
        return Tags.Select(t => t.TagName);
    }
}