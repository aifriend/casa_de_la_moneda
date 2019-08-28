using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Sequence;
using Idpsa.Control.Manuals;
using Idpsa.Control.Engine;

namespace Idpsa.Paletizado
{
    [System.Serializable()]
    public class BandaSalidaEnfajadora : IRi, IAutomaticRunnable, IDiagnosisOwner,IItemSuplier<GrupoPasaportes>
    {       
        public List<GrupoPasaportes> _gruposPasaportes;//MDG.2010-12-03.Puesto public
        [Manual(SuperGroup = "Encajado",Group="Entrada prodec")]
        private IEvaluable _sensorEntradaJaponesa;
        [Manual(SuperGroup = "Encajado", Group = "Entrada prodec")]
        private IEvaluable _sendorEntradaAlemana;
        [Manual(SuperGroup = "Encajado", Group = "Entrada prodec")]
        private IEvaluable _sensorSalida;
        [Manual(SuperGroup = "Encajado", Group = "Entrada prodec")]
        private ICylinder _cilindro;
        [Manual(SuperGroup = "Encajado", Group = "Entrada prodec")]
        private IEvaluable _semaphoreRepresentation;
        [Manual(SuperGroup = "Encajado", Group = "Entrada prodec")]
        private Actuator _motor;

        private bool _entradaLibre;
        private bool _grupoEnSalida;       


        private IItemSolicitor<GrupoPasaportes> _solicitor;
        private LinesSemaphore _semaphore;

        public IItemSolicitor<GrupoPasaportes> SolicitorAlemana { get; private set; }
        public IItemSolicitor<GrupoPasaportes> SolicitorJaponesa { get; private set; }

        public bool ModoAcumulacion;

        public Actuator Motor
        {
            get { return _motor; }
        }
        public IEvaluable SensorEntradaJaponesa
        {
            get { return _sensorEntradaJaponesa; }
        }

        public IEvaluable SensorEntradaAlemana
        {
            get { return _sensorEntradaJaponesa; }
        }

        public IEvaluable SensorSalida
        {
            get { return _sensorSalida; }
        }
        public ICylinder Cilindro
        {
            get
            {
                return _cilindro;
            }
        }

        public BandaSalidaEnfajadora(Actuator motor, IEvaluable sensorEntradaJaponesa,IEvaluable sensorEntradaAlemana,
                                     IEvaluable sensorSalida, LinesSemaphore semaphore, SystemControl control, IItemSolicitor<GrupoPasaportes> solicitor, SystemProductionPaletizado production)
        {
            _motor = motor;
            _gruposPasaportes = new List<GrupoPasaportes>();
            _sensorEntradaJaponesa = sensorEntradaJaponesa.WithManualRepresentation("entrada desde japonesa");
            _sendorEntradaAlemana = sensorEntradaAlemana.WithManualRepresentation("entrada desde alemana");
            _sensorSalida = sensorSalida.WithManualRepresentation("presencia salida");
            _semaphore = semaphore;
            _solicitor = solicitor;

            //Func<bool> freeEntry = () => (NºGrupos() < 2 && _entradaLibre); 
            Func<bool> freeEntry = () => (NºGrupos() < 1 && _entradaLibre);//MDG.2011-06-30 
                        
            SolicitorJaponesa = new SolicitorLine(this,
                () =>
                {
                    return
                        semaphore.HasPermission(IDLine.Japonesa) &&
                        freeEntry() &&
                        control.InActiveMode(Mode.Automatic) &&
                        production.IsCatalogReady(IDLine.Japonesa);
                }
            );

            SolicitorAlemana = new SolicitorLine(this,
                () => 
                {
                    return
                        semaphore.HasPermission(IDLine.Alemana)
                        && freeEntry()
                        && control.InActiveMode(Mode.Automatic)
                        && production.IsCatalogReady(IDLine.Alemana);
                }
            );

            _semaphoreRepresentation = _semaphore.GetEvaluableRepresentation();
        }        

        public int NºGrupos()
        {
            return _gruposPasaportes.Count;
        }  
        
        public void GrupoAñadido(GrupoPasaportes Grupo)
        {
            _gruposPasaportes.Add(Grupo);
            //_entradaLibre = false;//MDG.2010-11-29.Quito forzado de anulacion de permiso de entrada de cajas en la banda de la enfajadora. El permiso se evalua en la propia funcion

            //MDG.2011-06-22.Para asegurar quitar peticion de japonesa
            //try
            //{
                if (Grupo != null)
                    if (Grupo.LastOfBox)
                        _semaphore.QuitRequest(Grupo.IdLine);
            //}
            //catch (Exception ex) { ;}
        }

        public GrupoPasaportes GrupoRetirado()
        {
            GrupoPasaportes value = _gruposPasaportes[0];
            _gruposPasaportes.RemoveAt(0);
            return value;
        }

        public bool EntradaLibre
        {
            get { return _entradaLibre; }
            set { _entradaLibre = value; }
        }

        public bool GrupoEnSalida
        {
            get { return _grupoEnSalida; }
            set { _grupoEnSalida = value; }
        }

        public void Ri()
        {
            _motor.Activate(false);
        }

        #region Miembros de IAutoRunnable

        public IEnumerable<Chain> GetAutoChains()
        {
            return new List<Chain>() { new CadAutoBandaSalidaEnfajadora("Banda salida zona fajadora", this) };
        }

