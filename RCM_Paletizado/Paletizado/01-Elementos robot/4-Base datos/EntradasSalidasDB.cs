using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;

namespace Idpsa
{
   public enum Estado
    {
        Dentro,
        Fuera,
       Autorizado
    }

   public enum EstadoRearme
   {
       Entrando,
       Rearmando,
       Rearmado
   }

    

   [Serializable]
   public class RegistroES
   {
       private string _empleado; //datos del catálogo que se está produciendo.
       public DateTime _fechaEntrada;
       private DateTime _fechaSalida;
       public Estado estado;

       public RegistroES()
       {
           _empleado = "";
           _fechaEntrada = DateTime.Now;
           _fechaSalida = DateTime.Now;
       }
       public RegistroES(string empleado)
       {
           _empleado = empleado;
           estado = Estado.Dentro;
           _fechaEntrada = DateTime.Now;
           _fechaSalida = new DateTime(1,1,1,1,1,1);
       }
       public RegistroES(RegistroES reg)
       {
           _empleado = reg._empleado;
           _fechaEntrada = reg._fechaEntrada;
           _fechaSalida = reg._fechaSalida;
           estado = reg.estado;
       }
       public RegistroES(string Autorizado, RegistroES aut)
       {
           _empleado = aut.employee;
           estado = Estado.Autorizado;
           _fechaEntrada = DateTime.Now;
           _fechaSalida = DateTime.Now;
       }

       private void SetSalida ()
       {
           _fechaSalida = DateTime.Now;
           estado = Estado.Fuera;
       }
       public bool Autorizado()
       {
           if (_empleado.Contains("1657")
               || _empleado.Contains("1980")
               || _empleado.Contains("0000")
               || _empleado.Contains("4326")
               || _empleado.Contains("1657")
               || _empleado.Contains("4363"))
               return true;
           return false;
       }
       public bool Salida(bool forzada)
       {
           TimeSpan difference = DateTime.Now - _fechaSalida;
           bool tiempo = (difference.Milliseconds >= 50);

           if (estado != Estado.Dentro&&tiempo)
               return false;

           difference = DateTime.Now - _fechaEntrada;
           tiempo = (difference.Milliseconds >= 50);
           if (forzada || tiempo)
           {
               SetSalida();
           }
           return (true);
       }
       
       public String horaEntrada
       {
           get
           {
               String hora = _fechaEntrada.Hour.ToString();
               if (_fechaEntrada.Hour <= 9)
                   hora = "0" + _fechaEntrada.Hour.ToString();
               String minuto = _fechaEntrada.Minute.ToString();
               if (_fechaEntrada.Minute <= 9)
                   minuto = "0" + _fechaEntrada.Minute.ToString();
               String aux1 = hora + ":" + minuto;
               return aux1;
           }
       }
       public String horaSalida
       {
           get
           {
               if (_fechaSalida.Year == 1)
                   return ("");
               String hora = _fechaSalida.Hour.ToString();
               if (_fechaSalida.Hour <= 9)
                   hora = "0" + _fechaSalida.Hour.ToString();
               String minuto = _fechaSalida.Minute.ToString();
               if (_fechaSalida.Minute <= 9)
                   minuto = "0" + _fechaSalida.Minute.ToString();
               String aux1 = hora + ":" + minuto;
               return aux1;
           }
       }
       public String employee
       {
           get
           {
               return _empleado.ToString();
           }
       }
       public String state
       {
           get
           {
               String aux1 = "";
               if (estado == Estado.Dentro)
                   aux1 = "Dentro";
               else if (estado == Estado.Fuera)
                   aux1 = "Fuera";
               else if (estado == Estado.Autorizado)
                   aux1 = "Rearme Autorizado";
               return aux1;
           }
       }
   }


    [Serializable]
   public class ParadaDB
   {
       private DateTime _fecha;
       public List<RegistroES> Empleados;
       public RegistroES Rearme;
       public EstadoRearme estado;

       public ParadaDB()
       {
           _fecha = DateTime.Now;
           Empleados = new List<RegistroES>();
           Rearme = new RegistroES();
           estado = EstadoRearme.Entrando;
       }

       public ParadaDB(RegistroES empleado)
       {           
           _fecha = empleado._fechaEntrada;
           Empleados = new List<RegistroES>();
           Empleados.Add (empleado);
           Rearme = new RegistroES();
           estado = EstadoRearme.Entrando;
       }
       public ParadaDB(ParadaDB parada)
       {
           _fecha = parada._fecha;
           Empleados = new List<RegistroES>();

           Rearme = new RegistroES(parada.Rearme);

           foreach (var empleado in parada.Empleados)
           {
               Empleados.Add(empleado);
           }
           estado = parada.estado;
       }

       public void ForzarRearme (RegistroES operario)
       {
           Rearme = new RegistroES("rearme", operario);
           foreach (RegistroES emp in Empleados)
           {
               emp.Salida(true);
           }
       }

