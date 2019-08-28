using System.Collections.Generic;
using System.Linq;
using Idpsa.Control;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class LinesSemaphore
    {
        private readonly List<IDLine> _requests;
        public bool ModoAcumulacion_L1;
        public bool ModoAcumulacion_L2_T1;
        public bool ModoAcumulacion_L2_T2;

        public LinesSemaphore()
        {
            _requests = new List<IDLine>();
        }

        private IDLine? Semaphore
        {
            get
            {
                //return (_requests.Count == 0) ? (IDLine?)null : _requests.FirstOrDefault(); 
                //MDG.2011-05-30
                return (_requests.Count == 0) ? (IDLine?) null : _requests.FirstOrDefault();
            }
        }

        public void Request(IDLine idLine)
        {
            if (!_requests.Contains(idLine))
                _requests.Add(idLine);
        }

        public void QuitRequest(IDLine idLine)
        {
            _requests.Remove(idLine);
        }

        public bool HasPermission(IDLine idLine)
        {
            if (Semaphore == null)
                return false;

            bool value = false;

            //MDG.2011-05-30.Decision en funcion de si estamos en modo acumulacion o no
            if (Contains(idLine)) //Comprobamos que esté en el semaforo la petición
            {
                if (idLine == IDLine.Japonesa && ModoAcumulacion_L1)
                    value = true;
                else if (idLine == IDLine.Alemana && ModoAcumulacion_L2_T1)
                    value = true;
                else if (idLine == IDLine.Alemana && ModoAcumulacion_L2_T2)
                    value = true;
            }

            return value;
        }

        //MDG.2011-06-22
        public bool Contains(IDLine idLine)
        {
            if (Semaphore == null)
                return false;

            return _requests.Contains(idLine);
        }

        public IEvaluable GetEvaluableRepresentation()
        {
            return Evaluable.FromFunctor(() => Semaphore.HasValue)
                .WithToString(() => Semaphore.ToString())
                .WithManualRepresentation("Semaforo lineas de numerado");
        }
    }
}