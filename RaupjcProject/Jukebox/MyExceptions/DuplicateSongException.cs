using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox.MyExceptions
{
    public class DuplicateSongException : Exception
    {
        public DuplicateSongException()
        {
        }

        public DuplicateSongException(string message) : base(message)
        {
        }

        public DuplicateSongException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
