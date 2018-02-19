using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Nickname { get; set; }
        public byte[] Picture { get; set; }
        public List<Playlist> Playlists { get; set; }

        public User()
        {
            //for entity
        }

        public User(string id, string nickname)
        {
            Id = id;
            Nickname = nickname;
            Playlists = new List<Playlist>();
        }

        public User(string id, string nickname, byte[] picture)
        {
            Id = id;
            Nickname = nickname;
            Picture = picture;
            Playlists = new List<Playlist>();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is User)) return false;
            var temp = (User)obj;
            return temp.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
