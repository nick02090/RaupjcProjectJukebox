using RaupjcProject.Jukebox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Models.JukeboxViewModels
{
    public class ViewMoodsViewModel
    {
        public List<Mood> Moods { get; set; }
        public List<int> Frequencies { get; set; }

        public ViewMoodsViewModel()
        {
            //for entity
        }

        public ViewMoodsViewModel(List<Mood> moods, List<int> frequencies)
        {
            Moods = moods;
            Frequencies = frequencies;
        }
    }
}
