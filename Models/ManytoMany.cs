#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SpotifyPlus.Models;
namespace SpotifyPlus.Models;

public class UserToSongs
{
    [Key]
    public int UserToSongsId { get; set; }
    public int UserId { get; set; }
    public int SongId { get; set; }
    public User? User { get; set; }
    public Song? Song { get; set; }
}

public class SongsToTags
{
    [Key]
    public int SongsToTagsId { get; set; }
    public int SongId { get; set; }
    public int TagId { get; set; }
    public Song? Song { get; set; }
    public Tag? Tag { get; set; }
}