using RaupjcProject.Jukebox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Models.ManageViewModels
{
    public class MyPlaylistsViewModel
    {
        public string StatusMessage { get; set; }
        public List<Playlist> MyPlaylists { get; set; }
        public User User { get; set; }
        public List<Playlist> Playlists { get; set; }

        public MyPlaylistsViewModel()
        {
        }

        public MyPlaylistsViewModel(string statusMessage, List<Playlist> myPlaylists, List<Playlist> playlists, User user)
        {
            StatusMessage = statusMessage;
            MyPlaylists = myPlaylists;
            Playlists = playlists;
            User = user;
        }
    }
}
