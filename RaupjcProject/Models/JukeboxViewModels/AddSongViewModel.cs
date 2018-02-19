using RaupjcProject.Jukebox;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace RaupjcProject.Models.JukeboxViewModels
{
    public class AddSongViewModel
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string Artist { get; set; }
        [Required]
        public string Title { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public bool IsMp3 { get; set; } 

        public AddSongViewModel()
        {
            //initially set to true, if tries to upload another extension, changes to false
            IsMp3 = true;
        }
    }
}
