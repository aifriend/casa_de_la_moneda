using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Engine;
using Idpsa.Control.Subsystem;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class ControlAireTransportes : IDiagnosisOwner, IManagerRunnable
    {
        //private readonly Input _mandoConectado;
        private readonly IEvaluable _mandoConectado;//MCR. 2016. Mod Rearme Aereos
        private readonly Input _emergenciaActuada;
        private readonly Output _valvulaAireTransportes;
        //private readonly Output _contactoHabilitAscensor1;//MDG.2012-11-12


        public ControlAireTransportes(IEvaluable mandoConectado, Input emergenciaActuada, Output valvulaAireTransportes)//, Output contactoHabilitAscensor1)
        {
            _mandoConectado = mandoConectado;
            _emergenciaActuada = emergenciaActuada;
            _valvulaAireTransportes = valvulaAireTransportes;
            //_contactoHabilitAscensor1 = contactoHabilitAscensor1;
        }


        private Output ValvulaAireTransportes
        {
            get { return _valvulaAireTransportes; }
        }


        //private Output ContactoHabilitAscensor1
        //{
        //    get { return _contactoHabilitAscensor1; }
        //}

        #region IDiagnosisOwner Members

        public IEnumerable<SecurityDiagnosis> GetSecurityDiagnosis()
        {
            var list = new List<SecurityDiagnosis>
                           {
                               
                           };

            return list;
        }

        #endregion

        #region Miembros de IManagerRunnable

        private int _count;

        public IEnumerable<Action> GetManagers()
        {
            return new List<Action> { Gestor };
        }

        private void Gestor()
        {
            ValvulaAireTransportes.Activate(_mandoConectado.Value() && !_emergenciaActuada.Value());
            //ContactoHabilitAscensor1.Activate(_mandoConectado.Value() && !_emergenciaActuada.Value());//MDG.2012-11-12.
        }

        #endregion

    }
}