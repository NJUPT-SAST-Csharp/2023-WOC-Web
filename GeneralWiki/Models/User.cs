using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GeneralWiki.Models;

//�û��Ľ�ɫȨ��
public enum Role
{
    tourist = 1,
    consumer,
    author,
    adminstrator,
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
    public string? Email { get; set; }
    public Role Role { get; set; }
}