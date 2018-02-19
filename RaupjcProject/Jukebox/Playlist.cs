using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox
{
    public class Playlist
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public bool IsCreated { get; set; }
        public List<Mood> Moods { get; set; }
        public List<Song> Songs { get; set; }
        public User User { get; set; }

        public Playlist()
        {
            //for entity
        }

        public Playlist(string name, User user)
        {
            Id = Guid.NewGuid();
            Name = name;
            Moods = new List<Mood>();
            Songs = new List<Song>();
            IsCreated = false;
            User = user;
        }

        public Playlist(string name, byte[] image, User user)
        {
            Id = Guid.NewGuid();
            Name = name;
            Moods = new List<Mood>();
            Songs = new List<Song>();
            IsCreated = false;
            Image = image;
            User = user;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Playlist)) return false;
            var temp = (Playlist)obj;
            return temp.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
