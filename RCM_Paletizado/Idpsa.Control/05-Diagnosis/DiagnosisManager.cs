namespace Idpsa.Control.Diagnosis
{
    public sealed class DiagnosisManager
    {
        private static readonly DiagnosisManager _instance = new DiagnosisManager();

        private readonly DiagnosisFamilyLog _diagnosisFamilyLog;

        private DiagnosisManager()
        {
            Items = new DiagnosisCollection();
            _diagnosisFamilyLog = new DiagnosisFamilyLog();
        }

        public DiagnosisCollection Items { get; private set; }

        public static DiagnosisManager Instance
        {
            get { return _instance; }
        }

        public bool Exist(string name)
        {
            return Items.ContainsKey(name);
        }

        public void Remove(GeneralDiagnosis diagnosis)
        {
            _diagnosisFamilyLog.RemoveFromLog(diagnosis);
            Items.Remove(diagnosis.Name);
        }

        public void Add(GeneralDiagnosis diagnosis)
        {
            _diagnosisFamilyLog.AddToLog(diagnosis);
            Items.Add(diagnosis.Name, diagnosis);
        }

        public bool ActivedDiagnosisFamily(string diagnosisFamilyName)
        {
            return _diagnosisFamilyLog.DiagnosisFamilyActive(diagnosisFamilyName);
        }

        public void Clear()
        {
            Items.Clear();
            _diagnosisFamilyLog.Clear();
        }
    }
}