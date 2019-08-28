using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class Elevador2 : IAutomaticRunnable2, IItemSolicitor<GrupoPasaportes>, IItemSuplier<GrupoPasaportes>, IRi, IRi2
    {
        #region States enum

        public enum States
        {
            Subir,
            Bajar,
            LLenar,
            Vaciar
        }

        #endregion

        [Manual(SuperGroup = "Transporte", Group = "Elevador 2")] private readonly ActuatorTwoPositionsMovement
            _elevador;

        [Manual(SuperGroup = "Transporte", Group = "Elevador 2")] private readonly ICylinder _empujador;

        //public bool ModoAcumulacion;

        private readonly List<GrupoPasaportes> _gruposAñadidos;

        private readonly LinesSemaphore _semaphore;
        private readonly IItemSolicitor<GrupoPasaportes> _solicitor;
        [Manual(SuperGroup = "Transporte", Group = "Elevador 2")] private Sensor _confirmacion;
        public States _state; //MDG.2011-05-18.Puesto public

        public Elevador2(ActuatorTwoPositionsMovement elevador,
                         ICylinder empujador,
                         IItemSolicitor<GrupoPasaportes> solicitor,
                         LinesSemaphore semaphore)
        {
            _elevador = elevador;
            _empujador = empujador;
            _solicitor = solicitor;
            _state = States.Subir;
            _gruposAñadidos = new List<GrupoPasaportes>();
            _semaphore = semaphore;
        }

        public GrupoPasaportes GrupoPasaportes { get; private set; }


        private bool MoveUP()
        {
            return _elevador.Move1();
        }

        private bool MoveDown()
        {
            return _elevador.Move2();
        }

        //MDG.2010-12-02.Metodos de salvado y carga
        public StoredDataElevator2Group GetDataToStore()
        {
            return new StoredDataElevator2Group //
                       {
                           Group = GrupoPasaportes,
                           //_production.GetBoxes().Select(b => b.Id).ToList(),
                           State = _state
                       };
        }

        public void SetDataStored(StoredDataElevator2Group GrupoCargado)
        {
            GrupoPasaportes = GrupoCargado.Group;
            _state = GrupoCargado.State;
        }

        #region Nested type: AutoChainElevador2

        private class AutoChainElevador2 : StructuredChain
        {
            private readonly Elevador2 _elevador;
            private TON _timer;

            public AutoChainElevador2(string name, Elevador2 elevador)
                : base(name)
            {
                _elevador = elevador;
                AddSteps();
            }

            protected override void AddSteps()
            {
                Main_Steps();
                Subir_Steps();
                Bajar_Steps();
                Vaciar_Steps();
                LLenar_Steps();
            }

            private void ResetTimers()
            {
                _timer = new TON();
            }

            private void Main_Steps()
            {
                MainChain.Add(new Step("Paso inicial")).Task = () => { NextStep(); };

                MainChain.Add(new Step("Elegir cadena a ejecutar") {StopChain = true})
                    .Task = () =>
                                {
                                    ResetTimers();
                                    int kkk = _elevador._gruposAñadidos.Count;
                                    CallChain(_elevador._state);
                                };

                MainChain.Add(new Step("Paso final")).Task = () => { PreviousStep(); };
            }

            private void Subir_Steps()
            {
                Subchain chain = AddSubchain(States.Subir);

                chain.Add(new Step("Comprobar empujador elevador 2 en reposo", 5000))
                    .Task = () =>
                                {
                                    _elevador._empujador.Rest();
                                    if (_timer.TimingWithReset(1000, _elevador._empujador.InRest))
                                        NextStep();
                                };

                chain.Add(new Step("Subir elevador 2")).Task = () =>
                                                                   {
                                                                       if (_timer.TimingWithReset(400,
                                                                                                  _elevador.MoveUP()))
                                                                           NextStep();
                                                                   };

                chain.Add(new Step("Paso final")).Task = () =>
                                                             {
                                                                 _elevador._state = States.LLenar;
                                                                 Return();
                                                             };
            }

            private void Bajar_Steps()
            {
                Subchain chain = AddSubchain(States.Bajar);

                chain.Add(new Step("Comprobar empujador elevador 2 en reposo", 5000))
                    .Task = () =>
                                {
                                    _elevador._empujador.Rest();
                                    if (_timer.TimingWithReset(1000, _elevador._empujador.InRest))
                                        NextStep();
                                };


                chain.Add(new Step("Bajar elevador 2")).Task = () =>
                                                                   {
                                                                       if (_timer.TimingWithReset(400,
                                                                                                  _elevador.MoveDown()))
                                                                           NextStep();
                                                                   };

                chain.Add(new Step("Paso final")).Task = () =>
                                                             {
                                                                 _elevador._state = States.Vaciar;
                                                                 Return();
                                                             };
            }

            private void LLenar_Steps()
            {
                Subchain chain = AddSubchain(States.LLenar);

                chain.Add(new Step("Comprobar empujador elevador 2 en reposo", 5000))
                    .Task = () =>
                                {
                                    _elevador._empujador.Rest();
                                    if (_timer.TimingWithReset(1000, _elevador._empujador.InRest))
                                        NextStep();
                                };

                chain.Add(new Step("Esperar cambio de estado") {StopChain = true})
                    .Task = () =>
                                {
                                    if (_elevador._state != States.LLenar)
                                        Return();
                                };
            }

            private void Vaciar_Steps()
            {
                Subchain chain = AddSubchain(States.Vaciar);

                chain.Add(new Step("Comprobar empujador elevador 2 en reposo", 5000))
                    .Task = () =>
                                {
                                    _elevador._empujador.Rest();
                                    if (_timer.TimingWithReset(1000, _elevador._empujador.InRest))
                                    {
                                        _elevador._semaphore.Request(IDLine.Alemana);
                                        NextStep();
                                    }
                                };

                chain.Add(new Step("Esperar permiso semáforo") {StopChain = true})
                    .Task = () =>
                                {
                                    if (_elevador._semaphore.HasPermission(IDLine.Alemana))
                                    {
                                        NextStep();
                                    }
                                };

                chain.Add(new Step("Esperar posición libre en transportador") {StopChain = true})
                    .Task = () =>
                                {
                                    if (_elevador._solicitor.ReadyToPutElement)
                                    {
                                        NextStep();
                                    }
                                };

                chain.Add(new Step("Comprobar empujador elevador 2 en trabajo", 5000))
                    .Task = () =>
                                {
                                    _elevador._empujador.Work();
                                    if (_timer.TimingWithReset(500, _elevador._empujador.InWork))
                                    {
                                        GrupoPasaportes box = _elevador._solicitor.PutElement(_elevador);
                                        if (box.LastOfBox)
                                        {
                                            _elevador._semaphore.QuitRequest(IDLine.Alemana);
                                        }
                                        _elevador._state = States.Subir;
                                        Return();
                                    }
                                };
            }
        }

        #endregion

        #region Miembros de IAutomaticRunnable2

        IEnumerable<Chain> IAutomaticRunnable2.GetAutoChains2()
        {
            return new[] {new AutoChainElevador2("Elevador 2", this)};
        }

        #endregion

        #region Miembros de IItemSolicitor<GrupoPasaportes>

        public GrupoPasaportes PutElement(IItemSuplier<GrupoPasaportes> suplier)
        {
            GrupoPasaportes = suplier.QuitItem();
            _state = States.Bajar;
            return GrupoPasaportes;
        }

        public bool ReadyToPutElement
        {
            get { return _state == States.LLenar; }
        }

        #endregion

        #region Miembros de IItemSuplier<GrupoPasaportes>

        public GrupoPasaportes QuitItem()
        {
            GrupoPasaportes value = GrupoPasaportes;
            GrupoPasaportes = null;
            return value;
        }

        #endregion

        #region Miembros de IRi

        public void Ri()
        {
            ;// _elevador.Deactivate();
        }

        public void Ri2()
        {
            _elevador.Deactivate();
        }

        #endregion
    }
}