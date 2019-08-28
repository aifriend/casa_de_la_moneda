using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using IdpsaControl.Tool;

namespace Idpsa.Paletizado
{
    public class TramoTransporteGruposPasaportes : IItemSolicitor<GrupoPasaportes>,
                                                   IItemSuplier<GrupoPasaportes>,
                                                   IAutomaticRunnable2,
                                                   IRi, IRi2
    {
        #region States enum

        public enum States
        {
            WaitNewItem,
            QuitItem,
            MoveItems
        };

        #endregion

        private static IdpsaSystemPaletizado _sys;

        [Manual(SuperGroup = "Transporte", Group = "Cintas")] private readonly ICylinder _empujador;

        [Manual(SuperGroup = "Transporte", Group = "Cintas")] private readonly Actuator _motor;

        [Manual(SuperGroup = "Transporte", Group = "Cintas")] private readonly Sensor _sSalida;
        private readonly IItemSolicitor<GrupoPasaportes> _solicitor;
        private Elevador1 _elevador1; //MDG.2011-03-01. Para hacer suscripcion al elevador y ver su estado

        private States _state;

        public TramoTransporteGruposPasaportes(IdpsaSystemPaletizado sys, string name, int capacity, Actuator motor,
                                               Sensor sensorEntrada,
                                               Sensor sensorSalida, ICylinder empujador,
                                               IItemSolicitor<GrupoPasaportes> solicitor, Elevador1 elevador1)
        {
            Name = name;
            _sys = sys;
            _motor = motor;
            _sSalida = sensorSalida;
            _empujador = empujador;
            _solicitor = solicitor;
            //_supplier = supplier;//MDG.2011-03-01
            _elevador1 = elevador1;
            //MDG.2011-03-01. suscripcion al elevador 1 en tramo 1 y ver su estado para en funcion de él mover la cinta de transporte
            Capacity = capacity;
            Grupos = new FifoList<GrupoPasaportes>();
            Grupos.ElementPutted += GroupPuttedHandler;
            Grupos.ElementQuitted += GroupQuittedHandler;
            Grupos.ElementsCleared += GroupsClearedHandler;
        }

        //declaracion old que se sigue usando para tramo de tansporte 2
        public TramoTransporteGruposPasaportes(IdpsaSystemPaletizado sys, string name, int capacity, Actuator motor,
                                               Sensor sensorEntrada,
                                               Sensor sensorSalida, ICylinder empujador,
                                               IItemSolicitor<GrupoPasaportes> solicitor)
        {
            Name = name;
            _sys = sys;
            _motor = motor;
            _sSalida = sensorSalida;
            _empujador = empujador;
            _solicitor = solicitor;
            //_supplier = supplier;//MDG.2011-03-01
            //_elevador1 = elevador1;//MDG.2011-03-01
            Capacity = capacity;
            Grupos = new FifoList<GrupoPasaportes>();
            Grupos.ElementPutted += GroupPuttedHandler;
            Grupos.ElementQuitted += GroupQuittedHandler;
            Grupos.ElementsCleared += GroupsClearedHandler;
        }

        public int Capacity { get; set; }
        public FifoList<GrupoPasaportes> Grupos { get; private set; }

        public bool Vaciar { get; set; }
        public bool ModoAcumulacion { get; set; }
        public string Name { get; private set; }

        public event EventHandler<DataEventArgs<GrupoPasaportes>> GroupPutted;
        public event EventHandler<DataEventArgs<GrupoPasaportes>> GroupQuitted;
        public event EventHandler<EventArgs> GroupsCleared;


        public void SubscribeElevador1(Elevador1 elevador1)
        {
            _elevador1 = elevador1;
        }

        private void GroupPuttedHandler(object sender, DataEventArgs<GrupoPasaportes> grupo)
        {
            EventHandler<DataEventArgs<GrupoPasaportes>> temp = GroupPutted;
            if (temp != null)
            {
                temp(this, grupo);
            }
        }

        private void GroupQuittedHandler(object sender, DataEventArgs<GrupoPasaportes> grupo)
        {
            EventHandler<DataEventArgs<GrupoPasaportes>> temp = GroupQuitted;
            if (temp != null)
            {
                temp(this, grupo);
            }
        }

        private void GroupsClearedHandler(object sender, EventArgs grupo)
        {
            EventHandler<EventArgs> temp = GroupsCleared;
            if (temp != null)
            {
                temp(this, EventArgs.Empty);
            }
        }

        private bool IsFull()
        {
            return (Grupos.Count >= Capacity);
        }

        private bool IsEmpty()//MDG.2013-03-26
        {
            return (Grupos.Count <= 0);
        }

        //MDG.2010-12-02.Metodos de salvado y carga de los grupos de pasaportes almacenados en los transportadores aereos
        public StoredDataTransportGroups GetDataToStore()
        {
            return new StoredDataTransportGroups //
                       {
                           Groups = Grupos.List,
                           //_grupos,//Group = GrupoPasaportes,//_production.GetBoxes().Select(b => b.Id).ToList(),
                           State = _state,
                           Name = Name,
                           Vaciar = Vaciar,
                           //MDG.2010-12-09
                           ModoAcumulacion = ModoAcumulacion //MDG.2010-12-09
                       };
        }

        public void SetDataStored(StoredDataTransportGroups grupoCargado)
        {
            Grupos.List = grupoCargado.Groups;
            _state = grupoCargado.State;
            //Vaciar=GrupoCargado.Vaciar;//MDG.2010-12-09
            //ModoAcumulacion = GrupoCargado.ModoAcumulacion;//MDG.2010-12-09
        }

        #region Miembros de IItemSolicitor<GrupoPasaportes>

        public GrupoPasaportes PutElement(IItemSuplier<GrupoPasaportes> supplier)
        {
            GrupoPasaportes item = supplier.QuitItem();
            Grupos.Put(item);
            _state = States.MoveItems;
            return item;
        }

        public bool ReadyToPutElement
        {
            //get{return (_state == States.WaitNewItem);}
            //get { return ((_state == States.WaitNewItem) || (_state == States.MoveItems)); }
            get { return (((_state == States.WaitNewItem) || (_state == States.MoveItems)) && !(IsFull())); }
            //2011-05-31
            //MDG.2011-05-30.Diferencia entre Tramo 1 y tramo 2. El tramo
            //get {
            //    return ( (_state == States.WaitNewItem) || ( (_state == States.MoveItems)&& (Name=="Tramo 1") ) ); 
            //}
        }

        #endregion

        #region Miembros de IItemSuplier<GrupoPasaportes>

        public GrupoPasaportes QuitItem()
        {
            //return _grupos.Quit();
            ////MDG.2010-11-25.Grupo vacio para vaciado sin informacion
            var GrupoVacio = new GrupoPasaportes(); //Elemento para usarlo en
            if (Grupos.Count > 0)
                return Grupos.Quit();
            else
                return GrupoVacio; //MDG.2010-11-25.Grupo vacio para vaciado sin informacion
        }

        #endregion

        #region Miembros de IAutomaticRunnable2

        IEnumerable<Chain> IAutomaticRunnable2.GetAutoChains2()
        {
            return new[] {new AutoChainTramoTransporte(Name, this)};
        }

        #endregion

        #region Nested type: AutoChainTramoTransporte

        private class AutoChainTramoTransporte : StructuredChain
        {
            private const int GapTime = 2000;

            //MDG.2011-03-31.Minimizamos tiempo avance adicional porque avanza ya mientras se empuja//450;//500;//MDG.2010-12-03

            private const int GapTimeModoAcumulacionTramo2 = 800; //MDG.2010-12-13.Tramo 2 diferenciado
            private const int StopTime = 500;
            private readonly TramoTransporteGruposPasaportes _tramo;
            private int _gapTimeModoAcumulacionTramo1 = 200;
            private DynamicStepBody _reposoStep;
            private TON _timer;
            private TON _timerOff;

            public AutoChainTramoTransporte(string name, TramoTransporteGruposPasaportes tramo)
                : base(name)
            {
                _tramo = tramo;
                InitializeEmpujadorStep();
                AddSteps();
            }

            protected override void AddSteps()
            {
                Main_Steps();
                WaitNewItem_Steps();
                QuitItem_Steps();
                MoveItems_Steps();
            }

            private void Main_Steps()
            {
                MainChain.Add(new Step("Paso inicial")).Task = () => { NextStep(); };

                MainChain.Add(new Step("Elegir cadena a ejecutar") {StopChain = true})
                    .Task = () =>
                                {
                                    ResetTimers();
                                    CallChain(
                                        _tramo._state);
                                };

                MainChain.Add(new Step("Paso final")).Task = () => { PreviousStep(); };
            }

            private void WaitNewItem_Steps()
            {
                Subchain chain = AddSubchain(States.WaitNewItem);

                chain.Add(new Step()).SetDynamicBehaviour(() => _reposoStep);

                chain.Add(new Step("Esperar entrada grupo pasaportes en " + Name) {StopChain = true})
                    .Task = () =>
                                {
                                    if (_tramo._state != States.WaitNewItem)
                                    {
                                        Return();
                                    }
                                    else
                                    {
                                        if (!_sys.Rulhamat.RobotEnlaceEnabled
                                            || _sys.Lines.Line2.TransporteLinea.longTimeEmpty//MDG.2013-03-26.Para que se pare a los 2 minutos
                                            )
                                        {
                                            _tramo._motor.Activate(false);
                                            Return();
                                        }
                                        else
                                        {
                                            if (((_tramo.Vaciar) || (!_tramo.ModoAcumulacion))
                                                && !(_sys.Rulhamat.RobotEnlaceEnabled && !_tramo._solicitor.ReadyToPutElement)) //MCR 2016
                                            {
                                                _tramo._state = States.MoveItems;
                                                Return();
                                            }
                                            else
                                            {
                                                //está en espera
                                                if ((_tramo.Name.ToUpper() == "TRAMO 1"))
                                                {
                                                    //MDG.2011-03-17.Nueva condicion de avance. Sólo si está avanzando el empujador
                                                    if ((_tramo.ModoAcumulacion ||(_sys.Rulhamat.RobotEnlaceEnabled && !_tramo._solicitor.ReadyToPutElement)) //MCR 2016
                                                        && _tramo._elevador1.State == Elevador1.States.Vaciar
                                                        //&& this._tramo._elevador1.IsPusher(Elevador1.Pusher.Extend)
                                                        && !_tramo._elevador1.IsPusher(Elevador1.Pusher.Extend)
                                                        //MDG.2011-03-30
                                                        && !_tramo._elevador1.IsPusher(Elevador1.Pusher.Retract)
                                                        //MDG.2011-03-31
                                                        && _tramo._elevador1.StateEmpujando) //MDG.2011-04-11
                                                    {
                                                        _tramo._motor.Activate(true);
                                                    }
                                                    else
                                                    {
                                                        if (!(_tramo.ModoAcumulacion ||(_sys.Rulhamat.RobotEnlaceEnabled && !_tramo._solicitor.ReadyToPutElement))
                                                            && _tramo._elevador1.State == Elevador1.States.Vaciar)
                                                            //if (this._tramo._elevador1._state == Elevador1.States.Vaciar)
                                                            //_sys.Lines.Line2.TransporteLinea.Elevador1))
                                                            //&& (_sys.Lines.Line2.TransporteLinea.Elevador1))
                                                        {
                                                            _tramo._motor.Activate(true);
                                                        }
                                                        else
                                                        {
                                                            _tramo._motor.Activate(false);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    _tramo._motor.Activate(false);
                                                }
                                                Return();
                                            }
                                        }
                                    }
                                };
            }

            private void QuitItem_Steps()
            {
                Subchain chain = AddSubchain(States.QuitItem);

                chain.Add(new Step()).SetDynamicBehaviour(() => _reposoStep);

                chain.Add(new Step("Esperar permiso salida de pasaportes " + Name) {StopChain = true})
                    .Task = () =>
                                {
                                    if (!_tramo._solicitor.ReadyToPutElement) return;
                                    _timer.Reset();
                                    NextStep();
                                };

                chain.Add(new Step("Comprobar empujador pasaportes en trabajo de " + Name, 3000)).Task =
                    () =>
                        {
                            _tramo._empujador.Work();
                            if (_timer.TimingWithReset(100, _tramo._empujador.InWork))
                            {
                                _tramo._empujador.Rest();
                               if (_tramo.Grupos!=null&&_tramo.Grupos.Count>0)
                                    _tramo._solicitor.PutElement(_tramo); //MCR. 2016

                                if (_tramo.Grupos.Empty)
                                    ; //MDG.Los tramos continuan en estado de vaciar

                                if (!((_tramo.Vaciar) || (!_tramo.ModoAcumulacion))
                                     || (_sys.Rulhamat.RobotEnlaceEnabled&&!_tramo._solicitor.ReadyToPutElement)) //MCR. 2016.
                                    _tramo._state = States.WaitNewItem;
                                else //vaciado
                                    if (_tramo.Grupos.Empty)
                                        _tramo._state = States.WaitNewItem;
                                    else
                                        _tramo._state = States.MoveItems;

                                _timer.Reset();
                                Return();
                            }
                        };
            }

            private void MoveItems_Steps()
            {
                Subchain chain = AddSubchain(States.MoveItems);

                chain.Add(new Step()).SetDynamicBehaviour(() => _reposoStep);

                chain.Add(new Step("Mover grupos de pasaportes en " + Name))
                    .Task = () =>
                                {
                                    
                                    if (!_sys.Rulhamat.RobotEnlaceEnabled
                                        || _sys.Lines.Line2.TransporteLinea.longTimeEmpty//MDG.2013-03-26.Para que se pare a los 2 minutos
                                        )
                                    {
                                        _tramo._motor.Activate(false);
                                        Return();
                                    }
                                    else
                                    {
                                        //_tramo._motor.Activate(true);//MDG.2011-03-31.Movemos sólo si hace falta
                                        //if (_tramo.IsFull()||_tramo.Vaciar||_tramo._sSalida.Value())
                                        if (_tramo.IsFull() ||
                                            (((_tramo.Vaciar) || (!_tramo.ModoAcumulacion))
                                            && !(_sys.Rulhamat.RobotEnlaceEnabled && !_tramo._solicitor.ReadyToPutElement)) || //mcr.2016
                                            _tramo._sSalida.Value())
                                            //MDG.2011-06-30. Modo Normal (no acumulando) siempre vacía
                                        {
                                            if (_tramo._sSalida.Value())
                                            {
                                                if (_tramo.Grupos.Count > 0) //MCR. 2016
                                                {
                                                    //_timer = new TON();
                                                    //NextStep();
                                                    //MDG.2010-12-01.Quito temporizacion transportes de salida.El siguiente paso no se ejecuta
                                                    _tramo._motor.Activate(false);
                                                    _tramo._state = States.QuitItem;
                                                    Return();
                                                }
                                                else if (_tramo._solicitor.ReadyToPutElement)//MCR. 2016.
                                                {
                                                    _tramo._motor.Activate(false);
                                                    _tramo._empujador.Work();
                                                    GoToStep("Recoger cilindro");
                                                    
                                                }
                                            }
                                            else
                                            {
                                                _tramo._motor.Activate(true); //MDG.2011-03-31
                                            }
                                        }
                                        else
                                        {
                                            if (_tramo.ModoAcumulacion//MDG.2010-12-03
                                                 || (_sys.Rulhamat.RobotEnlaceEnabled && !_tramo._solicitor.ReadyToPutElement)) //MCR.2016
                                            {
                                                if (_tramo.Name.ToUpper() == "TRAMO 1")
                                                {
                                                    //if (_tramo._sEntrada.Value())//MDG.2011-03-31
                                                    //{
                                                    //    _tramo._motor.Activate(false);
                                                    //    _tramo._state = States.WaitNewItem;
                                                    //    Return();
                                                    //}
                                                    //else
                                                    //{
                                                    //if (_timer.Timing(_gapTimeModoAcumulacionTramo1))//MDG.2010-12-03
                                                    //{
                                                    _tramo._motor.Activate(false);
                                                    _tramo._state = States.WaitNewItem;
                                                    Return();
                                                    //}
                                                    //else
                                                    //{
                                                    //    _tramo._motor.Activate(true);//MDG.2011-03-31
                                                    //}
                                                    //}
                                                }
                                                else
                                                {
                                                    if (_timer.Timing(GapTimeModoAcumulacionTramo2)) //MDG.2010-12-13
                                                    {
                                                        _tramo._motor.Activate(false);
                                                        _tramo._state = States.WaitNewItem;
                                                        Return();
                                                    }
                                                    else
                                                    {
                                                        _tramo._motor.Activate(true); //MDG.2011-03-31
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (_timer.Timing(GapTime))
                                                {
                                                    _tramo._motor.Activate(false);
                                                    _tramo._state = States.WaitNewItem;
                                                    Return();
                                                }
                                                else
                                                {
                                                    _tramo._motor.Activate(true); //MDG.2011-03-31
                                                }
                                            }
                                        }
                                    }
                                };

                chain.Add(new Step("Temporizar parada banda " + Name))
                    .Task = () =>
                                {
                                    //if(_timer.TimingWithReset(_stopTime,_tramo._sSalida.Value()))
                                    //MDG.2010-11-30.Cambiamos temporizacion de deteccion a 50ms en vez de 500ms.
                                    //Las fotocelulas estan colocadas justo al final de la banda. De esta forma se evitan atascos
                                    //if (_timer.TimingWithReset(_stopTime/10, _tramo._sSalida.Value()))
                                    //MDG.2010-12-01.Quito temporizacion transportes de salida
                                    if (_tramo._sSalida.Value())
                                    {
                                        _tramo._motor.Activate(false);
                                        _tramo._state = States.QuitItem;
                                        Return();
                                    }

                                    if (_timerOff.TimingWithReset(StopTime/2, !_tramo._sSalida.Value()))
                                    {
                                        _timer.Reset();
                                        PreviousStep();
                                    }
                                };
                chain.Add(new Step("Recoger Cilindro " + Name).WithTag("Recoger cilindro"))
                    .Task = () =>
                    {
                        if (_timer.TimingWithReset(100, _tramo._empujador.InWork))
                        {
                            _tramo._empujador.Rest();
                            _tramo._motor.Activate(true);
                            _timer.Reset();//mcr.2016
                            Return();
                        }
                    };

            }

            private void InitializeEmpujadorStep()
            {
                var action = new Action(() =>
                                            {
                                                _tramo._empujador.Rest();
                                                //if (_timer.TimingWithReset(1000,_tramo._empujador.InRest))//old
                                                if (_timer.TimingWithReset(100, _tramo._empujador.InRest))
                                                    //MDG.2011-05-11.Only 100 ms necesary
                                                    NextStep();
                                            }
                    );

                _reposoStep = new DynamicStepBody(
                    () => "Comprobar cilindro " + Name + " en reposo", action)
                                  {AdditionalAction = false};
            }

            private void ResetTimers()
            {
                _timer = new TON();
                _timerOff = new TON();
            }
        }

        #endregion

        #region Miembros de IRi

        public void Ri()
        {
            ;// _motor.Activate(false);
        }

        public void Ri2()
        {
            _motor.Activate(false);
        }

        #endregion
    }
}