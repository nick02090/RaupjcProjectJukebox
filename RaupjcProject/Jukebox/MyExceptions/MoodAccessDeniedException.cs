using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox.MyExceptions
{
    public class MoodAccessDeniedException : Exception
    {
        public MoodAccessDeniedException()
        {
        }

        public MoodAccessDeniedException(string message) : base(message)
        {
        }

        public MoodAccessDeniedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
