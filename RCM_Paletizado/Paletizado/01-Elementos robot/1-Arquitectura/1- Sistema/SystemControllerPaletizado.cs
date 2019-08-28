using System;
using Idpsa.Control.Engine;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class SystemProductionRequest
    {
        #region IdRequest enum

        public enum IdRequest
        {
            Catalog,
            ManualValidation,
            TraslateSeccionsLeft,
            TraslateSeccionsRight
        }

        #endregion

        public SystemProductionRequest(IdRequest id)
        {
            Id = id;
        }

        public SystemProductionRequest(IdRequest id, object value)
        {
            Id = id;
            Value = value;
        }

        public IdRequest Id { get; private set; }
        public object Value { get; private set; }
    }

    public class SystemControllerPaletizado : SystemController
    {
        #region Peticiones al control de producción del modelo

        #region Delegates

        public delegate void ProductionControlRequestedHandler(SystemProductionRequest action);

        #endregion

        public event ProductionControlRequestedHandler SystemProductionRequested;

        public void RequestSystemProduction(SystemProductionRequest action)
        {
            if (SystemProductionRequested != null)
                SystemProductionRequested(action);
        }

        public void RequestSystemProduction(SystemProductionRequest.IdRequest idAction)
        {
            if (SystemProductionRequested != null)
                SystemProductionRequested(new SystemProductionRequest(idAction));
        }

        public void RequestSystemProduction(SystemProductionRequest.IdRequest idAction, object value)
        {
            if (SystemProductionRequested != null)
                SystemProductionRequested(new SystemProductionRequest(idAction, value));
        }

        #endregion

        public SystemControllerPaletizado(IdpsaSystemPaletizado sys) : base(sys)
        {
            TieToSystem(sys);
        }

        private void TieToSystem(IdpsaSystemPaletizado sys)
        {
            SystemProductionRequested += sys.Production.ActionRequestedHandler;
        }
    }
}