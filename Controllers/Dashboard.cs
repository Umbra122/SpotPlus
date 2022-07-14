using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using LoginRegistration.Controllers;
// Change based on project name

using SpotifyPlus.Models;

namespace SpotifyPlus.Controllers;

public class SpotifyController : Controller
{
    private MyContext _context;
    public SpotifyClient spotify = new SpotifyClient(SpotifyClientConfig.CreateDefault("BQCRTm9Cqf5eUzMgKjdfRtGRfIa9nHQtuz5OsWSbKpUR0_1J8IaMiFO7etLez9SYVgqufUZ10XcQL4fSvigbFsYtn-pcPTs1LdQzyAyj-41kpSW9rk7J3YyarPEeNpkYUyYLbKgBrjOd2pg5BzY_jiq-nua1srO83izL2ebTsvbK0UJQmlHhA0znONJzS_4p5yMMU727ohZ0232BPO7nJMU"));

    public SpotifyController(MyContext context)
    {
        _context = context;
        
        
    }
    
    [HttpGet("dashboard")]
    public ViewResult dashboard()
    {
        ViewBag.tags = _context.Tag.ToList();
        // ViewBag.privateTags = _context.PrivateTag.ToList();
        return View("dashboard");
    }
    [HttpGet("tagList/{tagId}")]
    public ViewResult tagList(int tagId)
    {
        // _context.Enthusiast.Include(w => w.User).Where(y => y.HobbyId == hobbyId).ToList();
        ViewBag.songs = _context.SongTags.Include(x => x.Song).Where(y => y.TagId == tagId).ToList();
        ViewBag.tag = _context.Tag.Where(g => g.TagId == tagId).FirstOrDefault();
        return View("tagSongList");
    }
    [HttpGet("newTag")]
    public ViewResult newTag()
    {
        return View("newTag");
    }
    [HttpPost("CreateTag")]
    public IActionResult CreateTag(Tag newTag)
    {
        var tagInDb = _context.Tag.FirstOrDefault(u => u.TagName == newTag.TagName);
        // If a tag exists with provided name
        if(tagInDb != null)
        {
            // Add an error to ModelState and return to View!
            ModelState.AddModelError("TagName", "That Tag Already Exists!");
            return View("newTag");
        }
        if((int)newTag.PrivateCheck == 1)
        {
            PrivateTag newPrivate = new PrivateTag()
            {
                TagName = newTag.TagName,
                TagDescription = newTag.TagDescription,
                UserId = (int)HttpContext.Session.GetInt32("ID")
            };
            _context.Add(newPrivate);
        }
        else
        {
            _context.Add(newTag);
        }

        _context.SaveChanges();
        return Redirect("dashboard");
    }
    [HttpGet("playlists")]
    async public Task<ViewResult> playlists()
    {
        var playlist = await spotify.Playlists.CurrentUsers();
        ViewBag.playlist = playlist.Items.ToList();
        return View("playlists");
    }
    [HttpGet("songList/{playlistId}")]
    async public Task<ViewResult> songList(string playlistId)
    {
        var singlePlaylist = await spotify.Playlists.Get(playlistId);
        List<Song> songs = new List<Song>();
        foreach (PlaylistTrack<IPlayableItem> item in singlePlaylist.Tracks.Items)
            {
            if (item.Track is FullTrack track1)
            {
                //This is how you get track properties from playlists
                // All FullTrack properties are available
                songs.Add(new Song() { SongName = track1.Name, Id = track1.Id});
            }
            if (item.Track is FullEpisode episode)
            {
                // All FullTrack properties are available
                Console.WriteLine(episode.Name);
            }
        }
        ViewBag.allSongs = songs;
        return View();
    }
    [HttpGet("addTag/{songID}")]
    async public Task<ViewResult> tagSong(string songId)
    {
        var singleSong = await spotify.Tracks.Get(songId);
        ViewBag.songName = singleSong.Name;
        ViewBag.songId = singleSong.Id;
        ViewBag.tags = _context.Tag.ToList();
        // Song song = new Song();
        // song.Id = songId;
        // song.SongName = singleSong.Name;
        return View("addTag");
    }
    [HttpPost("addTagtoSong/{songId}")]
    async public Task<RedirectToActionResult> addTagToSong(string songId, int tagId)
    {
        var singleSong = await spotify.Tracks.Get(songId);
        var songInDb = _context.Song.FirstOrDefault(u => u.SongName == singleSong.Name);
        int userId = (int)HttpContext.Session.GetInt32("ID");
        if(songInDb == null)
        {
            Song song = new Song(){
                Id = songId,
                SongName = singleSong.Name,
                SongAlbum = " ",
                SongArtist = " "
            };
            _context.Add(song);
            _context.SaveChanges();
            var songIdCheck = _context.Song.FirstOrDefault(x => x.Id == songId);
            SongsToTags newTag = new SongsToTags(){
                TagId = (int)tagId,
                SongId = songIdCheck.SongId
            };
            UserToSongs userSong = new UserToSongs(){
                UserId = userId,
                SongId = songIdCheck.SongId
            };
            _context.Add(newTag);
            _context.Add(userSong);
        }
        else
        {
            SongsToTags newTag = new SongsToTags(){
                TagId = tagId,
                SongId = songInDb.SongId
            };
            UserToSongs userSong = new UserToSongs(){
                UserId = userId,
                SongId = songInDb.SongId
            };
            _context.Add(newTag);
            _context.Add(userSong);
        }
        _context.SaveChanges();
        return RedirectToAction("dashboard");
    }

    // [HttpGet("userSongs")]
    // public ViewResult userSongs()
    // {
    //     int userId = (int)HttpContext.Session.GetInt32("ID");
    //     ViewBag.userSongs = _context.UserSongs.Include(s => s.Song).Where(h => h.UserId == userId);
    //     ViewBag.songTags = _context.SongTags.Include(t => t.Tag);
    //     return View("userSongs");
    // }

    [HttpGet("SpotifyTest")]
    async public Task<ViewResult> spotifyTest()
    {
        var track = await spotify.Tracks.Get("11dFghVXANMlKmJXsNCbNl");
        //This is how you get playlist information which includes tracks
        var singlePlaylist = await spotify.Playlists.Get("4hExAJOg2866pd1Z6PIJUy");
        //This gets you all playlists associated with the active user
        var playlist = await spotify.Playlists.CurrentUsers();
        List<Song> songs = new List<Song>();
        foreach (PlaylistTrack<IPlayableItem> item in singlePlaylist.Tracks.Items)
            {
            if (item.Track is FullTrack track1)
            {
                //This is how you get track properties from playlists
                // All FullTrack properties are available
                Console.WriteLine(track1.Name);
                songs.Add(new Song() { SongName = track1.Name, Id = track1.Id});
                Console.WriteLine(track1.Id);
                // songs.Add(new Song() { Id = track1.Id});
            }
            if (item.Track is FullEpisode episode)
            {
                // All FullTrack properties are available
                Console.WriteLine(episode.Name);
            }
        }

        ViewBag.track = track;
        ViewBag.allSongs = songs;
        ViewBag.playlist = playlist.Items.ToList();
        ViewBag.songId = ViewBag.playlist[0].Id;
        return View("playback");
    }
}