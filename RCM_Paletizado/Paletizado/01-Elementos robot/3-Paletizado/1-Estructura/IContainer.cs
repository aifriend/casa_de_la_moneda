using Idpsa.Paletizado.Definitions;

namespace Idpsa.Paletizado
{
    public interface IContainer : ILocation
    {
        void ElementAdded(IElement item);
        IElement ElementQuitted();
    }
}