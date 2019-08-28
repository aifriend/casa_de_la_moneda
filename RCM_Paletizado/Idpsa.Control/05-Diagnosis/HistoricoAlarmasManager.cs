using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Idpsa.Control.Diagnosis
{
    public sealed class HistoricoAlarmasManager
    {
        private static readonly HistoricoAlarmasManager _instance = new HistoricoAlarmasManager();

        private HistoricoAlarmasManager()
        {
            AlarmasSys = LoadHistoricoAlarmas(ConfigFiles.Alarms);
            EmergenciasSys = LoadHistoricoAlarmas(ConfigFiles.Emergencies);
        }

        public HistoricoAlarmas AlarmasSys { get; private set; }
        public HistoricoAlarmas EmergenciasSys { get; private set; }


        public static HistoricoAlarmasManager Instance
        {
            get { return _instance; }
        }

        private static HistoricoAlarmas LoadHistoricoAlarmas(string file)
        {
            HistoricoAlarmas Alarmas;
            if (File.Exists(file))
            {
                try
                {
                    var readFile = new FileStream(file, FileMode.Open, FileAccess.Read);
                    var BFormatter = new BinaryFormatter();
                    Alarmas = (HistoricoAlarmas)BFormatter.Deserialize(readFile);
                    readFile.Close();
                }
                catch (Exception)
                {
                    Alarmas = new HistoricoAlarmas();
                }
            }
            else
            {
                Alarmas = new HistoricoAlarmas();
            }
            return Alarmas;
        }

        public void SaveHistoricoAlarmas()
        {
            SaveHistoricoAlarmas(ConfigFiles.Alarms, AlarmasSys);
            SaveHistoricoAlarmas(ConfigFiles.Emergencies, EmergenciasSys);
        }

        public static void SaveHistoricoAlarmas(string file, HistoricoAlarmas parAlarmas)
        {
            try
            {
                var writeFile = new FileStream(file, FileMode.Create, FileAccess.Write);
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, parAlarmas);
                writeFile.Close();
            }
            catch (Exception)
            {
            }
        }

        public void AddStepsDiagnosis()
        {
            if (DiagnosisManager.Instance.Items.Count > 0)
            {
                foreach (GeneralDiagnosis diagnosis in DiagnosisManager.Instance.Items.AllValues)
                {
                    if ((diagnosis.Type & DiagnosisType.Step) != 0)
                    {
                        AlarmasSys.Add(diagnosis.Description);
                    }
                }
            }
        }

        public void AddEventDiagnosis(GeneralDiagnosis diagnosis)
        {
            if ((diagnosis.Type & DiagnosisType.Event) != 0)
            {
                if ((diagnosis.Type & DiagnosisType.Seguridad) != 0)
                    EmergenciasSys.Add(diagnosis.Description);
                else
                {
                    AlarmasSys.Add(diagnosis.Description);
                }
            }
        }
    }
}