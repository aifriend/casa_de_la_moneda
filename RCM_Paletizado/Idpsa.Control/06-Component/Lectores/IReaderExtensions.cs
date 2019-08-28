using Idpsa.Control.Component;

namespace Idpsa.Control.Standard
{
    public static class ReaderExtensions
    {
        public static bool WiseReaderConnect(this IReader reader)
        {
            var value = true;

            if (!reader.Connected())
                if (!reader.Connect())
                    value = false;

            return value;
        }

        public static bool WiseReaderDisconnect(this IReader reader)
        {
            var value = true;

            if (reader.Connected())
            {
                if (!reader.Disconnect())
                {
                    value = false;
                }
            }
            return value;
        }
    }
}