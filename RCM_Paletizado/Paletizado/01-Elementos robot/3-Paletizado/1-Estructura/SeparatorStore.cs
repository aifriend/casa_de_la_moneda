using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Manuals;
using Idpsa.Control.Subsystem;
using Idpsa.Control.Tool;
using Idpsa.Paletizado;
using Idpsa.Paletizado.Definitions;

namespace Idpsa
{
    public class SeparatorStore : IDiagnosisOwner, ISupplier, IContainer, IManualsProvider
    {
        //Esquina superior izquierda    

        private const Locations _location = Locations.Carton;
        private readonly CornerPoint3D _cornerPoint;
        private DistanceIntervals _defaultHightIntervalSeparator;
        private DistanceIntervals _hightIntervalSeparator;


        private ISeparator _separator;
        private Spin _spin;

        public SeparatorStore(string name, CornerPoint3D cornerPoint, IEvaluable sensor)
        {
            Name = name;
            _cornerPoint = cornerPoint;
            Sensor = sensor;
        }

        public string Name { get; private set; }

        [Manual(SuperGroup = "General", Group = "Sensores")]
        public IEvaluable Sensor { get; private set; }

        public PointSpin3D InitialPosition
        {
            get
            {
                DistanceIntervals value = null;
                if (_hightIntervalSeparator.CentralPosition != null)
                    value = _hightIntervalSeparator;
                else
                    value = _defaultHightIntervalSeparator;

                return new PointSpin3D(_spin, InitialPos(value));
            }
        }

        public PointSpin3D FinalPosition
        {
            get
            {
                DistanceIntervals value;
                if (_hightIntervalSeparator.CentralPosition != null)
                    value = _hightIntervalSeparator;
                else
                    value = _defaultHightIntervalSeparator;

                return new PointSpin3D(_spin, FinalPos(value));
            }
        }

        #region IDiagnosisOwner Members

        public IEnumerable<SecurityDiagnosis> GetSecurityDiagnosis()
        {
            var list = new List<SecurityDiagnosis>();
            list.Add(new SecurityDiagnosisCondition("Faltan cartones en " + Name,
                                                    "Reponga cartones en palet de hojas de cartón",
                                                    DiagnosisType.Step, () => !Sensor.Value()));
            return list;
        }

        #endregion

        #region Miembros de ISupplier

        public ElementTypes? ElementSupplied()
        {
            if (_separator == null)
                return null;
            else if (Sensor.Value())
                return ElementTypes.Separator;
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
            return _separator;
        }

        public void MerorizeHight(double position)
        {
            _hightIntervalSeparator.CentralPosition = position;
        }

        #endregion

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return (this).GetManualsRepresentations();
        }

        #endregion

        public void SetNewCatalog(DatosCatalogoPaletizado datosCatalogo)
        {
            _separator = datosCatalogo.PaletizerDefinition.Separator;
        }

        private void InitializateLocations()
        {
            _defaultHightIntervalSeparator = new DistanceIntervals(_cornerPoint.Z, 100, 100);
            _hightIntervalSeparator = new DistanceIntervals(default(Double), 20, 50);
            _spin = Spin.S0;
        }

        private Point2D CentralPosition()
        {
            var geo = new GeometriaConf();
            return new Rectangle(_cornerPoint.ToCornerPoint2D(), geo.SeparatorSize).Center;
        }


        private void DeleteMemoricedHight()
        {
            _hightIntervalSeparator.CentralPosition = null;
        }

        private Point3D InitialPos(DistanceIntervals distanceInverval)
        {
            return new Point3D(distanceInverval.NegativePosition, CentralPosition());
        }

        private Point3D FinalPos(DistanceIntervals distanceInterval)
        {
            return new Point3D(distanceInterval.PositivePosition, CentralPosition());
        }
    }
}