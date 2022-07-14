#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//Change this based on new project namespace
namespace SpotifyPlus.Models;
public class LoginUser
{
    // No other fields!
    [Required]
    public string LoginUsername { get; set; }
    //Change to LoginEmail
    [Required]
    [DataType(DataType.Password)]
    public string LoginPassword { get; set; } 
    //Change to LoginPassword
}