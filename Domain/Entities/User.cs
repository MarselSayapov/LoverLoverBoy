using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User : EntityBase
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    public string Email { get; set; }
    
}