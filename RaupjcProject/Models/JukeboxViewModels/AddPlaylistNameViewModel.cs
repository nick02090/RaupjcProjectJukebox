using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Models.JukeboxViewModels
{
    public class AddPlaylistNameViewModel
    {
        public IFormFile PictureFile { get; set; }
        [Required]
        public string Name { get; set; }
        //to signalize IsPicture that there was an attempt of loading a wrong file
        public bool Try { get; set; }
        //to signalize PlaylistExists that there was an attempt of naming a playlist with the same name
        public bool Attempt { get; set; }
        public bool IsPicture { get; set; }
        public bool PlaylistExists { get; set; }

        public AddPlaylistNameViewModel()
        {
            IsPicture = false;
            PlaylistExists = false;
            PictureFile = null;
            Try = false;
            Attempt = false;
        }
    }
}
