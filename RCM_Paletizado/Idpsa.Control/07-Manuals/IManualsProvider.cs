using System.Collections.Generic;

namespace Idpsa.Control.Manuals
{
    public interface IManualsProvider
    {
        IEnumerable<Manual> GetManualsRepresentations();
    }
}