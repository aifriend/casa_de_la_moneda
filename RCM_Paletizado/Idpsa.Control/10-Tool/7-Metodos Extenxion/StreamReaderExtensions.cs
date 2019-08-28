using System;
using System.Collections.Generic;
using System.IO;

namespace Idpsa.Control.Tool
{
    public static class StreamReaderExtensions
    {
        public static IEnumerable<string> Lines(this StreamReader source)
        {
            if (source == null)
                throw new ArgumentException("source");

            string line;

            while ((line = source.ReadLine()) != null)
                yield return line;
        }
    }
}