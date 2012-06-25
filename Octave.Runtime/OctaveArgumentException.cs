using System;

namespace Octave
{
    class OctaveArgumentException : ArgumentException
    {
        internal OctaveArgumentException(string message,string paramName):base(message,paramName)
        {
            
        }
    }
}
