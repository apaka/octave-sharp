using System;

namespace Octave
{
    public class OctaveRuntimeException : Exception
    {
        public OctaveRuntimeException(string message) : base(message)
        {
        }
    }
}