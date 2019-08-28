using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{
    [System.Serializable()]
    public abstract class ActuadorConInversor : IManualsProvider, IJog
    {
        protected string Name{get;set;}
       

        public bool ActuatorStoped { get; protected set; }

        protected ActuadorConInversor(string name)
        {
            Name = name;
        }

        protected ActuadorConInversor()
        {
            Name = String.Empty;
        }

        
      
        public abstract void Activate1();

        public abstract void Activate2();

        public abstract void Deactivate();
       


        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            return GetManualRepresentationsCore();
        }

        protected virtual IEnumerable<Manual> GetManualRepresentationsCore()
        {
            return new[] { new Manual(this) { Descripcion = Name } };
        }

        #endregion



        #region Miembros de IJog

        private Func<bool> _enableJogPos = () => true;
        private Func<bool> _enableJogNeg = () => true;
        private bool _invertedJogSense;
        private string _jogPosName = "";
        private string _jogNegName = "";

        bool IJog.EnableJogPos()
        {
            return _enableJogPos();
        }

        bool IJog.EnableJogNeg()
        {
            return _enableJogNeg();
        }

        void IJog.JogPos()
        {
            if (!_invertedJogSense) Activate1(); else Activate2();
        }

        void IJog.JogNeg()
        {
            if (!_invertedJogSense) Activate2(); else Activate1();
        }

        string IJog.JogPosName
        {
            get { return _jogPosName; }
        }

        string IJog.JogNegName
        {
            get { return _jogNegName; }
        }

        void IJog.StopJog() { Deactivate(); }


        public ActuadorConInversor WithEnableJogPos(Func<bool> enableJogPos)
        {
            _enableJogPos = enableJogPos;
            return this;
        }

        public ActuadorConInversor WithEnableJogNeg(Func<bool> enableJogNeg)
        {
            _enableJogNeg = enableJogNeg;
            return this;
        }

        public ActuadorConInversor WithEnableJog(Func<bool> enableJog)
        {
            _enableJogPos = _enableJogNeg = enableJog;

            return this;
        }

        public ActuadorConInversor WithJogPosName(string jogPosName)
        {
            _jogPosName = jogPosName;
            return this;
        }

        public ActuadorConInversor WithJogNegName(string jogNegName)
        {
            _jogNegName = jogNegName;
            return this;
        }

        public ActuadorConInversor WithInvertedJogSense(bool value)
        {
            _invertedJogSense = value;
            return this;
        }

        #endregion
    }
}
