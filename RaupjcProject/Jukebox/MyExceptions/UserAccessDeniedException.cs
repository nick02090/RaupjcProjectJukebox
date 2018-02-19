using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaupjcProject.Jukebox.MyExceptions
{
    public class UserAccessDeniedException : Exception
    {
        public UserAccessDeniedException()
        {
        }

        public UserAccessDeniedException(string message) : base(message)
        {
        }

        public UserAccessDeniedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
