using RaupjcProject.Jukebox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Models.JukeboxViewModels
{
    public class AddPlaylistMoodsViewModel
    {
        public List<Mood> AllMoods { get; set; }
        public string Search { get; set; }
        public List<Mood> SearchedMoods { get; set; }
        public Playlist Playlist { get; set; }

        public AddPlaylistMoodsViewModel()
        {
            Search = null;
            SearchedMoods = null;
        }

        public AddPlaylistMoodsViewModel(Playlist playlist, List<Mood> allMoods)
        {
            Search = null;
            SearchedMoods = null;
            Playlist = playlist;
            AllMoods = allMoods;
        }
    }
}
