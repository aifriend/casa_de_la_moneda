using System;

namespace Idpsa.Control.Component
{
    [Serializable]
    public class Baliza
    {
        private readonly ActuadorP _luzAzul;
        private readonly ActuadorP _luzNaranja;
        private readonly ActuadorP _luzRoja;
        private readonly ActuadorP _luzVerde;
        private readonly TON _timerNaranja;
        private readonly TON _timerRojo;

        public Baliza(ActuadorP luzRoja, ActuadorP luzNaranja, ActuadorP luzVerde, ActuadorP luzAzul)
        {
            _luzRoja = luzRoja;
            _luzNaranja = luzNaranja;
            _luzVerde = luzVerde;
            _luzAzul = luzAzul;
            _timerRojo = new TON();
            _timerNaranja = new TON();
        }

        public ActuadorP LuzRoja
        {
            get { return _luzRoja; }
        }

        public ActuadorP LuzNaranja
        {
            get { return _luzNaranja; }
        }

        public ActuadorP LuzVerde
        {
            get { return _luzVerde; }
        }

        public ActuadorP LuzAzul
        {
            get { return _luzAzul; }
        }

        public TON TimerRojo
        {
            get { return _timerRojo; }
        }

        public TON TimerNaranja
        {
            get { return _timerNaranja; }
        }

        public void TurnOff()
        {
            LuzRoja.Activate(false);
            LuzNaranja.Activate(false);
            LuzVerde.Activate(false);
            LuzAzul.Activate(false);
        }
    }
}