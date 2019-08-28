using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class BandaSalidaEnfajadora : IRi, IAutomaticRunnable, IItemSuplier<GrupoPasaportes>
    {
        [Manual(SuperGroup = "Encajado", Group = "Banda PRODEC")] 
        private readonly Actuator _motor;

        private readonly LinesSemaphore _semaphore;

        [Manual(SuperGroup = "Encajado", Group = "Banda PRODEC")] 
        private readonly IEvaluable _sensorEntradaAlemanaJaponesa1;
        private readonly IEvaluable _sensorEntradaAlemanaJaponesa2;

        [Manual(SuperGroup = "Encajado", Group = "Banda PRODEC")] 
        private readonly IEvaluable _sensorEntradaManualAlemana;

        [Manual(SuperGroup = "Encajado", Group = "Banda PRODEC")] 
        private readonly IEvaluable _sensorPresenciaSalida;

        private readonly IItemSolicitor<GrupoPasaportes> _solicitor;

        [Manual(SuperGroup = "Encajado", Group = "Banda PRODEC")] 
        private ICylinder _cilindro;

        [Manual(SuperGroup = "Encajado", Group = "Banda PRODEC")] 
        private IEvaluable _semaphoreRepresentation;

        [Manual(SuperGroup = "Encajado", Group = "Banda PRODEC")] 
        private IEvaluable _sensorReposoEmpujadorManualAlemana;

        public BandaSalidaEnfajadora(Actuator motor,
                                     IEvaluable sensorEntradaJaponesa,
                                     IEvaluable sensorEntradaAlemana,
                                     IEvaluable sensorEntradaManualAlemana,
                                     IEvaluable sensorEmpujadorEntradaManualAlemana,
                                     IEvaluable sensorSalida,
                                     LinesSemaphore semaphore,
                                     SystemControl control,
                                     IItemSolicitor<GrupoPasaportes> solicitor,
                                     SystemProductionPaletizado production)
        {
            _motor = motor;
            PassportGroup = new List<GrupoPasaportes>();
            _sensorEntradaAlemanaJaponesa1 = sensorEntradaJaponesa.WithManualRepresentation("entrada desde japonesa y alemana");
            _sensorEntradaAlemanaJaponesa2 = sensorEntradaAlemana;
            _sensorEntradaManualAlemana = sensorEntradaManualAlemana.WithManualRepresentation("entrada desde manual alemana");
            _sensorPresenciaSalida = sensorSalida.WithManualRepresentation("presencia salida");
            _sensorReposoEmpujadorManualAlemana = sensorEmpujadorEntradaManualAlemana.WithManualRepresentation("trabajo empujador manual alemana");
            _semaphore = semaphore;
            _solicitor = solicitor;

            SolicitorJaponesa = new SolicitorLine(this,
                                                  () => NumGrupos() == 0 &&
                                                        semaphore.HasPermission(IDLine.Japonesa) &&
                                                        control.InActiveMode(Mode.Automatic) &&
                                                        production.IsCatalogReady(IDLine.Japonesa));

            SolicitorAlemana = new SolicitorLine(this,
                                                 () => NumGrupos() == 0 &&
                                                       semaphore.HasPermission(IDLine.Alemana) && 
                                                       control.InActiveMode(Mode.Automatic) && 
                                                       production.IsCatalogReady(IDLine.Alemana));

            _semaphoreRepresentation = _semaphore.GetEvaluableRepresentation();
        }

        public List<GrupoPasaportes> PassportGroup { get; private set; }

        public IItemSolicitor<GrupoPasaportes> SolicitorAlemana { get; private set; }
        public IItemSolicitor<GrupoPasaportes> SolicitorJaponesa { get; private set; }

        public IEvaluable SensorEntradaAlemanaJaponesa1
        {
            get { return _sensorEntradaAlemanaJaponesa1; }
        }

        public IEvaluable SensorEntradaManualAlemana
        {
            get { return _sensorEntradaManualAlemana; }
        }

        public IEvaluable SensorPresenciaSalida
        {
            get { return _sensorPresenciaSalida; }
        }

        public bool GrupoEnSalida
        {
            get { return _grupoSacado.Value(); }
        }

        private IEvaluable _grupoSacado;

        #region IRi Members

        public void Ri()
        {
            _motor.Activate(false);
        }

        #endregion

        #region Miembros de IAutoRunnable

        public IEnumerable<Chain> GetAutoChains()
        {
            return new List<Chain> {new CadAutoBandaSalidaEnfajadora("Banda salida zona fajadora", this)};
        }

        #endregion

        public int NumGrupos()
        {
            return PassportGroup.Count;
        }

        private void GrupoAñadido(GrupoPasaportes grupo)
        {
            PassportGroup.Add(grupo);
            if (grupo != null)
                if (grupo.LastOfBox)
                    _semaphore.QuitRequest(grupo.IdLine);
        }

        private GrupoPasaportes GrupoRetirado()
        {
            GrupoPasaportes value = PassportGroup[0];
            PassportGroup.RemoveAt(0);
            return value;
        }

        public StoredDataBandaSalidaEnfajadoraGroups GetDataToStore()
        {
            return new StoredDataBandaSalidaEnfajadoraGroups //
                       {
                           Groups = PassportGroup
                       };
        }

        public void SetDataStored(StoredDataBandaSalidaEnfajadoraGroups grupoCargado)
        {
            PassportGroup = grupoCargado.Groups;
        }

        #region Miembros de IItemSuplier<GrupoPasaportes>

        public GrupoPasaportes QuitItem()
        {
            return GrupoRetirado();
        }

        #endregion

        #region Nested type: CadAutoBandaSalidaEnfajadora

        private class CadAutoBandaSalidaEnfajadora : StructuredChain
        {
            private readonly BandaSalidaEnfajadora _banda;

            public CadAutoBandaSalidaEnfajadora(string parName, BandaSalidaEnfajadora banda)
                : base(parName)
            {
                _banda = banda;
                AddSteps();
                InitializeLogicalSignals();
            }

            protected override void AddSteps()
            {
                var timer = new TON();

                MainChain.Add(new Step("Paso libre")).Task = () =>
                                                                 {
                                                                     timer.Reset();
                                                                     NextStep();
                                                                 };

                int nGrupos = 0;

                MainChain.Add(new Step("Tiempo de recorrido de grupo en cinta después de fajado excedido"))
                    .Task = () =>
                                {
                                    bool activate = _banda.NumGrupos() > 0 &&
                                                    !(_banda._grupoSacado.Value() && !_banda._solicitor.ReadyToPutElement);

                                    if (timer.TimingWithReset(15000, activate && nGrupos == _banda.NumGrupos()))
                                    {
                                        SetStepDiagnosis("Compruebe paquetes en cinta salida enfajadora");
                                        NextStep();
                                        return;
                                    }

                                    nGrupos = _banda.NumGrupos();
                                    _banda._motor.Activate(activate);
                                };

                MainChain.Add(new Step("Paso final")).Task = () =>
                                                                 {
                                                                     _banda._motor.Activate(false);
                                                                     PreviousStep();
                                                                 };
            }

            protected override void ChainSteps()
            {
                base.ChainSteps();
                _banda._grupoSacado.Value();
            }

            private void InitializeLogicalSignals()
            {
                _banda._grupoSacado = _banda.SensorPresenciaSalida
                    .AND(Evaluable.FromFunctor(() => _banda.NumGrupos() > 0)) //Tiene que haber grupo en la banda
                    //.DelayToConnection(100)
                    .DelayToDisconnection(500) //Tiempo que tarda un grupo en pasar por delante del detector
                    .Subscribe(v =>
                                   {
                                       if (!v) 
                                           _banda._solicitor.PutElement(_banda);
                                   });
            }
        }

        #endregion

        #region Nested type: SolicitorLine

        private class SolicitorLine : IItemSolicitor<GrupoPasaportes>
        {
            private readonly BandaSalidaEnfajadora _banda;
            private readonly Func<bool> _lineSelector;

            public SolicitorLine(BandaSalidaEnfajadora banda, Func<bool> lineSelector)
            {
                _banda = banda;
                _lineSelector = lineSelector;
            }

            #region IItemSolicitor<GrupoPasaportes> Members

            public GrupoPasaportes PutElement(IItemSuplier<GrupoPasaportes> suplier)
            {
                var item = suplier.QuitItem();
                _banda.GrupoAñadido(item);
                return item;
            }

            public bool ReadyToPutElement
            {
                get { return _lineSelector(); }
            }

            #endregion
        }

        #endregion
    }
}