using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;
using RCMCommonTypes;

namespace Idpsa
{
    public class ReprocesadorManual : ISupplier, IContainer, IAutomaticRunnable, IManualReprocessSolicitor,
                                      IManualsProvider, IRi
    {
        #region States enum

        public enum States
        {
            Free,
            GettingBox,
            ValidatingBox,
            SendingBox,
            WaitingGripper
        }

        #endregion

        private const Locations _location = Locations.Reproceso;

        [Manual(SuperGroup = "General", Group = "Banda rechazo", Description = "Motor")] private readonly
            ActuatorWithInversor
            _motor;

        private IManualReprocessor _reprocessor;
        private States _state;

        public ReprocesadorManual(Bus bus)
        {
            var geo = new GeometriaConf();
            _state = States.Free;
            PositionCatch = geo.PosCogerBandaReproceso; //MCR
           // PositionCatch = Geometria.PosCogerBandaReproceso;
            PositionLeave = new PointSpin3D(PositionCatch.Spin, PositionCatch.Desplazado(-50, 0, 0));
            SEntrada = new SensorP(bus.In("B815"), "Presencia entrada");
            SReproceso = new SensorP(bus.In("B817"), "Presencia reproceso");
            _motor = new ActuatorWithInversorSimple(bus.Out("K734A"), bus.Out("K734B"))
                .WithJogPosName("Adelante")
                .WithJogNegName("Atras")
                .WithInvertedJogSense(false);
        }

        public PointSpin3D PositionCatch { get; private set; }
        public PointSpin3D PositionLeave { get; private set; }

        [Manual(SuperGroup = "General", Group = "Banda rechazo")]
        public IEvaluable SEntrada { get; private set; }

        [Manual(SuperGroup = "General", Group = "Banda rechazo")]
        public IEvaluable SReproceso { get; private set; }

        public CajaPasaportes Caja { get; private set; }

        #region IContainer Members

        public void ElementAdded(IElement item)
        {
            Caja = (CajaPasaportes) item;
            _state = States.GettingBox;
        }

        public IElement ElementQuitted()
        {
            IPaletElement value = Caja;
            Caja = null;
            _state = States.Free;
            return value;
        }

        #endregion

        public void MarchaAdelante()
        {
            _motor.Activate1();
        }

        public void MarchaAtras()
        {
            _motor.Activate2();
        }

        public void Parar()
        {
            _motor.Deactivate();
        }

        public bool Busy()
        {
            if (_state != States.Free)
                return true;
            else
                return false;
        }

        //MDG.2010-12-10.Metodos de salvado y carga de las cajas de pasaportes almacenadas
        public StoredDataBandaReprocesoBox GetDataToStore()
        {
            return new StoredDataBandaReprocesoBox //
                       {
                           Caja = Caja,
                           //Groups = _grupos.List,//_grupos,//Group = GrupoPasaportes,//_production.GetBoxes().Select(b => b.Id).ToList(),
                           State = _state //,
                           //Name = this.Name//,
                           //Vaciar = this.Vaciar,//MDG.2010-12-09
                           //ModoAcumulacion = this.ModoAcumulacion//MDG.2010-12-09
                       };
        }

        public void SetDataStored(StoredDataBandaReprocesoBox GrupoCargado)
        {
            Caja = GrupoCargado.Caja;
            //_grupos.List = GrupoCargado.Groups;
            _state = GrupoCargado.State;
            //Vaciar = GrupoCargado.Vaciar;//MDG.2010-12-09
            //ModoAcumulacion = GrupoCargado.ModoAcumulacion;//MDG.2010-12-09
        }

        #region Miembros de IRi

        void IRi.Ri()
        {
            _motor.Deactivate();
        }

        #endregion

        #region Miembros de ISupplier

        public ElementTypes? ElementSupplied()
        {
            ElementTypes? element = null;
            if (_state == States.WaitingGripper)
            {
                if (Caja == null) return null;
                if (Caja.IdLine == IDLine.Japonesa)
                    return ElementTypes.ItemJaponesa;
                if (Caja.IdLine == IDLine.Alemana)
                    return ElementTypes.ItemAlemana;
                return null;
            }
            else
                return null;
        }

        public Locations Location
        {
            get { return _location; }
        }

        #endregion

        #region Nested type: CadAutoManualReprocess

        private class CadAutoManualReprocess : StructuredChain
        {
            private readonly ReprocesadorManual _reprocess;
            private TON _timer;

            public CadAutoManualReprocess(string name, ReprocesadorManual reprocess) : base(name)
            {
                _reprocess = reprocess;
                AddSteps();
            }

            protected override void AddSteps()
            {
                Main_Steps();
                GettingBox_Steps();
                SendingBox_Steps();
            }

            private void Main_Steps()
            {
                MainChain.Add(new Step("Paso inicial")).Task = () =>
                                                                   {
                                                                       ResetTimers();
                                                                       NextStep();
                                                                   };

                MainChain.Add(new Step("Selección cadena a ejecutar") {StopChain = true}).Task = () =>
                                                                                                     {
                                                                                                         if (
                                                                                                             _reprocess.
                                                                                                                 _state ==
                                                                                                             States.
                                                                                                                 GettingBox)
                                                                                                         {
                                                                                                             CallChain(
                                                                                                                 Chain.
                                                                                                                     GettingBox);
                                                                                                         }
                                                                                                         else if (
                                                                                                             _reprocess
                                                                                                                 .
                                                                                                                 _state ==
                                                                                                             States.
                                                                                                                 SendingBox)
                                                                                                         {
                                                                                                             CallChain
                                                                                                                 (Chain
                                                                                                                      .
                                                                                                                      SendingBox);
                                                                                                         }
                                                                                                     };

                MainChain.Add(new Step("Paso final")).Task = () =>
                                                                 {
                                                                     ResetTimers();
                                                                     PreviousStep();
                                                                 };
            }

            private void GettingBox_Steps()
            {
                Subchain chain = AddSubchain(Chain.GettingBox);

                var timer = new TON();

                chain.Add(new Step("Paso inicial")).Task = () =>
                                                               {
                                                                   timer.Reset();
                                                                   NextStep();
                                                               };

                chain.Add(new Step("Activar motor marcha atras")).Task = () =>
                                                                             {
                                                                                 if (timer.Timing(3000))
                                                                                 {
                                                                                     _reprocess.MarchaAtras();
                                                                                     NextStep();
                                                                                 }
                                                                             };

                chain.Add(new Step("Esperar detección fotocélula zona reprocesado") {StopChain = true}).Task = () =>
                                                                                                                   {
                                                                                                                       if
                                                                                                                           (
                                                                                                                           _timer
                                                                                                                               .
                                                                                                                               TimingWithReset
                                                                                                                               (1000,
                                                                                                                                _reprocess
                                                                                                                                    .
                                                                                                                                    SReproceso
                                                                                                                                    .
                                                                                                                                    Value
                                                                                                                                    ()))
                                                                                                                       {
                                                                                                                           _reprocess
                                                                                                                               .
                                                                                                                               Parar
                                                                                                                               ();
                                                                                                                           _reprocess
                                                                                                                               .
                                                                                                                               _state
                                                                                                                               =
                                                                                                                               States
                                                                                                                                   .
                                                                                                                                   ValidatingBox;
                                                                                                                           _reprocess
                                                                                                                               .
                                                                                                                               _reprocessor
                                                                                                                               .
                                                                                                                               OnNewRequest
                                                                                                                               ();
                                                                                                                           Return
                                                                                                                               ();
                                                                                                                       }
                                                                                                                   };
            }

            private void SendingBox_Steps()
            {
                Subchain chain = AddSubchain(Chain.SendingBox);

                chain.Add(new Step("Activar motor marcha adelante")).Task = () =>
                                                                                {
                                                                                    _reprocess.MarchaAdelante();
                                                                                    NextStep();
                                                                                };

                chain.Add(new Step("Esperar detección fotocélula entrada banda reprocesado") {StopChain = true}).Task =
                    () =>
                        {
                            if (_timer.TimingWithReset(1000, _reprocess.SEntrada.Value()))
                            {
                                _reprocess.Parar();
                                _reprocess._state = States.WaitingGripper;
                                Return();
                            }
                        };
            }

            private void ResetTimers()
            {
                _timer = new TON();
            }

            #region Nested type: Chain

            private enum Chain
            {
                GettingBox,
                SendingBox
            }

            #endregion
        }

        #endregion

        #region Miembros de IAutomaticRunnable

        IEnumerable<Chain> IAutomaticRunnable.GetAutoChains()
        {
            return new[] {new CadAutoManualReprocess("Reprocesado manual", this)};
        }

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return (this).GetManualsRepresentations();
        }

        #endregion

        #region Miembros de IManualReprocessSolicitor

        public bool WaitingReprocess
        {
            get { return _state == States.ValidatingBox; }
        }

        public CajaPasaportes GetBoxToReproccess()
        {
            if (!WaitingReprocess)
                return null;
            else
                return Caja;
        }

        public void OnReprocess()
        {
            _state = States.SendingBox;
        }

        public void AttachToReprocessor(IManualReprocessor reprocessor)
        {
            _reprocessor = reprocessor;
        }

        #endregion
    }
}