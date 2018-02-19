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
        //to show IsPicture that there was an attempt of loading a wrong file
        public bool Try { get; set; }
        public bool IsPicture { get; set; }

        public AddPlaylistNameViewModel()
        {
            IsPicture = false;
            PictureFile = null;
            Try = false;
        }
    }
}
