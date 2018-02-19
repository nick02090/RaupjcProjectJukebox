using RaupjcProject.Jukebox;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Models.JukeboxViewModels
{
    public class AddMoodsViewModel
    {
        [Required]
        public string Value { get; set; }
        public bool Exists { get; set; }
        public bool MoreThanOne { get; set; }

        public AddMoodsViewModel()
        {
            Exists = false;
            Value = null;
            MoreThanOne = false;
        }
    }
}
