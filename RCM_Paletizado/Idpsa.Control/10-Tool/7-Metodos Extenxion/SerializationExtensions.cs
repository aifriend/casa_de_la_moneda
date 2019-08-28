using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Idpsa.Control.Tool
{
    public static class SerializationExtensions
    {
        public static T Load<T>(string filePath)
        {
            T obj = default(T);
            if (File.Exists(filePath))
                using (var readFile =
                    new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var BFormatter = new BinaryFormatter();
                    obj = (T)BFormatter.Deserialize(readFile);
                }
            return obj;
        }

        public static bool TryLoad<T>(T obj, string filePath)
        {
            bool well = true;
            obj = default(T);
            try
            {
                Load<T>(filePath);
            }
            catch
            {
                well = false;
            }

            return well;
        }       

        public static void Save<T>(this T obj, string filePath)
        {
            using (var writeFile = new FileStream(filePath,
                                                  FileMode.Create, FileAccess.Write))
            {                
                var BFormatter = new BinaryFormatter();
                BFormatter.Serialize(writeFile, obj);  
            }
        }

        public static bool TrySave<T>(this T obj, string filePath)
        {
            var well = true;
            try
            {
                Save(obj, filePath);
            }
            catch
            {
                well = false;
            }
            return well;
        }
    }
    
}