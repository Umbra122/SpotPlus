#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotifyPlus.Models;

public class PrivateTag
{
    [Key]
    public int PrivateTagId { get; set; }
    [Required]
    public string TagName { get; set; }
    [Required]
    public string TagDescription { get; set; }
    [Required]
    public int UserId { get; set; }
}