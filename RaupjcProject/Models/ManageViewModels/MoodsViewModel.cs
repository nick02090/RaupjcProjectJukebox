using RaupjcProject.Jukebox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Models.ManageViewModels
{
    public class MoodsViewModel
    {
        public string StatusMessage { get; set; }
        public List<Mood> Moods { get; set; }
        public User User { get; set; }

        public MoodsViewModel()
        {
            //for entity
        }

        public MoodsViewModel(string statusMessage, List<Mood> moods, User user)
        {
            StatusMessage = statusMessage;
            Moods = moods;
            User = user;
        }
    }
}
