using System.Collections.Generic;
using System.Linq;
using Idpsa.Control;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class LinesSemaphore
    {
        private readonly List<IDLine> _requests;
        public bool AutoSeleccion;
        public bool ModoAcumulacion;

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

            //old.2010-12-15. Se tenía permiso siempre
            //if (Semaphore.Value == IDLine.Japonesa)
            //    value = true;
            //else if(Semaphore.Value == IDLine.Alemana)
            //    value = true;

            if (AutoSeleccion) //MDG.2011-06-30
            {
                //MDG.2010-12-15. La linea de entrada tiene permiso solo si coincide con el valor consultado en el semáforo
                if (Semaphore.Value == idLine)
                    value = true;
            }
            else
            {
                //MDG.2011-05-30.Decision en funcion de si estamos en modo acumulacion o no
                if (Contains(idLine)) //Comprobamos que esté en el semaforo la petición
                {
                    if (idLine == IDLine.Japonesa && ModoAcumulacion)
                        value = true;
                    if (idLine == IDLine.Alemana && !ModoAcumulacion)
                        value = true;
                }
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