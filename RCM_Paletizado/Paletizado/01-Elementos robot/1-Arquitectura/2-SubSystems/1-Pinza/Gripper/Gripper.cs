using System.Collections.Generic;
using System.Linq;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Manuals;
using Idpsa.Control.Sequence;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;

namespace Idpsa.Paletizado
{
    public partial class Gripper : ISpecialDeviceOwner, IAutomaticRunnable, IChainControllersOwner, IDiagnosisOwner, IRi
    {
        public const int NegativeZLimit = Geometria.ZTechoPortico;

        [Manual(SuperGroup = "Pinza", Group = "Neumática")] private readonly GyreActuatorSommerSFM _actuadorGiro;
        [Manual(SuperGroup = "Pinza", Group = "Neumática", Description = "Brazos")] private readonly ICylinder _arms;

        [Manual(SuperGroup = "Pinza", Group = "Neumática", Description = "Plano aspirante")] private readonly
            Control.Component.Sucker _aspirator;

        private readonly ChainController _chainController;

        [Manual(SuperGroup = "Pinza", Group = "Neumática", Description = "Extensor")] private readonly ICylinder
            _extensor;

        [Manual(SuperGroup = "Pinza", Group = "Dispositivos")] private readonly Gantry3_C3I20T11_ChainController _gantry;
        private readonly IdpsaSystemPaletizado _sys;

        [Manual(SuperGroup = "Pinza", Group = "Dispositivos", Description = "Bascula")] private readonly ScaleHBM _tilts;
        private IElement _element;

        public Gripper(Gantry3_C3I20T11_ChainController gantry, GyreActuatorSommerSFM actuadorGiro,
                       Control.Component.Sucker aspirator,
                       ICylinder extensor, ICylinder arms, ScaleHBM tilts, IdpsaSystemPaletizado sys)
        {
            _gantry = gantry;
            _actuadorGiro = actuadorGiro;
            _aspirator = aspirator;
            _extensor = extensor;
            _arms = arms;
            _tilts = tilts;
            _sys = sys;
            _chainController = new ChainController();
            InitializeControllerChains();
            //V = new Gradient() { Hight = 600, Middle = 400, Low = 100 };
            //A = new Gradient() { Hight = 600, Middle = 400, Low = 100 };
            //V = new Gradient() { Hight = 1500, Middle = 400, Low = 100 };//MDG.2011-05-31.Aumento velocidad alta
            V = new Gradient {Hight = 1500, Middle = 800, Low = 100}; //MDG.2011-06-02.Aumento velocidad media
            A = new Gradient {Hight = 1500, Middle = 400, Low = 100}; //MDG.2011-05-31.Aumento aceleracion alta
        }

        #region Giro

        public void GirarChain(Spin spin)
        {
            _actuadorGiro.GyrateChain(spin);
        }

        public bool EnGiro(Spin spin)
        {
            return _actuadorGiro.InGyre(spin);
        }

        #endregion

        #region Aspiracion

        public bool EnVacioOff
        {
            get { return true; /*!_aspirator.EnVacio();*/ }
        }

        public bool EnVacioOn
        {
            get { return true; /*_aspirator.EnVacio();*/ }
        }

        public void VacioOff()
        {
            _aspirator.VaccumOff();
        }

        public void VacioOn()
        {
            _aspirator.VaccumOn();
        }

        public void Vacio(Sucker sucker)
        {
            if (sucker == Sucker.On)
                _aspirator.VaccumOn();
            else if (sucker == Sucker.Off)
                _aspirator.VaccumOff();
        }

        public bool EnVacio(Sucker sucker)
        {
            if (sucker == Sucker.On)
                return EnVacioOn;
            else if (sucker == Sucker.Off)
                return EnVacioOff;
            else
                return true;
        }

        #endregion

        #region Extensor

        public bool Extendida
        {
            get { return _extensor.InWork; }
        }

        public bool Recogida
        {
            get { return _extensor.InRest; }
        }

        public void Extender()
        {
            _extensor.Work();
        }

        public void Recoger()
        {
            _extensor.Rest();
        }

        public void Extender(Extensor extensor)
        {
            if (extensor == Extensor.Extend)
                Extender();
            else if (extensor == Extensor.Retract)
                Recoger();
            else if (extensor == Extensor.Dead)
                _extensor.Dead();
        }

        public bool IsExtended(Extensor extensor)
        {
            if (extensor == Extensor.Extend)
                return Extendida;
            else if (extensor == Extensor.Retract)
                return Recogida;
            else
                return true;
        }

        #endregion

        #region Brazos

        public bool Abierta
        {
            get { return _arms.InRest; }
        }

