using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Idpsa.Control.Diagnosis
{
    public sealed class AlarmsHistoricalManager
    {
        private static readonly AlarmsHistoricalManager _instance = new AlarmsHistoricalManager();

        private AlarmsHistoricalManager()
        {
            AlarmsSys = LoadAlarmsHistorical(ConfigFiles.Alarms);
            EmergenciesSys = LoadAlarmsHistorical(ConfigFiles.Emergencies);
        }

        public AlarmsHistorical AlarmsSys { get; private set; }
        public AlarmsHistorical EmergenciesSys { get; private set; }

        public static AlarmsHistoricalManager Instance
        {
            get { return _instance; }
        }

        private static AlarmsHistorical LoadAlarmsHistorical(string file)
        {
            AlarmsHistorical alarms = null;
            if (File.Exists(file))
            {
                try
                {
                    var readFile = new FileStream(file, FileMode.Open, FileAccess.Read);
                    var BFormatter = new BinaryFormatter();
                    alarms = (AlarmsHistorical)BFormatter.Deserialize(readFile);
                    readFile.Close();
                }
                catch (Exception)
                {
                    alarms = new AlarmsHistorical();
                }
            }
            else
            {
                alarms = new AlarmsHistorical();
            }
            return alarms;
        }

        public void SaveAlarmsHistorical()
        {
            SaveAlarmsHistorical(ConfigFiles.Alarms, AlarmsSys);
            SaveAlarmsHistorical(ConfigFiles.Emergencies, EmergenciesSys);
        }

        private static void SaveAlarmsHistorical(string file, AlarmsHistorical parAlarmas)
        {
            try
            {
                var writeFile = new FileStream(file, FileMode.Create, FileAccess.Write);
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, parAlarmas);
                writeFile.Close();
            }
            catch
            {
                throw;
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
                        AlarmsSys.Add(diagnosis.ErrorMessage);
                    }
                }
            }
        }

        public void AddEventDiagnosis(GeneralDiagnosis diagnosis)
        {
            if ((diagnosis.Type & DiagnosisType.Event) != 0)
            {
                if ((diagnosis.Type & DiagnosisType.Security) != 0)
                    EmergenciesSys.Add(diagnosis.ErrorMessage);
                else
                {
                    AlarmsSys.Add(diagnosis.ErrorMessage);
                }
            }
        }
    }
}