using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Engine;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;
using Idpsa.Paletizado.Definitions;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public class Line2 : Line, IAutomaticRunnable, IDiagnosisOwner
    {
        private const Paletizer.States PaletizerFirstState = Paletizado.Paletizer.States.PutPalet;
                                       //.PutItem;//.QuitPaletizer;

        private readonly Bus _bus;

        [Manual(SuperGroup = "General", Group = "Sensores")] private readonly IEvaluable _presencia;

        private readonly IdpsaSystemPaletizado _sys;
        public bool ModoAcumulacion_T1;
        public bool ModoAcumulacion_T2;

        //MDG.2010-07-12.Estado inicial, copiado de linea 1
        //private Paletizer.States _paletizerFirstState = Idpsa.Paletizado.Paletizer.States.PutItem;
        //MDG.2010-12-01.Rehabilitamos insercion inicial de palet
        //private Paletizer.States _paletizerFirstState = Idpsa.Paletizado.Paletizer.States.PutPalet;

        public Line2(IdpsaSystemPaletizado sys,
                     SourceGroupSupplier supplier,
                     IItemSolicitor<GrupoPasaportes> solicitor,
                     LinesSemaphore semaphore)
            : base(IDLine.Alemana)
        {
            _sys = sys;
            _bus = _sys.Bus;
            _presencia = _bus.In("Input4.6").WithManualRepresentation("presencia paletizado alemana");
            Supplier = supplier;

            //var settings = new PaletizerSettings(Idpsa.Paletizado.Paletizer.Action.Paletize, Idpsa.Paletizado.Paletizer.States.PutPalet)
            //MDG.2010-07-09.El estado inicial no es dejar palet nuevo. Se comprueba si hay que dejar palet en cadena de eleccion de tarea
            //El nuevo estado inicial del paletizado es dejar cajas.En la cadena de deposicionde cajas se va a comprobar si hay palet donde dejarlas
            //var settings = new PaletizerSettings(Idpsa.Paletizado.Paletizer.Action.Paletize, Idpsa.Paletizado.Paletizer.States.PutItem)
            //MDG.2010-07-12.El estado inicial Sí es dejar palet nuevo. Se comprueba si hay palet con condicion origen = not(Hay palet && orden depositar palet (.PutPalet)
            var settings = new PaletizerSettings(Paletizado.Paletizer.Action.Paletize,
                                                 Paletizado.Paletizer.States.PutPalet)
                               {
                                   InitialState = PaletizerFirstState,
                                   //MDG.2010-07-12.Estado inicial, copiado de linea 1
                                   MinFlatsToDo = 0,
                                   HasSeparator = false,
                                   DownSeparator = false
                               };

            //CornerPoint3D cornerPoint = Geometria.PosZonaPaletizadoFinal2.ToConerPoint3D();
            CornerPoint3D cornerPoint = sys.Geo.PosZonaPaletizadoFinal2.ToConerPoint3D();//MCR

            var suppliers = new Dictionary<ElementTypes, IEnumerable<Locations>>
                                {
                                    {
                                        ElementTypes.ItemAlemana,
                                        new List<Locations> {Locations.Reproceso, Locations.Entrada}
                                        }
                                    ,
                                    {
                                        ElementTypes.Palet,
                                        new List<Locations> {Locations.PaletAlemana}
                                        }
                                };


            Paletizer = new NormalPaletizer("paletizado línea alemana", cornerPoint, Locations.PaletizadoAlemana,
                                            settings, (id) => Supplier.GetBox(id), suppliers, IDLine.Alemana)
                //.WithRequestElementAllowed(e=>(e==ElementTypes.Palet)?!bus.In("B819").Value():true);            
                .WithRequestElementAllowed(e => (e == ElementTypes.Palet) ? !_bus.In("Input4.6").Value() : true);
            //MDG.2010-12-01.cambio de fotocelula

            AddSolicitor(Paletizer);

            Func<bool> inAutomatic =
                () => ControlLoop<IdpsaSystemPaletizado>.Instance.Sys.Control.InActiveMode(Mode.Automatic);

            PaletStore = new PaletStore("pila de palets línea alemana", Locations.PaletAlemana,
                                        sys.Geo.PosPaletStore2.ToConerPoint3D(),
                                        _bus.In("B819").OR(
                                            Evaluable.FromFunctor(
                                                () => State != SubsystemState.Activated || !inAutomatic())).
                                            WithManualRepresentation("presencia almacén palets alemana"));

            AddSupplier(PaletStore);


            TransporteLinea = new TransporteLinea2(_sys, Supplier, solicitor, semaphore);
        }

        [Subsystem(Filter = SubsystemFilter.None)]
        public TransporteLinea2 TransporteLinea { get; private set; }

        [Manual(SuperGroup = "Estado", Group = "Socitors/Supliers")]
        public NormalPaletizer Paletizer { get; private set; }

        public SourceGroupSupplier Supplier { get; private set; }

        #region IAutomaticRunnable Members

        IEnumerable<Chain> IAutomaticRunnable.GetAutoChains()
        {
            return new[] {new CadAutoZonaPaletizadoLinea2(Paletizer.Name, this)};
        }

        #endregion

        public override bool SetNewCatalog(DatosCatalogoPaletizado datosCatalogo) //load data taken from file
        {
            PaletStore.SetNewCatalog(ProveCatalog.NInicialPalets, datosCatalogo.PaletizerDefinition.Palet);
            if (datosCatalogo.SotoredData != null)
            {
                //MDG.2010-12-13. Para evitar que dé error al crearse de nuevo el catalogo
                Supplier.CurrentIndex = datosCatalogo.SotoredData.GetCurrectGroupIndex();
                //MDG.2010-12-09.Lo cargo antes que los datos del palet para que no me lo sobre escriba con el sabe interno que hace
            }
            Paletizer.StartPaletizer(datosCatalogo);
            //_supplier.CurrentIndex = datosCatalogo.SotoredData.GetCurrectGroupIndex();//MDG.2010-12-09
            //datosCatalogo.SotoredData.GetCurrentIndex;
            _sys.Production.SaveCatalogs();

            return true;
        }


        public override StoreStateCatalog GetDataToStore() //get to save to file
        {
            StoredDataPaletizado palet = Paletizer.GetDataToStore();
            palet.CurrentGroupIndex = Supplier.CurrentIndex;
            //MDG.2010-12-09. Cogemos tambien el indice del ultimo grupo
            return new StoredDataCatalogoLinea2(palet);
        }

        public override string ToString()
        {
            return "Máquina numerado alemana";
        }

        //IEnumerable<Chain> IBackToOriginRunnable.GetBackToOriginChains()
        //{
        //    return new[] { new CadVOrigenVaciadoTramosLinea2(Paletizer.Name, this) };
        //}

        #region Nested type: CadAutoZonaPaletizadoLinea2

        private class CadAutoZonaPaletizadoLinea2 : StructuredChain
        {
            private const int _timeQuitPalet = 10000;
            private readonly NormalPaletizer _paletizer;
            private readonly IEvaluable _presencia;
            private readonly SourceGroupSupplier _supplier;
            private TON _timer;

            public CadAutoZonaPaletizadoLinea2(string name, Line2 line2)
                : base(name)
            {
                _paletizer = line2.Paletizer;
                _presencia = line2._presencia;
                _supplier = line2.Supplier;
                AddSteps();
            }

            protected override void AddSteps()
            {
                Main_Steps();
            }

            private void Main_Steps()
            {
                MainChain.Add(new Step("Paso inicial")).Task = () =>
                                                                   {
                                                                       ResetTimers();
                                                                       NextStep();
                                                                   };

                MainChain.Add(new Step("Comprobar paletizado retirado") {StopChain = true})
                    .Task = () =>
                                {
                                    if (_timer.TimingWithReset(_timeQuitPalet,
                                                               !_presencia.Value() &&
                                                               _paletizer.State ==
                                                               Paletizado.Paletizer.States.QuitPaletizer))
                                        //if (_timer.TimingWithReset(_timeQuitPalet,
                                        //    true && _paletizer.State == States.QuitPaletizer))
                                    {
                                        if (_supplier.IsCatalogReady())
                                        {
                                            _paletizer.StartPaletizer(_supplier.GetCatalog());
                                            PreviousStep();
                                        }
                                    }
                                };
            }

            private void ResetTimers()
            {
                _timer = new TON();
            }
        }

        #endregion

        #region Miembros de IDiagnosisOwner

        IEnumerable<SecurityDiagnosis> IDiagnosisOwner.GetSecurityDiagnosis()
        {
            IEvaluable diagnosisSignal =
                Evaluable.FromFunctor(
                    () =>
                    Paletizer.State == Paletizado.Paletizer.States.PutPalet && _presencia.Value())
                    .DelayToConnection(6000)
                    .DelayToDisconnection(6000);

            IEvaluable diagnosisSignal2 =
                Evaluable.FromFunctor(
                    () =>
                    _sys.Control.ConnectionCommand2 && !_bus.In("Q741").Value());
                    

            return new[]
                       {
                           new SecurityDiagnosisCondition
                               ("Elemento detectado en zona de paletizado final alemana",
                                "Quite elemento de zona de paletizado",
                                DiagnosisType.Step,
                                diagnosisSignal.Value),
                            new SecurityDiagnosisCondition
                               ("Térmico Q741 ascensor 1 caído o fallo variador CFTE4",
                                "Levante el interruptor Q741 y conecte el Mando de los Transportes",
                                DiagnosisType.Security,
                                diagnosisSignal2.Value)
                       };
        }

        #endregion

        public void ForceAddBox(int i)
        {
            if (i == 4)
            {
                if (Paletizer.UnderLyingPaletizer.Count < Paletizer.UnderLyingPaletizer.TotalToDoItems())
                {
                    _sys.Lines.SupplierLine2.GetNewIndexForced();
                    int ind = _sys.Lines.SupplierLine2.CurrentIndex;
                    GrupoPasaportes auxP = _sys.Lines.SupplierLine2.GetItem(ind);
                    if (auxP != null && auxP.TipoPasaporte != null)
                    {
                        CajaPasaportes box = new CajaPasaportes(auxP);
                        for (int c = 0; c < CajaPasaportes.NGrupos; c++)
                        {
                            ind = _sys.Lines.SupplierLine2.CurrentIndex--;
                            box.Add(_sys.Lines.SupplierLine2.GetItem(ind), CajaPasaportes.NGrupos - c - 1);
                        }
                        box = _sys.Production.CatalogManagerAlemana.GetBox(box.Id);
                        Paletizer.State = Paletizado.Paletizer.States.PutItem;

                            Paletizer.ForceElementAdded(box, i);
                            Paletizer.OnChanged();


                        _sys.Production.SaveCatalogs();
                    }
                }
            }

            _sys.Production.SaveCatalogs();
        }
        public void ForceQuitBox(int i)
        {
            if (i == 4)
            {
                _sys.Lines.SupplierLine2.GetNewIndexForced();
                //int ind = _sys.Lines.supplierLine1.CurrentIndex;
                //CajaPasaportes box = new CajaPasaportes(_sys.Lines.supplierLine1.GetItem(ind + 1));
                //for (int c = 0; c < CajaPasaportes.NGrupos; c++)
                //    box.Add(_sys.Lines.supplierLine1.GetItem(ind + 1 + c), CajaPasaportes.NGrupos - c - 1);
                Paletizer.State = Paletizado.Paletizer.States.QuitItem;
                Paletizer.ForceElementQuitted(i);
                Paletizer.OnChanged();
                Paletizer.State = Paletizado.Paletizer.States.PutItem;
                _sys.Production.SaveCatalogs();
            }

        }
    }
}