       public bool newSignal(RegistroES operario)
       {
           bool contenido = false;
           bool alguienDentro = true;
           if (estado == EstadoRearme.Rearmado)
           {
               return false;
           }
           if (estado == EstadoRearme.Rearmando)
               if (operario.Autorizado())
               {
                   Rearme = new RegistroES("Autorizado", operario);
                   foreach (RegistroES emp in Empleados)
                   {
                       emp.Salida(true);
                   }
                   return false;
               }
               else
                   return true;
           //Rearme = new RegistroES("Rearme", operario);
           else
           {
               bool salidaBool = false;
               alguienDentro = false;
               foreach (RegistroES emp in Empleados)
               {
                   contenido = false;
                   if (emp.employee == operario.employee)
                   {
                       contenido = true;
                   }
                   if (contenido)
                       salidaBool = emp.Salida(false);

                   if (salidaBool)
                       alguienDentro = true;
               }

               if (!alguienDentro)
               {
                   Empleados.Add(operario);
               }

               alguienDentro = false;
               foreach (RegistroES emp in Empleados)
               {
                   if (emp.estado != Estado.Fuera)
                   {
                       alguienDentro = true;
                   }
               }
               if (!alguienDentro&&Rearme.employee=="")
                   Rearme = new RegistroES("rearme", operario);
               return (alguienDentro);

           }
           SaveDB();
       }

       public String fechaStr
       {           
           get
           {
               String mes = _fecha.Month.ToString();
               if (_fecha.Month <= 9)
                   mes = "0" + _fecha.Month.ToString();
               String dia = _fecha.Day.ToString();
               if (_fecha.Day <= 9)
                   dia = "0" + _fecha.Day.ToString();
               String aux1 = dia+"/" + mes +"/"+ _fecha.Year.ToString();
               return aux1;
           }
       }

       public string OperarioRearme
       {
           get
           {
               return Rearme.employee;
           }
       }

       public string HoraRearme
       {
           get
           {
               return Rearme.horaEntrada;
           }
       }

       public void SaveDB()
       {
           if (Empleados != null)
           {
               string filePath = Path.Combine(ConfigPaletizadoFiles.RegistroES, fileName());

               using (var writeFile = new FileStream(filePath, FileMode.Create, FileAccess.Write))
               {
                   var BFormatter = new BinaryFormatter();
                   BFormatter.Serialize(writeFile, this);
               }
           }
       }

       public ParadaDB SearchDB(string fech)
       {
           datosParada aux = new datosParada(fech);
           if (aux.StopArray.Count > 0)
           {
               ParadaDB returnParada= new ParadaDB(aux.StopArray[0]);
               if (aux.StopArray.Count > 1)
                   foreach (ParadaDB stop in aux.StopArray)
                   {
                       if (DateTime.Compare(stop._fecha, returnParada._fecha) > 0)
                           returnParada = stop;
                   }
               return returnParada;
           }

           else
               return new ParadaDB();
           return new ParadaDB();
       }

        public String fileName()
       {
           String hora = _fecha.Hour.ToString();
           if (_fecha.Hour <= 9)
               hora = "0" + _fecha.Hour.ToString();
           String min = _fecha.Minute.ToString();
           if (_fecha.Minute <= 9)
               min = "0" + _fecha.Minute.ToString();
           String mes = _fecha.Month.ToString();
           if (_fecha.Month<=9)
               mes="0"+_fecha.Month.ToString();
           String dia = _fecha.Day.ToString();
           if (_fecha.Day <= 9)
               dia = "0"+_fecha.Day.ToString();
           String aux1 = _fecha.Year.ToString() + mes+ dia+hora+min;
           return aux1;
       }

        public List<RegistroES> Operarios
        {
            get 
            { 
                return Empleados; 
            }
        }

        public List<RegistroES> getOperarios()
        {
                return Empleados;
        }

        public ParadaDB LoadDatos()
        {
            String aux1 = fechaStr;
            return SearchDB(aux1);

        }
   }

   
    public class datosParada
    {
       private string _fecha;
       private List<ParadaDB> _stopArray;

       public datosParada()
       { 
       }
        public datosParada(string Date)
        {
            _fecha = Date;
            List<ParadaDB> _stopArray = new List<ParadaDB>();
            if (_stopArray.Count > 0)
                foreach (ParadaDB element in _stopArray)
                    _stopArray.Remove(element);
            LoadDatos();
        }


        public String fileName()
        {
            return (_fecha);
        }


        public string Fecha
        {
            get
            {
                return _fecha;
            }
        }

        public List<ParadaDB> StopArray
        {
            get
            {
                if (_stopArray == null)
                    _stopArray = new List<ParadaDB>();
                return _stopArray;
            }
        }

        public List<ParadaDB> getStopArray()
        {
        
            if (_stopArray == null)
                _stopArray = new List<ParadaDB>();
            return _stopArray;
            
        }


        public ParadaDB LoadData(string fileName)
        {
            string filePath = Path.Combine(ConfigPaletizadoFiles.RegistroES, fileName);

            using (var readFile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var BFormatter = new BinaryFormatter();
                var catalog = (ParadaDB)BFormatter.Deserialize(readFile);
                return catalog;
            }
        }

        public bool SearchDB(string fech)
        {
            string actualFile = "";
            if (fech == "")
                return false;
            try
            {
                var directory = new DirectoryInfo(ConfigPaletizadoFiles.RegistroES);
                FileInfo[] files = directory.GetFiles();
                var bFormatter = new BinaryFormatter();
                if (_stopArray!=null)
                    _stopArray.Clear();
                _stopArray = new List<ParadaDB>();

                foreach (FileInfo file in files)
                {
                    actualFile = file.Name;
                    bool b = ReadCatalogFromFile(actualFile, fech);
                    if (b)
                        _stopArray.Add(LoadData(actualFile));
                }
                if (_stopArray.Count() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool ReadCatalogFromFile(String actualFile, String fech)
        {
            
            if (actualFile.Contains(fech))
                return true;
            return false;
        }

        public void LoadDatos()
        {
            String aux1 = _fecha;
            SearchDB(aux1);

        }

    }

}
