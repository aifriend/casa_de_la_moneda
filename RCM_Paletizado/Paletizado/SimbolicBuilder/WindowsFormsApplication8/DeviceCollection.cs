using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Idpsa.SimbolicBuilder;
using System.Text.RegularExpressions;

namespace Idpsa.SimbolicBuilder
{
    [Flags()]
    public enum state
    {
        Title=0,
        Device=1,
        EndDevice=2,
        Imputs=4,
        ImputAddress=8,
        OutputAddress=16,
        SignalImputs=32,
        Outpus=64,
        SignalOutputs=128,
    }

    [Flags()]
    public enum command
    {
        None=0,
        Title=1,
        Device=2,
        Start=4,
        EndDevice=8,
        Inputs=16,
        Outputs=32,
        Desconocido=64,
    }

    public class DeviceCollection
    {
        public state estado;
        public Device currentDevice;
        public string Title { get; set; }
        public List<Device> Devices;
      

        public DeviceCollection()
        {
            Title = "";
            currentDevice = null;
            estado = state.Title;
            Devices=new List<Device>();
           
        }




        public List<Device> Elements()
        {
            return Devices;
        }


        private int currentLine;

        private void ProcessScript(string[] lines)
        {
            estado = state.Title;
            for (int i=0;i<lines.Length;i++)
            {
                currentLine = i;
                command c;

                string param;
                if(lines[i].Trim()=="")
                {
                    continue;
                }
                else if (IsComment(lines[i]))
                {
                    continue;
                }
                else if (command.None != (c = ComandInLine(lines[i], out param)))
                {
                    ProccesComand(c,param);           

                }
                else if (IsFordStatement(lines[i]))
                {
                    continue;
                }
                else if (IsAdress(lines[i]))
                {
                    continue;
                }
            }
          
        }


        public string GetSimbolico(string[] lines)
        {

            ProcessScript(lines);
            return (StringSimbolico());

        }


        private bool IsFordStatement(string line)
        {
            line = line.Trim();
            bool value = false;
            string[] str=new string[3];
            int NBytes;
            int  IByte;
            string corchetes1;
            string corchetes2;
            string address;
            string Byte;
            string bit;
            //Regex tagParser = new Regex(@"^(For\s+\(?\s*\d+\s*\,?\s*\d+\s*\)?\s*\[?.+,.+\]?)?");
            //if (tagParser.Matches(line).Count>0)
             if(line.StartsWith("For"))
            {
                //(1  ,   3424  ) [12Hkjifsda, kajjjasfd]
                value = true;
                string parentesis=line.Split(new char[]{'(',')'})[1];
                string corchetes = line.Split(new char[] { '[',']' })[1];
                
                
                if (!int.TryParse(parentesis.Split(new char[]{','})[0],out IByte))
                      throwException("El primer parámetro de la instrucción For no es correcto");

                if (!int.TryParse(parentesis.Split(new char[] { ',' })[1], out NBytes))
                    throwException("El segundo parámetro de la instrucción For no es correcto");

                corchetes1 = corchetes.Split(new char[] {','})[0];
                corchetes2 = corchetes.Split(new char[] { ',' })[1];

                for(int i=0;i<NBytes;i++)
                    for (int j=0;j<8;j++)
                {
                    str[0]=(IByte+i).ToString() + "." + j.ToString();
                    Byte = (IByte+i+ 1).ToString();
                    string Byte2 = (Byte.Length > 1) ? Byte :"0" + Byte;
                    bit = (j + 1).ToString();
                    
                    str[1]=corchetes1.Replace("&B", Byte2).Replace("&bit",bit);
                    str[2] = corchetes2.Replace("&B", Byte).Replace("&bit", bit);

                    this.ProcessAdrress(str);


                }
            
            }



            return value;

        }
       
