using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox.MyExceptions
{
    public class PlaylistAccessDeniedException : Exception
    {
        public PlaylistAccessDeniedException()
        {
        }

        public PlaylistAccessDeniedException(string message) : base(message)
        {
        }

        public PlaylistAccessDeniedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
