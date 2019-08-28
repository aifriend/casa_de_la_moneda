using System;
using System.Collections.Generic;

public class Wait_State : IGantryState
{
    #region IGantryState Members

    public Dictionary<string, object> GetChainParams()
    {
        var parameters = new Dictionary<string, object>();
        return parameters;
    }

    public bool NextState()
    {
        return false;
    }

    public void ElementLeft()
    {
        throw new NotImplementedException();
    }

    public void ElementCatched()
    {
        throw new NotImplementedException();
    }

    #endregion
}