using RaupjcProject.Jukebox;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Models.JukeboxViewModels
{
    public class JukeboxViewModel
    {
        public List<Playlist> AllPlaylists { get; set; }
        public List<Mood> Moods { get; set; }
        public List<Playlist> Playlists { get; set; }
        [Required]
        public string Search { get; set; }
        public List<Playlist> FilteredPlaylists { get; set; }

        public JukeboxViewModel()
        {
            Search = null;
        }

        public JukeboxViewModel(List<Playlist> allPlaylists)
        {
            AllPlaylists = allPlaylists;
            Search = null;
        }

        public JukeboxViewModel(List<Playlist> allPlaylists, List<Playlist> filteredPlaylists, string search)
        {
            AllPlaylists = allPlaylists;
            FilteredPlaylists = filteredPlaylists;
            Search = search;
        }
    }
}
