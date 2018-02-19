using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox.MyExceptions
{
    public class SongAccessDeniedException : Exception
    {
        public SongAccessDeniedException()
        {
        }

        public SongAccessDeniedException(string message) : base(message)
        {
        }

        public SongAccessDeniedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
