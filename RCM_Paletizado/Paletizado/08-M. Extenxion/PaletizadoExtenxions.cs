using System.Collections.Generic;
using System.Drawing;
using Idpsa.Control.Component;
using Idpsa.Control.Manuals;
using Idpsa.Control.Tool;
using Idpsa.Paletizado.Definitions;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    public static class PaletizadoExtensions
    {
        public static string ToStringImage(this IDLine idLine)
        {
            string str = null;
            switch (idLine)
            {
                case IDLine.Japonesa:
                    str = "Línea numerado japonesa";
                    break;
                case IDLine.Alemana:
                    str = "Línea numerado alemana";
                    break;
            }
            return str;
        }

        public static IEnumerable<Manual> GetManualsRepresentations(this ISolicitor solicitor)
        {
            var evaluable = new LogicalEvaluable(
                () => solicitor.ElementRequested() != null,
                () =>
                    {
                        if (solicitor.ElementRequested() != null)
                            return solicitor.ElementRequested().ToString();
                        else
                            return string.Empty;
                    },
                "Solicitor " + solicitor.Location
                );

            return ((IManualsProvider) evaluable).GetManualsRepresentations();
        }

        public static IEnumerable<Manual> GetManualsRepresentations(this ISupplier suplier)
        {
            var evaluable = new LogicalEvaluable(
                () => suplier.ElementSupplied() != null,
                () =>
                    {
                        if (suplier.ElementSupplied() != null)
                            return suplier.ElementSupplied().ToString();
                        else
                            return string.Empty;
                    },
                "Supplier " + suplier.Location
                );

            return ((IManualsProvider) evaluable).GetManualsRepresentations();
        }


        public static string ToStringImage(this Gripper.Arms arms)
        {
            string value = null;
            switch (arms)
            {
                case Gripper.Arms.Open:
                    value = "abiertos";
                    break;
                case Gripper.Arms.Close:
                    value = "cerrados";
                    break;
                case Gripper.Arms.Any:
                    value = "cualquier posicion";
                    break;
            }
            return value;
        }

        public static string ToStringImage(this Gripper.Extensor extensor)
        {
            string value = null;
            switch (extensor)
            {
                case Gripper.Extensor.Extend:
                    value = "extendido";
                    break;
                case Gripper.Extensor.Retract:
                    value = "retirado";
                    break;
                case Gripper.Extensor.Dead:
                    value = "muerto";
                    break;
                case Gripper.Extensor.Any:
                    value = "cualquier posicion";
                    break;
            }
            return value;
        }

        public static string ToStringImage(this Gripper.Sucker sucker)
        {
            string value = null;
            switch (sucker)
            {
                case Gripper.Sucker.On:
                    value = "vacio";
                    break;
                case Gripper.Sucker.Off:
                    value = "no en vacio";
                    break;
                case Gripper.Sucker.Any:
                    value = "cualquier estado";
                    break;
            }
            return value;
        }


        public static Point2D Transform(this GrowSense growSense, Point2D p)
        {
            Point2D value = null;
            switch (growSense)
            {
                case GrowSense.PosPos:
                    value = new Point2D(p.X, p.Y);
                    break;
                case GrowSense.NegPos:
                    value = new Point2D(-p.X, p.Y);
                    break;
                case GrowSense.PosNeg:
                    value = new Point2D(p.X, -p.Y);
                    break;
                case GrowSense.NegNeg:
                    value = new Point2D(-p.X, -p.Y);
                    break;
            }
            return value;
        }


        public static RectangleF Rotate(this RectangleF rec)
        {
            return new RectangleF(rec.Location.Y, rec.Location.X, rec.Height, rec.Width);
        }
    }


    public static class PasaporteExtensions
    {
        public static string ConvertToString(this TipoPasaporte.TypeRfid t)
        {
            string s = null;
            switch (t)
            {
                case TipoPasaporte.TypeRfid.NoDefined:
                case TipoPasaporte.TypeRfid.No:
                    s = "Sin RfId";
                    break;
                case TipoPasaporte.TypeRfid.A:
                    s = "Tipo A";
                    break;
                case TipoPasaporte.TypeRfid.B:
                    s = "Tipo B";
                    break;
            }
            return s;
        }

        public static string ConvertToString(this TipoPasaporte.Types t)
        {
            return t.ToString();
        }

        public static int NumericValue(this TipoPasaporte.TypeRfid t)
        {
            int value = 0;

            switch (t)
            {
                case TipoPasaporte.TypeRfid.No:
                    value = 0;
                    break;
                case TipoPasaporte.TypeRfid.A:
                    value = 1;
                    break;
                case TipoPasaporte.TypeRfid.B:
                    value = 2;
                    break;
                case TipoPasaporte.TypeRfid.NoDefined: //MDG.2010-12-01
                    value = -1;
                    break;
            }

            return value;
        }


        public static int NumericValue(this TipoPasaporte.Types t)
        {
            int value = 0;
            switch (t)
            {
                case TipoPasaporte.Types.Normal:
                    value = 0;
                    break;
                case TipoPasaporte.Types.Consular:
                    value = 1;
                    break;
                case TipoPasaporte.Types.Servicio:
                    value = 2;
                    break;
                case TipoPasaporte.Types.Diplomatico:
                    value = 3;
                    break;
                case TipoPasaporte.Types.Subsidiario:
                    value = 4;
                    break;
                case TipoPasaporte.Types.Refugiados:
                    value = 5;
                    break;
                case TipoPasaporte.Types.TituloViaje:
                    value = 6;
                    break;
                case TipoPasaporte.Types.Apatridas:
                    value = 7;
                    break;
                case TipoPasaporte.Types.Provisional:
                    value = 8;
                    break;
                case TipoPasaporte.Types.Oficial:
                    value = 9;
                    break;
            }

            return value;
        }
    }
}