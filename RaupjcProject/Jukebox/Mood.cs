using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox
{
    public class Mood
    {
        [Key]
        public string Value { get; set; }
        public List<Playlist> Playlists { get; set; }

        public Mood()
        {
            //for entity
        }

        public Mood(string value)
        {
            Value = value;
            Playlists = new List<Playlist>();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Mood)) return false;
            var temp = (Mood)obj;
            return temp.Value.ToLower().Equals(Value.ToLower());
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