        public bool Cerrada
        {
            get { return _arms.InWork; }
        }

        public void Abrir()
        {
            _arms.Rest();
        }

        public void Cerrar()
        {
            _arms.Work();
        }

        public void Abrir(Arms arms)
        {
            if (arms == Arms.Open)
                Abrir();
            else if (arms == Arms.Close)
                Cerrar();
        }

        public bool Open(Arms arms)
        {
            if (arms == Arms.Open)
                return Abierta;
            else if (arms == Arms.Close)
                return Cerrada;
            else
                return true;
        }

        #endregion

        #region Ejes

        public CompaxC3I20T11 X
        {
            get { return _gantry.X; }
        }

        public CompaxC3I20T11 Y
        {
            get { return _gantry.Y; }
        }

        public CompaxC3I20T11 Z
        {
            get { return _gantry.Z; }
        }

        #endregion

        #region Gantry

        public bool StartMov(Point3D p, int v, int a)
        {
            return _gantry.StartMov(p, v, a);
        }

        public bool EndMov()
        {
            return _gantry.EndMov();
        }

        public void MoveChain(Point3D p, int v, int a)
        {
            _gantry.MoveChain(p, v, a);
        }

        public void MoveChain(Point3D p, int v, int a, double positionWindow)
        {
            _gantry.MoveChain(p, v, a, positionWindow);
        }

        public void MoveToCeilingChain(Point2D p, int v, int a, double positionWindow)
        {
            _gantry.MoveChain(new Point3D(NegativeZLimit, p), v, a, positionWindow);
        }

        public void MoveRelativeChain(Point3D shift, int v, int a, double positionWindow)
        {
            _gantry.MoveRelativeChain(shift, v, a, positionWindow);
        }

        public void EnableChain()
        {
            _gantry.EnableChain();
        }

        public Point3D Position()
        {
            return _gantry.Position();
        }

        public bool InPosition(Point3D position, double error)
        {
            return _gantry.InPosition(position, error);
        }

        public bool BeginWeightConfiguration()
        {
            return _tilts.BeginConfiguration();
        }

        public bool EndWeightConfiguration()
        {
            return _tilts.EndWeight();
        }

        public bool BeginWeightItem()
        {
            return _tilts.BeginWeight();
        }

        public bool EndWeightItem()
        {
            if (_tilts.EndWeight())
            {
                ((CajaPasaportes) _element).Peso = _tilts.Weight;
                return true;
            }
            return false;
        }

        public bool tiltHabilitada()//MCR.2015.03.13
        {
            if (!_tilts.Habilitado)
            {
                ((CajaPasaportes)_element).ValidacionManual();
            }
            return _tilts.Habilitado;
        }

        #endregion

        public Gradient V { get; private set; }
        public Gradient A { get; private set; }

        #region IChainControllersOwner Members

        public IEnumerable<IChainController> GetChainControllers()
        {
            return (new List<IChainController> {_chainController})
                .Concat(_gantry.GetChainControllers()).Concat(_actuadorGiro.GetChainControllers());
        }

        #endregion

        #region ISpecialDeviceOwner Members

        public IEnumerable<ISpecialDevice> GetSpecialDevices()
        {
            return new List<ISpecialDevice> {_gantry};
        }

        #endregion

        public IElement PeekElement()
        {
            return _element;
        }

        public void ElementAdded(IElement element)
        {
            _element = element;
        }

        public IElement ElementQuitted()
        {
            IElement value = _element;
            _element = null;
            return value;
        }

        #region Miembros de IAutomaticRunnable

        public IEnumerable<Chain> GetAutoChains()
        {
            return new[] {new CadAutoGripper("Cadena pinza", _sys).AddChainControllers(this)};
        }

        #endregion

        #region Miembros de ISecurityDiagnosisOwner

        IEnumerable<SecurityDiagnosis> IDiagnosisOwner.GetSecurityDiagnosis()
        {
            return new SecurityDiagnosis[]
                       {
                           new SecurityDiagnosisSignal(_sys.Bus.In("F712"), TypeDiagnosisSignal.Failure),
                           new SecurityDiagnosisSignal(_sys.Bus.In("F713"), TypeDiagnosisSignal.Failure),
                           new SecurityDiagnosisSignal(_sys.Bus.In("F714"), TypeDiagnosisSignal.Failure)
                       };
        }

        #endregion

        #region Miembros de IRi

        void IRi.Ri()
        {
            _gantry.Ri();
        }

        #endregion

        #region Nested type: Gradient

        public struct Gradient
        {
            public int Hight;
            public int Low;
            public int Middle;
        }

        #endregion
    }
}