        #endregion

        #region Miembros de ISecurityDiagnosisOwner

        IEnumerable<SecurityDiagnosis> IDiagnosisOwner.GetSecurityDiagnosis()
        {
            return new SecurityDiagnosis[]
           {               
                //TODO new SecurityDiagnosisSignal(_sys.Bus.In("Q732"), TypeDiagnosisSignal.Failure)
           };
        }

        #endregion
        
        private class CadAutoBandaSalidaEnfajadora : StructuredChain
        {            
            private BandaSalidaEnfajadora _banda;

            private bool grupoDetectadoEnSalida;
           
            IEvaluable grupoIntroducido;
            IEvaluable grupoSacado;

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

                var entradaLibre = _banda.SensorEntradaJaponesa
                                   .OR(_banda.SensorEntradaAlemana)
                                   .NOT();

                MainChain.Add(new Step("Paso libre")).Task = () =>
                {
                    _banda.EntradaLibre = entradaLibre.Value();
                    timer.Reset();
                    NextStep();
                };

                int nGrupos = 0;

                MainChain.Add(new Step("Tiempo de recorrido de grupo en cinta después de fajado excedido")).Task = () =>
                {
                    bool activate = _banda.NºGrupos() > 0 &&
                        !(grupoDetectadoEnSalida && !_banda._solicitor.ReadyToPutElement);
                                        
                    if (timer.TimingWithReset(15000, activate && nGrupos == _banda.NºGrupos()))
                    {
                        SetStepDiagnosis("Compruebe paquetes en cinta salida enfajadora");
                        
                        NextStep();
                        return;
                    }

                    nGrupos = _banda.NºGrupos();                    
                    _banda.Motor.Activate( activate);                   
                };

                MainChain.Add(new Step("Paso final")).Task = () =>
                {
                    _banda.Motor.Activate(false);
                    PreviousStep();                    
                };
            }

            protected override void ChainSteps()
            {
                base.ChainSteps();
                GestorEntradaSalidaPasaportes();
            }

            private void InitializeLogicalSignals()
            {
                grupoIntroducido =
                                 _banda.SensorEntradaJaponesa
                                 .AND(Evaluable.FromFunctor(() => _banda._semaphore.HasPermission(IDLine.Japonesa)))
                                 .OR(
                                    _banda.SensorEntradaAlemana
                                    .AND(Evaluable.FromFunctor(() => _banda._semaphore.HasPermission(IDLine.Alemana)))
                                  )
                                 .AND(Evaluable.FromFunctor(() => _banda.NºGrupos() > 0))
                                 .DelayToConnection(100)
                                 .DelayToDisconnection(500)
                                 .Subscribe((v) => _banda.EntradaLibre = !v);                

                grupoSacado = _banda.SensorSalida
                                    .AND(Evaluable.FromFunctor(() => _banda.NºGrupos() > 0))
                                    .DelayToConnection(100)
                                    .DelayToDisconnection(500)
                                    .Subscribe((v) =>
                                    {
                                        grupoDetectadoEnSalida = v;
                                        if (!v)
                                            _banda._solicitor.PutElement(_banda);
                                    });
            }

            private void ResetLogicalSignals()
            {
                grupoIntroducido.Reset();
                grupoSacado.Reset();
            }

            private void GestorEntradaSalidaPasaportes()
            {
                grupoIntroducido.Value();             
                grupoSacado.Value();
            }
        }

        private class SolicitorLine : IItemSolicitor<GrupoPasaportes>
        {
            private BandaSalidaEnfajadora _banda;
            private Func<bool> _lineSelector;

            public SolicitorLine(BandaSalidaEnfajadora banda, Func<bool> lineSelector)
            {
                _banda = banda;
                _lineSelector = lineSelector; 
            }

            public GrupoPasaportes PutElement(IItemSuplier<GrupoPasaportes> suplier)
            {
                var item = suplier.QuitItem();
                _banda.GrupoAñadido(item);
                return item;
            }

            public bool ReadyToPutElement
            {
                get { return _banda.EntradaLibre && _lineSelector(); }
            }          
        }

        //MDG.2010-12-02.Metodos de salvado y carga de los grupos de pasaportes almacenados en los transportadores aereos
        public StoredDataBandaSalidaEnfajadoraGroups GetDataToStore()
        {
            return new StoredDataBandaSalidaEnfajadoraGroups()//
            {
                Groups =_gruposPasaportes//,//_grupos,//Group = GrupoPasaportes,//_production.GetBoxes().Select(b => b.Id).ToList(),
                //State = _state,
                //Name = this.Name//,
                //Vaciar = this.Vaciar,//MDG.2010-12-09
                //ModoAcumulacion = this.ModoAcumulacion//MDG.2010-12-09
            };
        }

        public void SetDataStored(StoredDataBandaSalidaEnfajadoraGroups GrupoCargado)
        {
            _gruposPasaportes = GrupoCargado.Groups;
            //_state = GrupoCargado.State;
            //Vaciar = GrupoCargado.Vaciar;//MDG.2010-12-09
            //ModoAcumulacion = GrupoCargado.ModoAcumulacion;//MDG.2010-12-09
        }


        #region Miembros de IItemSuplier<GrupoPasaportes>

        public GrupoPasaportes QuitItem()
        {            
            return GrupoRetirado(); 
        }

        #endregion
    }
}