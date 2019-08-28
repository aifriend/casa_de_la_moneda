using System.Collections.Generic;
using Idpsa.Paletizado;

public interface IGantryState
{
    Dictionary<string, object> GetChainParams();
    bool NextState();
    void ElementLeft();
    void ElementCatched();
}