        private string StringSimbolico()
        {
           
            Device.SimbCom sC=new Device.SimbCom("","");
            string txt = Title+"\n";
            string address = "";
            string simbText = "";
            Dictionary<string, string> simbolicosAddress = new Dictionary<string, string>();
            string tagSimbolico = "";
            for(int i=0;i<255;i++)
            {
                for (int j=0;j<8;j++)
                {
                    address = "I" + i.ToString() + "." + j.ToString();
                    simbText = "Input" + i.ToString() + "." + j.ToString();
                    sC = new Device.SimbCom(simbText, simbText);
                    tagSimbolico = "";
                    foreach (Device dev in Devices)
                    {
                        
                        foreach (var input in dev.Inputs)
                        {

                            if (input.ContainsKey(address))
                            {
                                if (tagSimbolico!="")
                                {
                                    throw new Exception("Error: los textos simbólicos " + tagSimbolico + " y " +
                                                        input[address].simbolico + " tienen la misma dirección profibus asociada: " + address);
                                }
                                else
                                {
                                    sC = input[address];
                                    tagSimbolico = sC.simbolico;
                                }
                            }
                        }
                    }

                    CheckSimbolicRepetition(address, sC.simbolico, simbolicosAddress);  
                   
                    txt+=address+";"+sC.simbolico+";"+sC.comentario+"\n";
                }

              
               
            }

            for(int i=0;i<255;i++)
            {
                for (int j=0;j<8;j++)
                {
                    address = "O" + i.ToString() + "." + j.ToString();
                    simbText = "Output" + i.ToString() + "." + j.ToString();
                    sC = new Device.SimbCom(simbText, simbText);
                    tagSimbolico = "";
                    foreach (Device dev in Devices){
                      
                        foreach (var output in dev.Outputs)
                        {
                                                        
                           if (output.ContainsKey(address))
                                if (tagSimbolico !="")
                                {
                                    throw new Exception("Error: los textos simbólicos " + tagSimbolico + " y " +
                                                       output[address].simbolico + " tienen la misma dirección profibus asociada: " + address);
                                }
                                else
                                {
                                    sC = output[address];
                                    tagSimbolico = sC.simbolico;
                                }
                            }
                        }

                    CheckSimbolicRepetition(address, sC.simbolico, simbolicosAddress);
                    txt += address + ";" + sC.simbolico + ";" + sC.comentario + "\n";


                    }

                   

                }

            return txt;
            
        }

        private void CheckSimbolicRepetition(string address,string sCSimbolico,Dictionary<string,string> simbolicosAddress){
              if (simbolicosAddress.ContainsKey(sCSimbolico))
                    {
                        throw new Exception("Error: las direcirecciones de profibus " + simbolicosAddress[sCSimbolico] + " y " +
                                            address + " tienen el mismo simbólico asociado: " + sCSimbolico);
                    }
                    simbolicosAddress.Add(sCSimbolico, address);

        }

        

       

        private bool IsComment(string line)
        {
            bool value = false;
            line = line.Trim();
          
            if (line.StartsWith("*"))
                value = true;

            return value;
        }

        private bool IsAdress(string line)
        {
            string[] str = line.Trim().Split(new char[] {';'});
            bool value = false;
            double number=0.0;
           
            if (str.Length==3)
                if (double.TryParse(str[0],out number))
                {
                    if ((estado & state.ImputAddress) == 0 && ((estado & state.OutputAddress) == 0))
                    {
                        throwException("No se esperaba una especificación de señal de profibus");
                    }
                    value = true;
                    try
                    {
                        ProcessAdrress(str);
                    }
                    catch (Exception e)
                    {
                        throwException(e);
                    }
                }
                else
                {
                    sigNalProfibusExpectedExceptcion();
                }
            else
            {
                sigNalProfibusExpectedExceptcion();
            }     
       

            return value;
        }

        private void sigNalProfibusExpectedExceptcion()
        {
            if ((estado & state.ImputAddress) != 0)
                throwException("Se esperaba la especificación una entrada de profibus");
            else if ((estado & state.OutputAddress) != 0)
                throwException("Se esperaba la especificación de una salida de profibus");
            else
                throwException("Se esperaba la especificación una señal de profibus");
        }

        private void ProcessAdrress(string[] str)
        {
            if ((estado & state.ImputAddress) != 0)
            {
                currentDevice.setInputs(str);
            }
            else if ((estado & state.OutputAddress) != 0)
            {
                currentDevice.setOutputs(str);
            }
            else
                throwException("No se esperaba la especificación de una dirección profibus");
        } 

        private void ProccesComand(command c,string p)
        {
            if (c == command.Title)
                ProccesTitle(p);
            else if (c == command.Device)
                ProccesDevice(p);
            else if (c == command.EndDevice) 
                ProccesEndDeviece();
            else if (c==command.Inputs)
                ProccesInputs(p);
            else if (c==command.Outputs)
                ProccesOutputs(p);
            else
            {
                throwException("Comando no encontrado");
            }
          
              
              
        }
       
        private void ProccesTitle(string p)
        {
            if (estado==state.Title)
            {
                Title = ";"+p;
                estado = state.Device;
            }
            else
            {
                throwException("Comando <Title> no esperado");
            }
           
        }
       
