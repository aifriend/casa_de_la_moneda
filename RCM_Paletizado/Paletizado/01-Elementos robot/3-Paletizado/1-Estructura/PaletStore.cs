using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

namespace Idpsa
{
    public class PaletStore : IDiagnosisOwner, ISupplier, IContainer, IAutomaticRunnable, IManualsProvider
    {
        //Esquina superior derecha 

        private readonly CornerPoint3D _cornerPoint;
        private readonly Locations _location;
        private int _nPalets;
        private IPalet _palet;
        public double baseHeight;

        public PaletStore(string name, Locations location, CornerPoint3D cornerPoint, IEvaluable sensor)
        {
            Name = name;
            _cornerPoint = cornerPoint;
            _location = location;
            Sensor = sensor;
        }

        public string Name { get; private set; }


        public int NPalets
        {
            get { return _nPalets; }
            private set { _nPalets = value; }
        }

        public int NTotalPalets { get; private set; }

        public IPalet Palet
        {
            get { return _palet; }
            set { _palet = value; }
        }

        public Rectangle Rectangle { get; private set; }

        [Manual(SuperGroup = "General", Group = "Sensores")]
        public IEvaluable Sensor { get; private set; }

        #region IDiagnosisOwner Members

        public IEnumerable<SecurityDiagnosis> GetSecurityDiagnosis()
        {
            var list = new List<SecurityDiagnosis>();
            list.Add(new SecurityDiagnosisCondition("No se detectan palets en " + Name,
                                                    "compruebe detector de presencia en mesa de palets vacíos",
                                                    DiagnosisType.Event, () => !Sensor.Value() && HasPalet()));
            list.Add(new SecurityDiagnosisCondition("Reponga palets en " + Name,
                                                    "no hay palets en mesa de palets vacíos " + Name,
                                                    DiagnosisType.Event, () => !Sensor.Value() && !HasPalet()));
            return list;
        }

        #endregion

        public void SetNewCatalog(int nTotalPalets, IPalet palet)
        {
            Palet = palet;
            NTotalPalets = nTotalPalets;
        }


        public bool HasPalet()
        {
            return (_nPalets > 0) && Sensor.Value();
        }


        private Point3D CenterBasePosition()
        {
            return new Point3D(_cornerPoint.Z, new Rectangle(_cornerPoint.ToCornerPoint2D(), _palet.Base.Lados).Center);
        }

        public Point3D Position()
        {
            return CenterBasePosition().Desplazado(new Point3D(0, 0, (-_nPalets*_palet.Height) + 27));
        }

        public Point3D PositionNextPalet1() //MDG.2011-06-22
        {
            if (_nPalets >= 2)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 1)*_palet.Height) + 27));
            else
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-_nPalets*_palet.Height) + 27));
        }

        public Point3D PositionNextPalet2() //MDG.2011-06-22
        {
            if (_nPalets >= 3)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 2)*_palet.Height) + 27));
            else if (_nPalets >= 2)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 1)*_palet.Height) + 27));
            else
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-_nPalets*_palet.Height) + 27));
        }

        public Point3D PositionNextPalet3() //MDG.2011-06-22
        {
            if (_nPalets >= 4)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 3)*_palet.Height) + 27));
            else if (_nPalets >= 3)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 2)*_palet.Height) + 27));
            else if (_nPalets >= 2)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 1)*_palet.Height) + 27));
            else
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-_nPalets*_palet.Height) + 27));
        }

        public Point3D PositionNextPalet4() //MDG.2011-06-27
        {
            if (_nPalets >= 5)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 4)*_palet.Height) + 27));
            else if (_nPalets >= 4)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 3)*_palet.Height) + 27));
            else if (_nPalets >= 3)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 2)*_palet.Height) + 27));
            else if (_nPalets >= 2)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 1)*_palet.Height) + 27));
            else
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-_nPalets*_palet.Height) + 27));
        }

        public Point3D PositionNextPalet5() //MDG.2011-06-27
        {
            if (_nPalets >= 6)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 5)*_palet.Height) + 27));
            else if (_nPalets >= 5)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 4)*_palet.Height) + 27));
            else if (_nPalets >= 4)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 3)*_palet.Height) + 27));
            else if (_nPalets >= 3)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 2)*_palet.Height) + 27));
            else if (_nPalets >= 2)
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-(_nPalets - 1)*_palet.Height) + 27));
            else
                return CenterBasePosition().Desplazado(new Point3D(0, 0, (-_nPalets*_palet.Height) + 27));
        }

        //Se carga cuando con temporización en la cadena de automático
        //se detecta palet durante 10 s
        public void ElementsAdded()
        {
            NPalets = NTotalPalets;
        }

        #region Miembros de ISupplier

        public ElementTypes? ElementSupplied()
        {
            if (_palet == null)
                return null;
            else if (HasPalet())
                return ElementTypes.Palet;
            else
                return null;
        }

        public Locations Location
        {
            get { return _location; }
        }

        #endregion

        #region Miembros de IContainer

        public void ElementAdded(IElement item)
        {
            throw new NotImplementedException();
        }

        public IElement ElementQuitted()
        {
            if (_nPalets > 0)
                //_nPalets--;
                ; //MDG.2011-06-22.No descontamos el numero de palets para que siempre busque el de arriba del todo
            return _palet;
        }

        #endregion

        #region Nested type: CadAutoPaletStore

        private class CadAutoPaletStore : StructuredChain
        {
            private readonly PaletStore _paletStore;
            private TON _timer;

            public CadAutoPaletStore(string name, PaletStore paletStore, IdpsaSystemPaletizado sys)
                : base(name)
            {
                _paletStore = paletStore;
                AddSteps();
            }

            protected override void AddSteps()
            {
                AddMain_Steps();
            }

            private void AddMain_Steps()
            {
                MainChain.Add(new Step("Paso inicial").WithTag("first")).Task = () =>
                                                                                    {
                                                                                        ResetTimers();
                                                                                        NextStep();
                                                                                    };

                MainChain.Add(new Step("Chequear reposición de palets") {StopChain = true}).Task = () =>
                                                                                                       {
                                                                                                           if (
                                                                                                               PaletsAdded
                                                                                                                   ())
                                                                                                               NextStep();
                                                                                                       };

                MainChain.Add(new Step("Paso final")).Task = () => { GoToStep("first"); };
            }

            private bool PaletsAdded()
            {
                bool value = false;
                if (_paletStore.HasPalet() == false)
                {
                    if (_timer.TimingWithReset(5000, _paletStore.Sensor.Value()))
                    {
                        _paletStore.ElementsAdded();
                        value = true;
                    }
                }
                return value;
            }

            private void ResetTimers()
            {
                _timer = new TON();
            }
        }

        #endregion

        #region Miembros de IAutomaticRunnable

        IEnumerable<Chain> IAutomaticRunnable.GetAutoChains()
        {
            return new[] {new CadAutoPaletStore(Name, this, null)};
        }

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return (this).GetManualsRepresentations();
        }

        #endregion
    }
}