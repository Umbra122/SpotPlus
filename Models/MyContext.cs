#pragma warning disable CS8618
/* 
Disabled Warning: "Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable."
We can disable this safely because we know the framework will assign non-null values when it constructs this class for us.
*/
using Microsoft.EntityFrameworkCore;
// Change namespace based on project file name
namespace SpotifyPlus.Models;

// the MyContext class representing a session with our MySQL database, allowing us to query for or save data
public class MyContext : DbContext 
{ 
    public MyContext(DbContextOptions options) : base(options) { }
    // the "User" table name will come from the DbSet property name
    public DbSet<User> User { get; set; } 
    public DbSet<Song> Song { get; set; }
    public DbSet<Tag> Tag { get; set; }
    public DbSet<PrivateTag> PrivateTag { get; set; }
    public DbSet<UserToSongs> UserSongs { get; set; }
    public DbSet<SongsToTags> SongTags { get; set; }

    // Change this based on class model you wish to use for database
}