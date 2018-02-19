using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox
{
    public class Song
    {
        [Key]
        public Guid Id { get; set; }
        public byte[] Sound { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public List<Playlist> Playlists { get; set; }

        public Song()
        {
            //for entity
        }

        public Song(string artist, byte[] sound, string title)
        {
            Id = Guid.NewGuid();
            Artist = artist;
            Sound = sound;
            Title = title;
            Playlists = new List<Playlist>();
        }

        public Song(string artist, string album, byte[] sound, string title)
        {
            Id = Guid.NewGuid();
            Artist = artist;
            Album = album;
            Sound = sound;
            Title = title;
            Playlists = new List<Playlist>();
        }

        public Song(string artist, string album, string year, byte[] sound, string title)
        {
            Id = Guid.NewGuid();
            Artist = artist;
            Album = album;
            Year = year;
            Sound = sound;
            Title = title;
            Playlists = new List<Playlist>();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Song)) return false;
            var temp = (Song)obj;
            return temp.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
