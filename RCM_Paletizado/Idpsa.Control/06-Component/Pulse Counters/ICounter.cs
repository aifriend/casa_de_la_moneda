namespace Idpsa.Control.Component
{
    public interface ICounter
    {
        bool StopCounter { get; set; }
        bool BackWardsMode { get; set; }
        int MaxValue { get; }
        int MinValue { get; }
        bool Reset();
        bool SetValue(int value);
        int GetValue();
    }
}