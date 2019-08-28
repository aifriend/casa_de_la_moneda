using System;
using System.Collections.Generic;
using System.Net;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class EnlaceJaponesa : IManagerRunnable, IDisposable, IItemSuplier<GrupoPasaportes>
    {
        private readonly SocketListener<GrupoPasaportes, GroupResume> _listener;
        [Manual(SuperGroup = "Encajado", Group = "Enlace japonesa")] private readonly IEvaluable _readyToReceiveIn;

        [Manual(SuperGroup = "Encajado", Group = "Enlace japonesa")] private readonly IActivable _readyToReceiveOut;

        [Manual(SuperGroup = "Encajado", Group = "Enlace japonesa")] private readonly IEvaluable _requestJaponesa;

        private readonly LinesSemaphore _semaphore;
        private readonly IItemSolicitor<GrupoPasaportes> _solicitor;
        private readonly SourceGroupSupplier _supplier; //MDG.2011-07-05
        private GrupoPasaportes _groupReceived;

        public EnlaceJaponesa(IPEndPoint endPoint,
                              IActivable readyToReceiveOut,
                              IItemSolicitor<GrupoPasaportes> solicitor,
                              IEvaluable requestJaponesa,
                              LinesSemaphore semaphore,
                              SourceGroupSupplier supplier) //MDG.2011-07-05
        {
            _listener = new SocketListener<GrupoPasaportes, GroupResume>(endPoint.Address, endPoint.Port,
                                                                         _ => new GrupoPasaportes(_));
            _readyToReceiveOut = readyToReceiveOut.WithManualRepresentation("entrada grupos japonesa habilitada");

            _solicitor = solicitor;

            _readyToReceiveIn = Evaluable.FromFunctor(() => solicitor.ReadyToPutElement && _groupReceived != null
                                                            && _supplier.ContainsGroupId(_groupReceived.Id))
                //MDG.2011-07-05
                .DelayToConnection(500)
                .DelayToDisconnection(2000); //MDG.2011-07-05

            _requestJaponesa =
                requestJaponesa.DelayToConnection(1000).WithManualRepresentation("solicitud entrada grupos japonesa");

            _semaphore = semaphore;
            _supplier = supplier; //MDG.2011-07-05
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            ((IDisposable) _listener).Dispose();
        }

        #endregion

        #region Miembros de IItemSuplier<GrupoPasaportes>

        public GrupoPasaportes QuitItem()
        {
            GrupoPasaportes group = _groupReceived;
            _groupReceived = null;
            return group;
        }

        #endregion

        #region IManagerRunnable Members

        IEnumerable<Action> IManagerRunnable.GetManagers()
        {
            return new Action[] {Manager};
        }

        #endregion

        private void Manager()
        {
            _listener.Manager();
            CheckRequestJaponesa();
            //SendReadyToJaponesa();
            ReceiveGroupJaponesa();
            SendReadyToJaponesa(); //permiso empujar
        }

        private void ReceiveGroupJaponesa()
        {
            GrupoPasaportes grupo = null;
            if (_listener.TryGetData(out grupo))
            {
                //MDG.2011-07-05.Comprobacion grupo recibido en catalogo actual
                if (grupo != null) //Comprobacion de que no sea nulo para que no de excepcion
                {
                    if (_supplier != null) //Comprobacion de que no sea nulo para que no de excepcion
                    {
                        if (_supplier.ContainsGroupId(grupo.Id))
                            //Comprobacion de que el id del grupo esta en el catalogo cargado en la linea 1
                        {
                            //Completamos transferencia informacion
                            _groupReceived = grupo;
                            //_solicitor.PutElement(this);
                            //if (grupo.LastOfBox)
                            //{
                            //    _semaphore.QuitRequest(IDLine.Japonesa);
                            //    _requestJaponesa.Reset();
                            //}
                        }
                        else
                        {
                            _groupReceived = null;
                            //Alarma.TO DO
                            ;
                        }
                    }
                }

                //MDG.Old from 2011-07-05
                //_groupReceived = grupo;
                //_solicitor.PutElement(this);
                //if (grupo.LastOfBox)
                //{                    
                //    _semaphore.QuitRequest(IDLine.Japonesa);
                //    _requestJaponesa.Reset();
                //}
            }
        }

        private void SendReadyToJaponesa()
        {
            //_readyToReceiveOut.Activate(_readyToReceiveIn.Value());
            if (_readyToReceiveIn.Value())
            {
                _readyToReceiveOut.Activate(true);
                if (_solicitor.ReadyToPutElement)
                {
                    if (_groupReceived != null)
                    {
                        if (_groupReceived.LastOfBox)
                        {
                            _semaphore.QuitRequest(IDLine.Japonesa);
                            _requestJaponesa.Reset();
                        }
                        _solicitor.PutElement(this);
                    }
                }
            }

            else
            {
                _readyToReceiveOut.Activate(false);
            }
        }

        private void CheckRequestJaponesa()
        {
            if (_requestJaponesa.Value())
                _semaphore.Request(IDLine.Japonesa);
        }
    }
}