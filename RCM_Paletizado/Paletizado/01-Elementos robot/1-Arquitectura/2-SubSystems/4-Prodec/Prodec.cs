using System;
using System.Collections.Generic;
using Idpsa.Control;
using Idpsa.Control.Component;
using Idpsa.Control.Diagnosis;
using Idpsa.Control.Engine;
using Idpsa.Control.Subsystem;

namespace Idpsa.Paletizado
{
    [Serializable]
    public class Prodec : IDiagnosisOwner, IManagerRunnable, IItemSolicitor<GrupoPasaportes>,
                          IItemSuplier<CajaPasaportes>
    {
        public delegate void GroupAddedToProdec(GrupoPasaportes grupo);
        public delegate void BoxLeavingProdec(CajaPasaportes caja);

        public event GroupAddedToProdec GroupAdded;
        public event BoxLeavingProdec BoxCreated;

        private readonly Input _cajaParaRechazar; //Fallo de precintado
        private readonly SystemControl _control;
        private readonly Input _entradaPaquetes; //Para saber cuando se pueden enviar más paquetes
        private readonly Input _fallo; //Diagnosis y/o emergencia

        private readonly Output _maquinaAnteriorParada; //Se saca notificación de Máquina Anterior Parada
        private readonly Input _marcha; //Funcionamiento en automático
        private readonly Output _permisoSalida; //Permiso para salida de cajas
        private readonly IItemSolicitor<CajaPasaportes> _solicitor;
        private readonly Input _stopEmergencia; //Maquina en emergencia
        private bool _entradaLibre;

        public Prodec(Input entradaPaquetes, Input stopEmergencia, Input marcha, Input fallo,
                      Input cajaParaRechazar, Output maquinaAnteriorParada, Output permisoSalida,
                      IItemSolicitor<CajaPasaportes> solicitor, SystemControl control)
        {
            _entradaPaquetes = entradaPaquetes;
            _stopEmergencia = stopEmergencia;
            _marcha = marcha;
            _fallo = fallo;
            _cajaParaRechazar = cajaParaRechazar;
            _maquinaAnteriorParada = maquinaAnteriorParada;
            _permisoSalida = permisoSalida;
            _solicitor = solicitor;
            GruposPasaportes = new List<GrupoPasaportes>();
            _control = control;
        }

        public List<GrupoPasaportes> GruposPasaportes { get; private set; }

        private Output PermisoSalida
        {
            get { return _permisoSalida; }
        }

        public Output MaquinaAnteriorParada
        {
            get { return _maquinaAnteriorParada; }
        }

        private bool EntradaLibre
        {
            get { return (MarchaOk() && _entradaPaquetes.Value()); }
        }

        #region IDiagnosisOwner Members

        public IEnumerable<SecurityDiagnosis> GetSecurityDiagnosis()
        {
            var list = new List<SecurityDiagnosis>
                           {
                               new SecurityDiagnosisCondition("EMERGENCIA PRODEC",
                                                              "ENCAJADORA DE PASAPORTES EN EMERGENCIA",
                                                              DiagnosisType.Step,
                                                              () => _stopEmergencia.Value()),
                               new SecurityDiagnosisCondition("PRODEC EN FALLO", "LA ENCAJADORA SE ENCUENTRA EN FALLO",
                                                              DiagnosisType.Step,
                                                              () => (_fallo.Value() && !_stopEmergencia.Value())),
                               new SecurityDiagnosisCondition("PRODEC FALLO PRECINTADO",
                                                              "DEBE RETIRARSE CAJA DE LA ENCAJADORA POR FALLO DE PRECINTADO",
                                                              DiagnosisType.Step,
                                                              () => _cajaParaRechazar.Value()),
                               new SecurityDiagnosisCondition("PRODEC NO EN AUTOMATICO",
                                                              "DEBE SELECCIONAR MODO AUTOMATICO EN LA PRODEC",
                                                              DiagnosisType.Step,
                                                              delegate
                                                                  {
                                                                      bool value = false;
                                                                      if (!_stopEmergencia.Value() && !_fallo.Value())
                                                                      {
                                                                          if (!_marcha.Value())
                                                                              value = true;
                                                                      }
                                                                      return value;
                                                                  })
                           };

            return list;
        }

        #endregion

        #region Miembros de IManagerRunnable

        private int _count;

        public IEnumerable<Action> GetManagers()
        {
            return new List<Action> {Gestor};
        }

        private void Gestor()
        {
            PermisoSalida.Activate(_solicitor.ReadyToPutElement && _control.InActiveMode(Mode.Automatic));
            if (_solicitor.ReadyToPutElement && _control.InActiveMode(Mode.Automatic) && ContieneCaja())
            {
                _count++;
                _solicitor.PutElement(this);
            }
        }

        #endregion

        #region Miembros de IItemSolicitor<GrupoPasaportes>

        public GrupoPasaportes PutElement(IItemSuplier<GrupoPasaportes> suplier)
        {
            GrupoPasaportes item = suplier.QuitItem();
            GrupoAñadido(item);
            return item;
        }

        public bool ReadyToPutElement
        {
            get { return EntradaLibre; }
        }

        #endregion

        #region Miembros de IItemSuplier<GrupoPasaportes>

        public CajaPasaportes QuitItem()
        {
            return CajaRetirada();
        }

        #endregion

        private bool MarchaOk()
        {
            return (_marcha.Value() && !_fallo.Value() && !_stopEmergencia.Value());
        }

        private void GrupoAñadido(GrupoPasaportes grupo)
        {
            GruposPasaportes.Add(grupo);
            GroupAdded(grupo);
        }

        private CajaPasaportes CajaRetirada()
        {
            CajaPasaportes caja = null;
            if (GruposPasaportes.Count >= CajaPasaportes.NGrupos)
            {
                caja = new CajaPasaportes(GruposPasaportes[0]);
                for (int i = 0; i < CajaPasaportes.NGrupos; i++)
                    caja.Add(GruposPasaportes[i], CajaPasaportes.NGrupos - i - 1);

                GruposPasaportes.RemoveRange(0, CajaPasaportes.NGrupos);
            }
            BoxCreated(caja);
            return caja;
        }

        private bool ContieneCaja()
        {
            return (GruposPasaportes.Count >= CajaPasaportes.NGrupos);
        }

        //MDG.2010-12-02.Metodos de salvado y carga de los grupos de pasaportes almacenados
        public StoredDataProdecGroups GetDataToStore()
        {
            return new StoredDataProdecGroups //
                       {
                           Groups = GruposPasaportes
                           //,//_grupos,//Group = GrupoPasaportes,//_production.GetBoxes().Select(b => b.Id).ToList(),
                           //State = _state,
                           //Name = this.Name//,
                           //Vaciar = this.Vaciar,//MDG.2010-12-09
                           //ModoAcumulacion = this.ModoAcumulacion//MDG.2010-12-09
                       };
        }

        public void SetDataStored(StoredDataProdecGroups grupoCargado)
        {
            GruposPasaportes = grupoCargado.Groups;
            //_state = GrupoCargado.State;
            //Vaciar = GrupoCargado.Vaciar;//MDG.2010-12-09
            //ModoAcumulacion = GrupoCargado.ModoAcumulacion;//MDG.2010-12-09
        }
    }
}