namespace Idpsa.Control.Component
{
    public interface IFajadora
    {
        bool Busy();
        bool Done();
        IActivable Fajar();
        bool Fallo();
        bool FinalFajar();
        bool InicioFajar();
        bool Preparada();
        bool Ready();
    }
}