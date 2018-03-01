using RaupjcProject.Jukebox;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Models.JukeboxViewModels
{
    public class AddPlaylistViewModel
    {
        public List<Song> AllSongs { get; set; }
        public Playlist Playlist { get; set; }
        public User User { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public List<Song> SearchedSongs { get; set; }

        public AddPlaylistViewModel()
        {
            SearchedSongs = null;
            Artist = null;
            Title = null;
        }

        public AddPlaylistViewModel(Playlist playlist)
        {
            SearchedSongs = null;
            Artist = null;
            Title = null;
            Playlist = playlist;
        }

        public AddPlaylistViewModel(Playlist playlist, List<Song> allSongs, User user)
        {
            AllSongs = allSongs;
            User = user;
            SearchedSongs = null;
            Artist = null;
            Title = null;
            Playlist = playlist;
        }
    }
}
