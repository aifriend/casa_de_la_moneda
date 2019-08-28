namespace Idpsa.Paletizado
{
    public interface IItemSolicitor<TItem>
    {
        bool ReadyToPutElement { get; }
        TItem PutElement(IItemSuplier<TItem> suplier);
    }
}