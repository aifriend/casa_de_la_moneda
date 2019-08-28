namespace Idpsa.Control.Manuals
{  
    public interface IManualFormatProvider
    {
        bool HasFormatDirectives { get; }
        int SenderNameFontSize { get; }
        int DataFontSize { get; }
    }
}