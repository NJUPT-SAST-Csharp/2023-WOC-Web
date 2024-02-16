using System.ComponentModel.DataAnnotations;

namespace GeneralWiki.Models;

public class Picture
{
    [Key]
    public int PictureId { get; set; }
    public string PictureUrl { get; set; }
}