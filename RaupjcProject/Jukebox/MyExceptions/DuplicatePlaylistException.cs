using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox.MyExceptions
{
    public class DuplicatePlaylistException : Exception
    {
        public DuplicatePlaylistException()
        {
        }

        public DuplicatePlaylistException(string message) : base(message)
        {
        }

        public DuplicatePlaylistException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
