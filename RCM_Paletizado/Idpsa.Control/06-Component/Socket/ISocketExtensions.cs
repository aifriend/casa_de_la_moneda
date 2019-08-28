using Idpsa.Control.Component;

namespace Idpsa.Control.Standard
{
    public static class SocketExtensions
    {
        public static bool WiseSocketConnect(this IReader reader)
        {
            bool value = true;

            if (!reader.Connected())
                if (!reader.Connect())
                    value = false;

            return value;
        }

        public static bool WiseSocketDisconnect(this IReader reader)
        {
            bool value = true;

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