        private void ProccesDevice(string p)
        {
            if (estado==state.Device)
            {
                currentDevice = new Device(p);
                Devices.Add(currentDevice);
                estado = state.EndDevice | state.Imputs | state.Outpus;
            }
            else
            {
                throwException("Comando <Device> no esperado");
            }
           
        }

        private void ProccesEndDeviece()
        {
            if ((estado&state.EndDevice)!=0)
            {
                estado = state.Device;

            }
            else
            {
                throwException("Comando <EndDevice> no esperado");
            }
            

        }

        private void ProccesInputs(String param)
        {
            if ((estado & state.Imputs) != 0)
            {
                currentDevice.Inputs.Add(new Dictionary<string, Device.SimbCom>());
                string[] str = ExtactCommnent(param);
                int start;
                if (int.TryParse(str[0],out start))
                    currentDevice.StartInputs.Add(str[0]);
                else
                    throwException( String.Format("Dirección absoluta de entradas: {0}, inválida",str[0]));

                currentDevice.InputsComent.Add(str[1]);
                estado = state.ImputAddress | state.Imputs| state.Outpus | state.EndDevice;
            }
            else
            {
                throwException( "Comando <Imputs> no esperado");
            }
        }

        private void ProccesOutputs(String param)
        {
            if ((estado & state.Outpus) != 0)
            {
                currentDevice.Outputs.Add(new Dictionary<string, Device.SimbCom>());
                string[] str = ExtactCommnent(param);
                int start;
                if (int.TryParse(str[0], out start))
                    currentDevice.StartInputs.Add(str[0]);
                else
                    throwException(String.Format("Dirección absoluta de salidas: {0}, inválida", str[0]));

                currentDevice.StartOutputs.Add(str[0]);
                currentDevice.OutputsComent.Add(str[1]);
                estado = state.OutputAddress | state.Outpus | state.Imputs | state.EndDevice;
            }
            else
            {
                throwException("Comando <Outputs> no esperado");
            }
        }

        private string[] ExtactCommnent(string param)
        {
            string[] value = new string[2];
            string[] str = param.Split(new char[] { ',' });            

            if (str.Length == 2)
                value = str;
            else
            {
                value[0] = str[0];
                value[1] = "";
            }
            return value;
        }


        public command ComandInLine(string line,out string param)
        {
            command value = command.None;
            param = "";
            if (line.IndexOf("<") != -1 & line.IndexOf(">") != -1){
                if (line.IndexOf("<") < line.IndexOf(">"))
                {
                    string str = line.Substring(line.IndexOf("<") + 1, line.IndexOf(">") - line.IndexOf("<") - 1);
                    str = str.Trim();
                    string[] text = str.Split(new char[] { '(', ')' });
                    string comand = text[0].Trim().ToUpper();
                  
                       

                    switch (comand)
                    {

                        case "TITLE":
                            value = command.Title;
                            param = checkParameter(value, text);
                            break;
                        case "DEVICE":
                            value = command.Device;
                            param = checkParameter(value, text);
                            break;
                        case "ENDDEVICE":
                            value = command.EndDevice;
                            break;
                        case "INPUTS":
                            value = command.Inputs;
                            param = checkParameter(value, text);
                            break;
                        case "OUTPUTS":
                            value = command.Outputs;
                            param = checkParameter(value, text);
                            break;

                        default:
                            value=command.Desconocido;
                            break;
                        

                    }

                    if (value==command.Desconocido)
                    {

                        throwException("Comando especificado no encontrado");
                    }   

                }
               

            }

            return value;
          
        }

        private string checkParameter(command c,string[] str)
        {
            string parametro = "";
            string msg = String.Format("El comando {0} debe especificar un parámetro", c);
            if (str.Length > 1)
                if (str[1].Trim()=="")
                    throwException(msg);
                else
                    parametro = str[1].Trim();
            else
                throwException(msg);

            return parametro;
        }

        private void throwException(){

            throw new Exception("Error en línea: " +(currentLine+1).ToString());

        }

        private void throwException(string comentario)
        {

            throw new Exception("Error en línea: " + (currentLine + 1).ToString() +"; "+ comentario);
           
        }

        private void throwException(string linea,string comentario)
        {

            throw new Exception("Error en línea :" + linea+"; "+comentario);

        }

        private void throwException(Exception e) {
            throwException(e.Message);
        }


    }
}