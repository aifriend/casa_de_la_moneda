namespace Idpsa.Control.Component
{
    public interface IJog
    {
        string JogPosName { get; }
        string JogNegName { get; }
        bool EnableJogPos();
        bool EnableJogNeg();
        void JogPos();
        void JogNeg();
        void StopJog();
    }
}