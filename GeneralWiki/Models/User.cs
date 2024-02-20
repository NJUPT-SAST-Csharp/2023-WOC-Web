using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeneralWiki.Models;

//用户的角色权限
public enum Role
{
    tourist = 1,
    author,
    adminstrator
}
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(30)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(60)]
    public string Password { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}