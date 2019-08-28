namespace Idpsa.Control.Component
{
    public interface ICilindro
    {
        string Name { get; }
        bool EnReposo { get; }
        bool EnTrabajo { get; }
        void Muerto();
        void Reposo();
        void Trabajo();
    }
}