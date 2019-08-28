using System;

namespace Idpsa.Control.Component
{
    public static class CylinderEx
    {
        public static string ToStringImage(this CylinderPosition value)
        {
            switch (value)
            {                
                case CylinderPosition.Dead:
                    return "muerto";                   
                case CylinderPosition.Rest:
                    return "reposo";                   
                case CylinderPosition.Work:
                    return "trabajo";
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }

        public static CylinderPosition GetPosition(this ICylinder value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.InRest)
                return CylinderPosition.Rest;
            if (value.InWork)
                return CylinderPosition.Work;
            return CylinderPosition.Dead;
        }
        public static void SetPosition(this ICylinder value, CylinderPosition position)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            switch (position)
            {
                case CylinderPosition.Dead:
                    value.Dead();
                    break;
                case CylinderPosition.Rest:
                    value.Rest();
                    break;
                case CylinderPosition.Work:
                    value.Work();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("position");
            }
        }

    }

}