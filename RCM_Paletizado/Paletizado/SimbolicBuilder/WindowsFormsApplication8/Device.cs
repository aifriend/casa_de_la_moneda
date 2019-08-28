using System;
using System.Collections;
using System.Collections.Generic;

namespace Idpsa.SimbolicBuilder
    {    
      public class Device{
      public struct SimbCom
      {
            public string simbolico;
            public string comentario;

            public SimbCom(string Simbolico,string Comentario)
            {
                simbolico = Simbolico;
                comentario = Comentario;
            }
      }

       public Device(string Name)
       {
           this.Name = Name;
           Inputs=new  List<Dictionary<string, SimbCom>>();
           Outputs= new List<Dictionary<string, SimbCom>>();
           StartInputs=new List<string>();
           StartOutputs=new List<string>();
           InputsComent = new List<string>();
           OutputsComent = new List<string>();
           addressSimbolico = new Dictionary<string, string>();
       }
    
        public string Name{get;set;}
        public List<string> StartInputs { get; set; }
        public List<string> InputsComent { get; set; }
        public List<string> StartOutputs { get; set; }
        public List<string> OutputsComent { get; set; }
        public List<Dictionary<string, SimbCom>> Inputs;
        public List<Dictionary<string, SimbCom>> Outputs;
        private Dictionary<string, string> addressSimbolico;

        public void setInputs(string[] str)
        {
            string[] dir = str[0].Split(new char[] {'.'});
            if (dir.Length==2)
            {
                int start;
                if (!int.TryParse(StartInputs[StartInputs.Count-1], out start))
                {
                    throw new Exception("Parámetro de dirección absoluta de entradas :" + StartOutputs + ", inválido"); 
                }
                int Byte = start+int.Parse(dir[0]);
                int bit = int.Parse(dir[1]);
                checkAddressOutOfLitmits(Byte, bit, "entrada");
                string address ="I"+Byte.ToString()+"."+bit;
                checkAddressRepeticion(address, str[1]);
                Inputs[Inputs.Count-1].Add(address,new SimbCom(str[1],str[2]));
            }
            else
            {
                throw new Exception("Error de formato de dirección Byte de entrada profibus"); 
                
            }
        }

    


        public void setOutputs(string[] str)
        {
            string[] dir = str[0].Split(new char[] { '.' });
          
            if (dir.Length == 2)
            {
                int start;
                if (!int.TryParse(StartOutputs[StartOutputs.Count-1], out start))
                {
                    throw new Exception("Parámetro de dirección absoluta de salidas:"+StartOutputs +", inválido");
                    
                }
                int Byte = start + int.Parse(dir[0]);
                int bit = int.Parse(dir[1]);
                checkAddressOutOfLitmits(Byte, bit, "salida");
                string address = "O" + Byte.ToString() + "." + bit;
                checkAddressRepeticion(address, str[1]);
                Outputs[Outputs.Count-1].Add(address, new SimbCom(str[1], str[2]));
            }
            else
            {
                 throw new Exception("Error de formato de dirección Byte de salida profibus");
                
            }
        }

          private void checkAddressRepeticion(string address,string simbolico){
           if (addressSimbolico.ContainsKey(address)){
                   throw new Exception("Error: los textos simbólicos " +addressSimbolico[address]  + " y " +
                                       simbolico + " tienen la misma dirección profibus asociada: " + address);
           }
               else{
                   addressSimbolico.Add(address, simbolico);
                }

        }

          private void checkAddressOutOfLitmits(int Byte, int bit, string Input_Output)
          {
              bool error = false;
              string msg = "La {0} profibus especificada ";
              if (Byte > 254)
              {
                  msg += "tiene una dirección byte mayor que el límite de {0}s profibus (254)";
                  error = true;
              }
              else if (Byte < 0)
              {
                  msg += "tiene una dirección byte negativa";
                  error = true;
              }

              else if (bit > 7)
              {
                  msg += "tiene una dirección bit mayor que 7";
                  error = true;
              }
              else if (bit < 0)
              {
                  msg += "tiene una dirección bit negativa";
                  error = true;
              }

              if (error)
                  throw new Exception(String.Format(msg, Input_Output)); 
             
          }


   }
}