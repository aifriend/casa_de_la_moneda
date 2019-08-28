using System.Collections.Generic;

public interface IGantryState
{
    Dictionary<string, object> GetChainParams();
    bool NextState();
    void ElementLeft();
    void ElementCatched();
}