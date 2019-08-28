namespace Idpsa.Control.Component
{
    public interface ICylinder
    {
        string Name { get; }
        bool InRest { get; }
        bool InWork { get; }
        void Dead();
        void Rest();
        void Work();
    }
}