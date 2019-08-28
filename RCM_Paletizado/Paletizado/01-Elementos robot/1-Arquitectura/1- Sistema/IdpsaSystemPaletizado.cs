using System;
using System.Runtime.Remoting.Contexts;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Engine;
using Idpsa.Control.Engine.Status;
using Idpsa.Control.Subsystem;

namespace Idpsa.Paletizado
{
    [Synchronization]
    [Serializable]
    public partial class IdpsaSystemPaletizado : IDPSASystem, IDisposable
    {
        public GeometriaConf Geo; //MCR
        #region Subsystems

        [Subsystem(Filter = SubsystemFilter.None)]
        public BotoneraBarrera BotoneraBarrera { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public SystemProductionPaletizado Production { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public Gripper Gripper { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public Lines Lines { get; set; }

        [Subsystem(Filter = SubsystemFilter.All)]
        public DataBase Db { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public EnlaceRulamat Rulhamat { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public RuhlamatManualFeed ManualFeedRuhlamat { get; set; }

        [Subsystem(Filter = SubsystemFilter.None)]
        public AuthorizedReset barreraRearme { get; set; }

        #endregion

        protected override Bus ConstructBus()
        {
            return new Profibus(81, 81);
        }

        protected override SystemControl ConstructSystemControl()
        {
            Input presostato = Bus.In("A020");
            Output valvulaAireOut = Bus.Out("Y085");
            Input presostato2 = Bus.In("A020");
            Output valvulaAireOut2 = Bus.Out("Output4.0");
            Input presostato3 = Bus.In("A020");
            Output valvulaAireOut3 = Bus.Out("K744");//MDG.2012-11-12.Contactor habilitacion ascensor 1
            //var airStatus = new AirAddedStatus("AireOk", presostato, valvulaAireOut);
            var airStatus = new AirAddedStatus("AireOk", presostato, valvulaAireOut, presostato2, valvulaAireOut2, presostato3, valvulaAireOut3);

            return new SystemControl(this).WithAddedStatus(airStatus);
            //.WithAddedStatus2(airStatus2);//MDG.2012-07-23
        }

        //protected override SystemControl2 ConstructSystemControl2()
        //{
        //    Input presostato = Bus.In("A020");//MDG.2012-07-23.El mismo presostato que la zona general
        //    Output valvulaAireOut = Bus.Out("Output4.0");
        //    var airStatus = new AirAddedStatus("AireOk", presostato, valvulaAireOut);

        //    return new SystemControl2(this).WithAddedStatus(airStatus);
        //}

        protected override SystemGeneralSignals ConstructSystemGeneralSignals()
        {
            ICommandController commandController = ConstructCommandController();

            Input desconexionMandoIn = Bus.In("A021_S902");
            Output busOk = Bus.Out("K928");
            IActivable busOkAndReset = Activable.FromFunctor(work => busOk.Activate(!desconexionMandoIn.Value() && work));

            IActivable botonConexionMandoLuz = Bus.Out("H901")//.Join(Bus.Out("Output0.7"))
                .DelayToConnection(1000); 

            return new SystemGeneralSignals(busOkAndReset, Bus, Control, commandController,
                                            new GeneralSecuritiesPaletizado(Bus))
                .WithCommandActivatedSignals(botonConexionMandoLuz);
        }

        private ICommandController ConstructCommandController()
        {
            Input emergenciaOkIn = Bus.In("K921D");
            Input mandoConectadoIn = Bus.In("K922B");
            Input botonConexionMando1 = Bus.In("S901");
           // Input botonConexionMando2 = Bus.In("Input3.4");
            IEvaluable botonConexionMando = botonConexionMando1.AND(Bus.Out("Output0.7"));//.OR(botonConexionMando2);

            Input ProdecEnEmergencia = Bus.In("ProdecEmergencia");
            Input ProdecEnFallo = Bus.In("ProdecFallo");
            Input ProdecParada = Bus.In("ProdecParada");
            IEvaluable PararCasilleros = ProdecEnEmergencia.OR(ProdecEnFallo.OR(ProdecParada));

            Output emergenciaOkOut = Bus.Out("K960");
            Output conexionMandoOkOut = Bus.Out("K961");
            Output StopCasilleros = Bus.Out("StopCas");

            Input emergenciaOkIn2 = Bus.In("Input56.7");
            Input mandoConectadoIn2 = Bus.In("Input56.6");
            Input botonConexionMando2 = Bus.In("Input4.5"); //MCR. 2016. mod. Rearme Aereos. 
            Output conexionMandoOkOut2 = Bus.Out("H902");
            Output conexionMandoRele2 = Bus.Out("Output49.3");

            return new CommandControllerPaletizadora() //MCR. 2016. Rearme Aereos.
                .WithInputs(mandoConectadoIn, emergenciaOkIn,
                            botonConexionMando, Input.Simulated, PararCasilleros,
                            emergenciaOkIn2, mandoConectadoIn2, botonConexionMando2)
                .WithOutputs(conexionMandoOkOut, emergenciaOkOut,
                             StopCasilleros, conexionMandoOkOut2, conexionMandoRele2
                             );
        }

        #region Miembros de IDisposable

        protected override void DisposeCore()
        {
            Bus.ResetOutputs();
        }

        #endregion
    }
}