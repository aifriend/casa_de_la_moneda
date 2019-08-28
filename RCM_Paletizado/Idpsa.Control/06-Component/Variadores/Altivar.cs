
namespace Idpsa.Control.Component
{
    [System.Serializable()]
	public class Altivar : ActuatorWithInversorSimple,ISlowFastVelocity
	{
		private ActuatorP _contactor;
		private ActuatorP _vSel;
		public Altivar(ActuatorP parContactor, ActuatorWithInversorSimple parActuatorConInv, ActuatorP parVSel) : base(parActuatorConInv)
		{
			_contactor = parContactor;
			_vSel = parVSel;
		}
        
        public void Energize(bool work)
        {
            _contactor.Activate(work);
        }
        public void Energize()
        {
            _contactor.Activate(true);
        }
        public void Desenergize()
        {
            _contactor.Activate(false);
        }
        public void Forward()
        {
            Activate2();
        }
        public void BackWard()
        {
            Activate1();
        }
        public void Stop()
        {
            Deactivate();
        }        

        #region Miembros de ISlowFastVelocity

        public void SlowVelocity()
        {
            _vSel.Activate(false);
        }

        public void FastVelocity()
        {
            _vSel.Activate(true);
        }

        #endregion
    }
}
