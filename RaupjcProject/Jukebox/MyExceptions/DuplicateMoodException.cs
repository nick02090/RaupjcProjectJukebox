using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox.MyExceptions
{
    public class DuplicateMoodException : Exception
    {
        public DuplicateMoodException()
        {
        }

        public DuplicateMoodException(string message) : base(message)
        {
        }

        public DuplicateMoodException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
