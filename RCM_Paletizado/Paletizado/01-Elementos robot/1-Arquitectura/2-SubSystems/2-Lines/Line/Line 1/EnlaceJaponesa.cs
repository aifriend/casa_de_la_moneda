using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Manuals;


namespace Idpsa.Paletizado
{
    public class EnlaceJaponesa : IManagerRunnable, IDisposable,IItemSuplier<GrupoPasaportes>
    {
        [Manual(SuperGroup = "Encajado", Group = "Enlace japonesa")]
        private IActivable _readyToReceiveOut;
        [Manual(SuperGroup = "Encajado", Group = "Enlace japonesa")]
        private IEvaluable _readyToReceiveIn;
        [Manual(SuperGroup = "Encajado", Group = "Enlace japonesa")]
        private IEvaluable _requestJaponesa;
        private SocketListener<GrupoPasaportes, GroupResume> _listener;
        private LinesSemaphore _semaphore;        
        private IItemSolicitor<GrupoPasaportes> _solicitor;
        private GrupoPasaportes _groupReceived;
        private SourceGroupSupplier _supplier;//MDG.2011-07-05

        public EnlaceJaponesa( IPEndPoint endPoint,
            IActivable readyToReceiveOut,
            IItemSolicitor<GrupoPasaportes> solicitor,IEvaluable requestJaponesa,
            LinesSemaphore semaphore,
            SourceGroupSupplier supplier)//MDG.2011-07-05
        {
            _listener = new SocketListener<GrupoPasaportes,GroupResume>(endPoint.Address, endPoint.Port,(_) => new GrupoPasaportes(_));
            _readyToReceiveOut = readyToReceiveOut.WithManualRepresentation("entrada grupos japonesa habilitada");
            _solicitor = solicitor;
            //_readyToReceiveIn = Evaluable.FromFunctor(() => solicitor.ReadyToPutElement && _groupReceived == null).DelayToConnection(1000); //ALVARO
            //_readyToReceiveIn = Evaluable.FromFunctor(() => solicitor.ReadyToPutElement && _groupReceived != null
            //    ).DelayToConnection(1000); //MDG.2011-07-05
            _readyToReceiveIn = Evaluable.FromFunctor(() => solicitor.ReadyToPutElement && _groupReceived != null
                && _supplier.ContainsGroupId(_groupReceived.Id))//MDG.2011-07-05
                .DelayToConnection(500)
                .DelayToDisconnection(2000);//MDG.2011-07-05
            
            _requestJaponesa = requestJaponesa
                .DelayToConnection(1000)                
                .WithManualRepresentation("solicitud entrada grupos japonesa");
            _semaphore = semaphore;
            _supplier = supplier;//MDG.2011-07-05
        }

        private void Manager()
        {
            CheckRequestJaponesa();
            //SendReadyToJaponesa();
            ReceiveGroupJaponesa();
            SendReadyToJaponesa();//permiso empujar
        }

        private void ReceiveGroupJaponesa()
        {
            GrupoPasaportes grupo = null;
            if (_listener.TryGetData(out grupo))
            {
                //MDG.2011-07-05.Comprobacion grupo recibido en catalogo actual
                if (grupo != null)//Comprobacion de que no sea nulo para que no de excepcion
                {
                    if (_supplier != null)//Comprobacion de que no sea nulo para que no de excepcion
                    {
                        if(_supplier.ContainsGroupId(grupo.Id))//Comprobacion de que el id del grupo esta en el catalogo cargado en la linea 1
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
                    //MDG.2011-07-06
                    if (_groupReceived != null)
                    {
                        if (_groupReceived.LastOfBox)
                        {
                            _semaphore.QuitRequest(IDLine.Japonesa);
                            _requestJaponesa.Reset();
                        }
                    }
                    //MDG.2011-07-05
                    _solicitor.PutElement(this);
                    
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

        #region IManagerRunnable Members

        IEnumerable<Action> IManagerRunnable.GetManagers()
        {
            return ((IManagerRunnable)_listener).GetManagers().Concat(new Action[] { Manager }); 
        }       

        #endregion                

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            ((IDisposable)_listener).Dispose();
        }

        #endregion

        #region Miembros de IItemSuplier<GrupoPasaportes>

        public GrupoPasaportes QuitItem()
        {
            var group = _groupReceived;
            _groupReceived = null;
            return group;
        }

        #endregion
    }













}