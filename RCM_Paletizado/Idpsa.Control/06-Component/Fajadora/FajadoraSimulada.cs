namespace Idpsa.Control.Component
{
    internal class FajadoraSimulada : IFajadora
    {
        #region Miembros de IFajadora

        private readonly IActivable _actuator = new ActuatorSimulated();

        public bool Busy()
        {
            return false;
        }

        public bool Done()
        {
            return true;
        }

        public IActivable Fajar()
        {
            return _actuator;
        }

        public bool Fallo()
        {
            return false;
        }

        public bool FinalFajar()
        {
            return true;
        }

        public bool InicioFajar()
        {
            return true;
        }

        public bool Preparada()
        {
            return true;
        }

        public bool Ready()
        {
            return true;
        }

        #endregion
    }
}