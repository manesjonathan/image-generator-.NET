using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageGeneratorApi.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string GoogleId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public bool IsAllowed { get; set; }
    public int Quota { get; set; }

    public User(string googleId, string email, string password, string name, bool isAllowed, int quota)
    {
        this.GoogleId = googleId;
        this.Email = email;
        this.Password = password;
        this.Name = name;
        this.IsAllowed = isAllowed;
        this.Quota = quota;
    }
}