using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotifyPlus.Models;

public class Tag
{
    [Key]
    public int TagId { get; set; }
    [Required]
    [MinLength(2, ErrorMessage="Tag name must be between 2 and 9 characters!")]
    [MaxLength(9, ErrorMessage="Tag name must be between 2 and 9 characters!")]
    public string TagName { get; set; }
    [Required]
    public string TagDescription { get; set; }
    [NotMapped]
    public int PrivateCheck { get; set; }
}