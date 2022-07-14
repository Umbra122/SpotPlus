#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SpotifyPlus.Models;
namespace SpotifyPlus.Models;
    
public class User
{
    [Key]
    public int UserId { get; set; } 
    [Required]
    [MinLength(3, ErrorMessage="Username must be between 3 and 15 characters!")]
    [MaxLength(15, ErrorMessage="Username must be between 3 and 15 characters!")]
    public string Username { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    [Required]
    [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
    public string Password { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    // Will not be mapped to your users table!
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Confirm { get; set; } 
} 