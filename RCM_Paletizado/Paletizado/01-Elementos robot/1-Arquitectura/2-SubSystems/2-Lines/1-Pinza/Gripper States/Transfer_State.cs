using System;
using System.Collections.Generic;

public class TransferState_State : IGantryState
{
    private readonly Action _transferedElementLeft;

    public TransferState_State(Action transferedElementLeft)
    {
        if (transferedElementLeft == null)
            throw new NullReferenceException("transferedElementLeft");

        _transferedElementLeft = transferedElementLeft;
    }

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
        _transferedElementLeft();
    }

    public void ElementCatched()
    {
        throw new NotImplementedException();
    }

    #endregion
}