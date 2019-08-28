using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    internal class CilindroVertical
    {
        #region Correspondence enum

        public enum Correspondence
        {
            UpTrabajo,
            UpReposo
        }

        #endregion

        private readonly Func<bool> _checkDown;
        private readonly Func<bool> _checkUp;
        private readonly ICilindro _cilindro;
        private readonly Action _taskDown;
        private readonly Action _tasKUp;

        public CilindroVertical(ICilindro cilindro, Correspondence correspondence)
        {
            _cilindro = cilindro;
            if (correspondence == Correspondence.UpTrabajo)
            {
                _tasKUp = cilindro.Trabajo;
                _checkUp = (() => cilindro.EnTrabajo);
                _taskDown = cilindro.Reposo;
                _checkDown = (() => cilindro.EnReposo);
            }
            else
            {
                _tasKUp = cilindro.Reposo;
                _checkUp = (() => cilindro.EnReposo);
                _taskDown = cilindro.Trabajo;
                _checkDown = (() => cilindro.EnTrabajo);
            }
        }

        public ICilindro cilindro
        {
            get { return _cilindro; }
        }

        public string Name
        {
            get { return cilindro.Name; }
        }


        public bool IsUp
        {
            get { return _checkUp(); }
        }

        public bool IsDown
        {
            get { return _checkDown(); }
        }

        public void Up()
        {
            _tasKUp();
        }

        public void Down()
        {
            _taskDown();
        }
    }
}