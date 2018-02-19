using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RaupjcProject.Jukebox;

namespace RaupjcProject.Models.JukeboxViewModels
{
    public class ListenToPlaylistViewModel
    {
        public Playlist Playlist { get; set; }
        public List<Playlist> FilteredPlaylists { get; set; }
        public string Search { get; set; }

        public ListenToPlaylistViewModel()
        {
            //for entity
        }

        public ListenToPlaylistViewModel(Playlist playlist, List<Playlist> filteredPlaylists, string search)
        {
            Playlist = playlist;
            FilteredPlaylists = filteredPlaylists;
            Search = search;
        }
    }
}
