#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotifyPlus.Models;

public class Song
{
    [Key]
    public int SongId { get; set; }
    [Required]
    public string SongName { get; set; }
    [Required]
    public string SongArtist { get; set; }
    [Required]
    public string SongAlbum { get; set; }
    [Required]
    public string Id { get; set; }
    //SongId is for the database, Id is spotify's Id markers
}