using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Idpsa.PassportExtensions;
using RCMCommonTypes;

namespace Idpsa.Paletizado
{
    [Serializable]
    public abstract class Zebra_CajasPrinter
    {
        public const int BarcodeLength = 18;
        private const int BarcodeBoxIdLength = 11;
        private readonly string _name;

        private readonly Dictionary<string, string> _tildes = new Dictionary<string, string>
                                                                  {
                                                                      {"Á", "A"},
                                                                      {"É", "E"},
                                                                      {"Í", "I"},
                                                                      {"Ú", @"\E9"},
                                                                      {"Ó", @"\E3"},
                                                                      {"á", "a"},
                                                                      {"é", "e"},
                                                                      {"ú", "u"}
                                                                  };

//{ { "Á", "A" }, { "É", "E" }, { "Í", "I" }, { "Ú", "U" }, { "Ó", "O" } };  //En Unicode con tilde sería: Á=C1, É=C9, Í=CD,Ó=D3, Ú=DA.
        private readonly Dictionary<string, string> _tildes2 = new Dictionary<string, string>
                                                                   {
                                                                       {"Ú", @"\E9"},
                                                                       {"Ó", @"\E3"},
                                                                       {"í", @"\A1"},
                                                                       {"ó", @"\A2"}
                                                                   };

        protected BasicFields BasFields;
        private Dictionary<int, string> _formatoZpl_Templates;

        //private char Ñ='¥';
        private string Ñ = "\\A5";
        private string ñ = "\\A4";

        protected Zebra_CajasPrinter(string name)
        {
            SetZPLTemplates();
            var pd = new PrintDialog();
            pd.PrinterSettings = new PrinterSettings();
            PrinterName = pd.PrinterSettings.PrinterName;
            _name = name;
        }

        protected Zebra_CajasPrinter(string name, string printerName)
        {
            SetZPLTemplates();
            PrinterName = printerName;
            _name = name;
        }

        protected string PrinterName { get; private set; }


        public void Print(CajaPasaportes caja, bool excepcionCaja99Pasaportes)
            //También habrá que pasarle la impresora que está usando e incluirla en para saber el formato de impresión que necesitamos.
        {
            string zplCommand = GetPrintString(GetLabelFormat(caja), caja, excepcionCaja99Pasaportes);
                //En GetLabelFormat tendrás que especificar la impresora con la que vas a imprimir.
            PrintString(zplCommand);
        }

        private int GetLabelFormat(CajaPasaportes caja)
        {
            if (PrinterName == "ZebraTLP2844")
            {
                if (caja.TipoPasaporte.TieneFechaDeLaminacion
                    ||( caja.TipoPasaporte.Country.Name == "España"))//mcr. 2016&&(caja.TipoPasaporte.Type==TipoPasaporte.Types.Normal||caja.TipoPasaporte.Type==TipoPasaporte.Types.Consular)))
                    return 7;
                else
                    return 6;
            }
            else
            {
                // MCR.2011-02-11.


                if (caja.TipoPasaporte.TieneFechaDeLaminacion
                    || (caja.TipoPasaporte.Country.Name == "España"))//&& (caja.TipoPasaporte.Type != TipoPasaporte.Types.Maritimo) && (caja.TipoPasaporte.Type != TipoPasaporte.Types.Provisional))//MCR. 2016 &&(caja.TipoPasaporte.Type==TipoPasaporte.Types.Normal||caja.TipoPasaporte.Type==TipoPasaporte.Types.Consular)))
                    return 5; //MDG.2010-07-14.Nueva plantilla de etiqueta//3;
                else
                    return 4;
            }
        }

        protected abstract void PrintString(string text);

        public override string ToString()
        {
            return _name;
        }

        public string CalculateBarcode(CajaPasaportes caja)
        {
            string pais = caja.TipoPasaporte.Country.Code.ToString().PadLeft(3, '0');
            string tipo = caja.TipoPasaporte.Type.NumericValue().ToString();
            string Rfid = caja.TipoPasaporte.RfIdType.NumericValue().ToString();
            //string Id = stringToNumber(caja.Id).PadLeft(12, '0');   
            string Id = caja.Id.PadLeft(BarcodeBoxIdLength, '0');
            string nCharsId = caja.TipoPasaporte.Length.ToString().Substring(0, 1);
            string CheckSum = CalculateSecurityNumber(pais + tipo + Rfid + Id).ToString();
            return (pais + tipo + Rfid + Id + nCharsId + CheckSum);
        }

        public bool TryExtractDataOfBarcode(string barcode, out BarcodeData barcodeData, out string errorMessage)
        {
            errorMessage = null;
            barcodeData = null;

            if (barcode.Length != BarcodeLength)
            {
                errorMessage = "error numero de caracteres";
                return false;
            }

            int countryCode;
            Country country;
            if (!(int.TryParse(barcode.Substring(0, 3), out countryCode)
                  && ((country = Country.GetCountry(countryCode)) != null)))
            {
                errorMessage = "error codigo pais";
                return false;
            }

            int tipoCode;
            TipoPasaporte.Types tipo;
            if (!(int.TryParse(barcode.Substring(3, 1), out tipoCode)
                  && (tipo = tipoCode.NumericValueToPasaportType()) != TipoPasaporte.Types.NotDefined))
            {
                errorMessage = "error codigo tipo pasaporte";
                return false;
            }

            int rfidCode;
            TipoPasaporte.TypeRfid rfid;
            if (!(int.TryParse(barcode.Substring(4, 1), out rfidCode)
                  && (rfid = tipoCode.NumericValueToTypeRfid()) != TipoPasaporte.TypeRfid.NoDefined))
            {
                errorMessage = "error codigo RFID";
                return false;
            }

            int lenghtIdCaja;

            if (!int.TryParse(barcode.Substring(5 + BarcodeBoxIdLength, 1), out lenghtIdCaja))
            {
                errorMessage = "error longitud código de caja";
                return false;
            }

            int length = BarcodeBoxIdLength - lenghtIdCaja;
            string idCaja = barcode.Substring(5 + length, lenghtIdCaja);

            int checkSum;
            if (!int.TryParse(barcode.ToCharArray().Last().ToString(), out checkSum))
            {
                errorMessage = "error CheckSum";
                return false;
            }

            barcodeData =
                new BarcodeData
                    {
                        Pais = country,
                        Rfid = rfid,
                        IdCaja = idCaja,
                        CheckSum = checkSum,
                        Tipo = tipo
                    };

            return true;
        }


        private string Center(string s)
        {
            int center = 15;
            int spaces = center - (int) Math.Round((s.Length + 0.2)/2);

            if (spaces > 0)
                s = s.Insert(0, "".PadLeft(spaces, ' '));
            s = s.PadRight(2*center);

            return s;
        }

        private string stringToNumber(string s)
        {
            var so = new StringBuilder();
            s = s.ToLower();

            foreach (char c in s)
            {
                if (c >= 'a' && c <= 'z')
                {
                    so.Append((c - 'a' + 10).ToString().PadLeft(2, '0'));
                }
                else if (c >= '0' && c <= '9')
                {
                    so.Append(c);
                }
            }

            return so.ToString();
        }


        private int CalculateSecurityNumber(string s)
        {
            int sum = 0;
            foreach (char c in s)
            {
                if (char.IsDigit(c))
                {
                    sum += int.Parse(c.ToString());
                }
            }

            return (9 - (sum%10)); //return (10-(sum % 10));
        }


        protected string GetPrintString(int formato, CajaPasaportes caja, bool excepcionCaja99Pasaportes)
        {
            string template = _formatoZpl_Templates[formato]; //MCR.Lo que había antes del caos.
            SetText(caja, excepcionCaja99Pasaportes);//MDG.2012-11-28

            string zplCommand = template.Replace(LabelTokens.Responsable, SetFormat(BasFields.Responsable));
            zplCommand = zplCommand.Replace(LabelTokens.Destinatario, SetFormat(BasFields.Destinatario));
            //zplCommand = zplCommand.Replace(LabelTokens.Contenido, ((CajaPasaportes.NMaxPasaportes.ToString() + SetFormat(" LIBRETAS DE PASAPORTE") + (caja.TipoPasaporte.HasRfid ? "-e" : "") )));
            zplCommand = zplCommand.Replace(LabelTokens.Contenido, (SetFormat(BasFields.Contenido)));
            //zplCommand = zplCommand.Replace(LabelTokens.Rango,(String.Format("Del {1} Al {2}", caja.NSerie, (caja.NCaja + 1).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'), (caja.NCaja + CajaPasaportes.NMaxPasaportes).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'))).ToUpper());
            //zplCommand = zplCommand.Replace(LabelTokens.Rango, (String.Format("Del {0} {1} Al {0} {2}", caja.NSerie, (caja.NCaja - CajaPasaportes.NMaxPasaportes+1).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'), (caja.NCaja).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'))));
            zplCommand = zplCommand.Replace(LabelTokens.Rango, SetFormat(BasFields.Rango));
            //       zplCommand = zplCommand.Replace(LabelTokens.FechaLaminacion, (String.Format(@"Fecha l\A1mite de laminaci\A2n: *{0}", "07/2012")));
            zplCommand = zplCommand.Replace(LabelTokens.FechaLaminacion, (SetFormat(BasFields.FechaLaminacion)));
            //zplCommand = zplCommand.Replace(LabelTokens.NumeroCaja, (String.Format("CAJA N\\A7 {0}", (caja.NCaja - 1 + CajaPasaportes.NMaxPasaportes).ToString().Substring(0,caja.TipoPasaporte.NDigits - 2))));//.PadLeft(caja.TipoPasaporte.NDigits - 2, '0'))));//Sumamos 99 al numero de pasaporte,le quitamos las 2 últimas cifras y obtenemos el nímero de caja
            zplCommand = zplCommand.Replace(LabelTokens.NumeroCaja, (SetFormat(BasFields.NumeroCaja)));
            zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, SetFormat(BasFields.TipoOpcional));
            return zplCommand.Replace(LabelTokens.CodigoBarras, CalculateBarcode(caja));


/*            string template = _formatoZpl_Templates[formato];
            if (formato == 6 || formato == 7)
            {
                var zplCommand = template.Replace(LabelTokens.Responsable, SetFormat(caja.TipoPasaporte.Responsable));
                zplCommand = zplCommand.Replace(LabelTokens.Destinatario, SetFormat(caja.TipoPasaporte.Destinatario));
                //zplCommand = zplCommand.Replace(LabelTokens.Contenido, ((CajaPasaportes.NMaxPasaportes.ToString() + SetFormat(" LIBRETAS DE PASAPORTE") + (caja.TipoPasaporte.HasRfid ? "-e" : "") )));
                zplCommand = zplCommand.Replace(LabelTokens.Contenido, (SetFormat(caja.TipoPasaporte.Country.ToString() == "Panama" ? "200 DOCUMENTOS MARITIMOS" : "100 LIBRETAS DE PASAPORTE") + (caja.TipoPasaporte.HasRfid ? "-e" : "")));
                //zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("PASAPORTE {0} {1}", SetFormat(caja.TipoPasaporte.Country.ToString()), SetFormat(caja.TipoPasaporte.Type.ConvertToString()))));
                //zplCommand = zplCommand.Replace(LabelTokens.Rango,(String.Format("Del {1} Al {2}", caja.NSerie, (caja.NCaja + 1).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'), (caja.NCaja + CajaPasaportes.NMaxPasaportes).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'))).ToUpper());
                //zplCommand = zplCommand.Replace(LabelTokens.Rango, (String.Format("Del {0} {1} Al {0} {2}", caja.NSerie, (caja.NCaja - CajaPasaportes.NMaxPasaportes+1).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'), (caja.NCaja).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'))));
                if (caja.fechaLam != null)//MDG.2011-03-23
                {
                    zplCommand = zplCommand.Replace(LabelTokens.Rango, (String.Format("Del {0} {1} Al {0} {2}", caja.NSerie, (caja.NCaja - (caja.TipoPasaporte.Country.ToString() == "Panama" ? 200 : CajaPasaportes.NMaxPasaportes) + 1).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'), (caja.NCaja).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'))));
                }
                zplCommand = zplCommand.Replace(LabelTokens.FechaLaminacion, (String.Format(@"Fecha lÍmite de laminaciÓn: *{0}", SetFormat(caja.fechaLam))));  //MCR 2011-03-02
                //zplCommand = zplCommand.Replace(LabelTokens.NumeroCaja, (String.Format("CAJA N\\A7 {0}", (caja.NCaja - 1 + CajaPasaportes.NMaxPasaportes).ToString().Substring(0,caja.TipoPasaporte.NDigits - 2))));//.PadLeft(caja.TipoPasaporte.NDigits - 2, '0'))));//Sumamos 99 al numero de pasaporte,le quitamos las 2 últimas cifras y obtenemos el nímero de caja
                zplCommand = zplCommand.Replace(LabelTokens.NumeroCaja, (String.Format("CAJA Nº {0}", (caja.NCaja - 1 + CajaPasaportes.NMaxPasaportes).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0').Substring(0, caja.TipoPasaporte.NDigits - 2))));//.PadLeft(caja.TipoPasaporte.NDigits - 2, '0'))));//Sumamos 99 al numero de pasaporte,le quitamos las 2 últimas cifras y obtenemos el nímero de caja
                zplCommand = zplCommand.Replace(Ñ, "Ñ"); //MCR2011-02-23

                if ((caja.TipoPasaporte.NombrePasaporte != "") && (caja.TipoPasaporte.NombrePasaporte != null))
                {   //MDG.2011-03-17.Usamos nuevo campo de texto variable
                    zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, SetFormat(caja.TipoPasaporte.NombrePasaporte));
                }
                else
                {   //antigua asignacion//MDG.2001-03-22.Ñapa para pasaporte Dominicana Diplomatico
                    if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Militar)
                        //zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("PASAPORTE {0}", SetFormat(caja.TipoPasaporte.Type.ConvertToString()))));
                        //ñapa para.quitar
                        zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("DIPLOMATICO")));
                    else
                        //antigua asignacion
                        if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Consular)
                            //zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("PASAPORTE {0}", SetFormat(caja.TipoPasaporte.Type.ConvertToString()))));
                            //ñapa para.quitar
                            zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("OFICIALES")));
                        else
                            if (caja.TipoPasaporte.Country.ToString() == "España")//MDG.2011-03-23
                                zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("PASAPORTE {0} {1}", SetFormat(caja.TipoPasaporte.Country.ToString()), SetFormat(caja.TipoPasaporte.Type.ConvertToString()))));
                            else
                                zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format(""))); //MDG.2011-03-23 para el resto de pasaportes no escribimos nada
                }
                
                return zplCommand.Replace(LabelTokens.CodigoBarras, CalculateBarcode(caja));
            }
            else
            {
                var zplCommand = template.Replace(LabelTokens.Responsable, SetFormat(caja.TipoPasaporte.Responsable));
                zplCommand = zplCommand.Replace(LabelTokens.Destinatario, SetFormat(caja.TipoPasaporte.Destinatario));
                //zplCommand = zplCommand.Replace(LabelTokens.Contenido, ((CajaPasaportes.NMaxPasaportes.ToString() + SetFormat(" LIBRETAS DE PASAPORTE") + (caja.TipoPasaporte.HasRfid ? "-e" : "") )));
                zplCommand = zplCommand.Replace(LabelTokens.Contenido, (SetFormat(caja.TipoPasaporte.Country.ToString() == "Panama" ? "200 DOCUMENTOS MARITIMOS" : "100 LIBRETAS DE PASAPORTE") + (caja.TipoPasaporte.HasRfid ? "-e" : "")));
                //zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("PASAPORTE {0} {1}", SetFormat(caja.TipoPasaporte.Country.ToString()), SetFormat(caja.TipoPasaporte.Type.ConvertToString()))));
                //zplCommand = zplCommand.Replace(LabelTokens.Rango,(String.Format("Del {1} Al {2}", caja.NSerie, (caja.NCaja + 1).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'), (caja.NCaja + CajaPasaportes.NMaxPasaportes).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'))).ToUpper());
                //zplCommand = zplCommand.Replace(LabelTokens.Rango, (String.Format("Del {0} {1} Al {0} {2}", caja.NSerie, (caja.NCaja - CajaPasaportes.NMaxPasaportes+1).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'), (caja.NCaja).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'))));
                zplCommand = zplCommand.Replace(LabelTokens.Rango, (String.Format("Del {0} {1} Al {0} {2}", caja.NSerie, (caja.NCaja - (caja.TipoPasaporte.Country.ToString() == "Panama" ? 200 : CajaPasaportes.NMaxPasaportes) + 1).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'), (caja.NCaja).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'))));
                if (caja.fechaLam!=null)
                {
                    zplCommand = zplCommand.Replace(LabelTokens.FechaLaminacion, (String.Format(@"Fecha l\A1mite de laminaci\A2n: *{0}", SetFormat(caja.fechaLam.ToString()))));  //MCR 2011-03-02
                }
                //zplCommand = zplCommand.Replace(LabelTokens.NumeroCaja, (String.Format("CAJA N\\A7 {0}", (caja.NCaja - 1 + CajaPasaportes.NMaxPasaportes).ToString().Substring(0,caja.TipoPasaporte.NDigits - 2))));//.PadLeft(caja.TipoPasaporte.NDigits - 2, '0'))));//Sumamos 99 al numero de pasaporte,le quitamos las 2 últimas cifras y obtenemos el nímero de caja
                zplCommand = zplCommand.Replace(LabelTokens.NumeroCaja, (String.Format("CAJA N\\A7 {0}", (caja.NCaja - 1 + CajaPasaportes.NMaxPasaportes).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0').Substring(0, caja.TipoPasaporte.NDigits - 2))));//.PadLeft(caja.TipoPasaporte.NDigits - 2, '0'))));//Sumamos 99 al numero de pasaporte,le quitamos las 2 últimas cifras y obtenemos el nímero de caja
                
                if ((caja.TipoPasaporte.NombrePasaporte != "") && (caja.TipoPasaporte.NombrePasaporte != null))
                {   //MDG.2011-03-17.Usamos nuevo campo de texto variable
                    zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, SetFormat(caja.TipoPasaporte.NombrePasaporte));
                }
                else
                {   
                    //MDG.2001-03-22.Ñapa para pasaporte Dominicana Diplomatico
                    if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Militar)
                        //zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("PASAPORTE {0}", SetFormat(caja.TipoPasaporte.Type.ConvertToString()))));
                        //ñapa para.quitar
                        zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("DIPLOMATICO")));
                    else
                    {
                        //antigua asignacion//MDG.2001-03-22.Ñapa para pasaporte Dominicana Diplomatico
                        if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Militar)
                            //zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("PASAPORTE {0}", SetFormat(caja.TipoPasaporte.Type.ConvertToString()))));
                            //ñapa para.quitar
                            zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("DIPLOMATICO")));
                        else
                            //antigua asignacion
                            if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Consular)
                                //zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("PASAPORTE {0}", SetFormat(caja.TipoPasaporte.Type.ConvertToString()))));
                                //ñapa para.quitar
                                zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("OFICIALES")));
                            else
                                if (caja.TipoPasaporte.Country.ToString() == "España")//MDG.2011-03-23
                                    zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format("PASAPORTE {0} {1}", SetFormat(caja.TipoPasaporte.Country.ToString()), SetFormat(caja.TipoPasaporte.Type.ConvertToString()))));
                                else
                                    zplCommand = zplCommand.Replace(LabelTokens.TipoOpcional, (String.Format(""))); //MDG.2011-03-23 para el resto de pasaportes no escribimos nada
                    }
                }

                return zplCommand.Replace(LabelTokens.CodigoBarras, CalculateBarcode(caja));
            }*/
            //MCR.Es una locura y está muy desorganizado. Mejor meter los cambios en otro método y dejar este igual a antes.
        }

        private string SetFormat(string text)
        {
            //text = text.ToUpper();

            /*foreach (var tilde in _tildes)
            {
                text = text.Replace(tilde.Key, tilde.Value);
            }

            text = text.Replace("Ñ", Ñ);
            text = text.Replace("ñ", ñ);
            text = text.Replace("º", "\\A7");

            text = text.ToUpper();//MDG.2010-12-13.Para que aparezca en mayusculas el texto de Panama*/

            if (_name == "Etiquetadora manual")
            {
                text = text.Replace(Ñ, "Ñ");
                text = text.Replace(ñ, "ñ");
                text = text.Replace("\\A7", "º");

                // text = text.ToUpper();MCR. Lo cambio a mayúsculas sólo donde se necesita.

                foreach (var tilde in _tildes2)
                {
                    text = text.Replace(tilde.Value, tilde.Key);
                } //MCR.2011-02-11.

                return text;
            }
            else
            {
                text = text.Replace("Ñ", Ñ);
                text = text.Replace("ñ", ñ);
                text = text.Replace("º", "\\A7");

                //    text = text.ToUpper();    //MCR. Lo cambio a mayúsculas en zplTemplate para que tb cambie el formato de las minúsculas.

                foreach (var tilde in _tildes)
                {
                    text = text.Replace(tilde.Key, tilde.Value);
                } //MCR.2011-02-11.

                return text;
            }
        }

        protected void SetText(CajaPasaportes caja, bool ExcepcionCaja99Pasaportes)
        {
            if (caja.TipoPasaporte.Country.Name == "España")
            {
                //if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Normal || MCR.Mod Impresión 2016
                //    caja.TipoPasaporte.Type == TipoPasaporte.Types.Subsidiario)
                //    BasFields.Responsable = "MINISTERIO DEL INTERIOR";
                //else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.TituloViaje |
                //         caja.TipoPasaporte.Type == TipoPasaporte.Types.Servicio |
                //         caja.TipoPasaporte.Type == TipoPasaporte.Types.Apatridas |
                //         caja.TipoPasaporte.Type == TipoPasaporte.Types.Diplomatico |
                //         caja.TipoPasaporte.Type == TipoPasaporte.Types.Refugiados |
                //         caja.TipoPasaporte.Type == TipoPasaporte.Types.Consular)
                //    BasFields.Responsable = "MINISTERIO DE ASUNTOS EXTERIORES";
                if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Normal ||
                    caja.TipoPasaporte.Type == TipoPasaporte.Types.Refugiados |
                    caja.TipoPasaporte.Type == TipoPasaporte.Types.Apatridas |
                    caja.TipoPasaporte.Type == TipoPasaporte.Types.TituloViaje |
                    caja.TipoPasaporte.Type == TipoPasaporte.Types.Subsidiario)
                    BasFields.Responsable = "MINISTERIO DEL INTERIOR";
                else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Servicio |
                         caja.TipoPasaporte.Type == TipoPasaporte.Types.Diplomatico |
                         caja.TipoPasaporte.Type == TipoPasaporte.Types.Consular|
                         caja.TipoPasaporte.Type == TipoPasaporte.Types.Provisional)
                    BasFields.Responsable = "MINISTERIO DE ASUNTOS EXTERIORES";
                else if ((caja.NSerie.StartsWith("P")&&caja.TipoPasaporte.NChars==3)||
                    caja.NSerie.Contains("ZAB")||
                    caja.NSerie.Contains("DVT")||
                    caja.NSerie.Contains("DVR")||
                    caja.NSerie.Contains("ZVR")||
                    caja.NSerie.Contains("DVA")||
                    caja.NSerie.Contains("DVP") ||
                    caja.NSerie.Contains("ZVP") ||
                    caja.NSerie.Contains("LM") ||
                    caja.NSerie.Contains("ZVA"))
                    BasFields.Responsable = "MINISTERIO DEL INTERIOR";
                else if (caja.NSerie.Contains("XDC")||
                    caja.NSerie.Contains("ZZB")||
                    (caja.NSerie.Substring(0, caja.TipoPasaporte.NChars).Equals("PA") || caja.NSerie.Equals(" PA")) ||
                    caja.NSerie.Contains("XF")||
                    caja.NSerie.Contains("XG"))
                    BasFields.Responsable = "MINISTERIO DE ASUNTOS EXTERIORES";
                else 
                    BasFields.Responsable = SetFormat(caja.TipoPasaporte.Responsable.ToUpper());
               
                if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Normal)
                    BasFields.Destinatario = "DIRECCIÓN GENERAL DE LA POLICIA";
                else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Servicio |
                         caja.TipoPasaporte.Type == TipoPasaporte.Types.TituloViaje |
                         caja.TipoPasaporte.Type == TipoPasaporte.Types.Consular |
                         caja.TipoPasaporte.Type == TipoPasaporte.Types.Diplomatico)
                {
                    if (ExcepcionCaja99Pasaportes)
                    {
                        BasFields.Destinatario = "99 LIBRETAS DE PASAPORTE-e";
                    }
                    else
                    {
                        BasFields.Destinatario = "100 LIBRETAS DE PASAPORTE-e";
                    }
                }
                else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Apatridas |
                         caja.TipoPasaporte.Type == TipoPasaporte.Types.Refugiados |
                         caja.TipoPasaporte.Type == TipoPasaporte.Types.Subsidiario)
                {
                    if (ExcepcionCaja99Pasaportes)
                    {
                        BasFields.Destinatario = "99 DOCUMENTOS DE VIAJE-e";
                    }
                    else
                    {
                        BasFields.Destinatario = "100 DOCUMENTOS DE VIAJE-e";
                    }
                }
                else
                    BasFields.Destinatario = SetFormat(caja.TipoPasaporte.Destinatario.ToUpper());

                if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Normal)
                {
                    if (ExcepcionCaja99Pasaportes)
                    {
                        BasFields.Contenido = "99 LIBRETAS DE PASAPORTES-e";
                    }
                    else
                    {
                        BasFields.Contenido = "100 LIBRETAS DE PASAPORTES-e";
                        
                    }
                }
                else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Apatridas)
                    BasFields.Contenido = "APATRIDAS";
                else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Consular)
                    BasFields.Contenido = "PARA EL SERVICIO CONSULAR";
                else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Diplomatico)
                    BasFields.Contenido = "ESPAÑOL DIPLOMATICO";
                else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Refugiados)
                    BasFields.Contenido = "REFUGIADOS";
                else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Servicio)
                    BasFields.Contenido = "ESPAÑOL DE SERVICIO";
                else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Subsidiario)
                    BasFields.Contenido = "PROTECCION SUBSIDIARIA";
                else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.TituloViaje)
                    BasFields.Contenido = "TITULO DE VIAJE";
                else
                    if (caja.TipoPasaporte.Country.ToString() == "Panama")
                    {
                        if (ExcepcionCaja99Pasaportes)
                        {
                            BasFields.Contenido = (SetFormat("199 DOCUMENTOS MARITIMOS") + (caja.TipoPasaporte.HasRfid ? "-e" : "")); ;
                        }
                        else
                        {
                            BasFields.Contenido = (SetFormat("200 DOCUMENTOS MARITIMOS") + (caja.TipoPasaporte.HasRfid ? "-e" : "")); ;
                        }
                    }
                    else
                    {
                        if (ExcepcionCaja99Pasaportes)
                        {
                            BasFields.Contenido = (SetFormat("99 LIBRETAS DE PASAPORTE") + (caja.TipoPasaporte.HasRfid ? "-e" : "")); ;
                        }
                        else
                        {
                            BasFields.Contenido = (SetFormat("100 LIBRETAS DE PASAPORTE") + (caja.TipoPasaporte.HasRfid ? "-e" : "")); ;
                        }
                    }

                    //BasFields.Contenido =
                    //    (SetFormat(caja.TipoPasaporte.Country.ToString() == "Panama"
                    //                   ? "200 DOCUMENTOS MARITIMOS"
                    //                   : "100 LIBRETAS DE PASAPORTE") + (caja.TipoPasaporte.HasRfid ? "-e" : ""));


                if (caja.TipoPasaporte.Type == TipoPasaporte.Types.TituloViaje |
                    caja.TipoPasaporte.Type == TipoPasaporte.Types.Servicio |
                    caja.TipoPasaporte.Type == TipoPasaporte.Types.Apatridas |
                    caja.TipoPasaporte.Type == TipoPasaporte.Types.Diplomatico |
                    caja.TipoPasaporte.Type == TipoPasaporte.Types.Refugiados |
                    caja.TipoPasaporte.Type == TipoPasaporte.Types.Subsidiario)
                {
                    if (ExcepcionCaja99Pasaportes)
                    {
                        BasFields.FechaLaminacion =
                            (String.Format("Del {0} {1} Al {0} {2}",
                            caja.NSerie,
                                           (caja.NCaja -
                                            (caja.TipoPasaporte.Country.ToString() == "Panama"
                                                 ? 199
                                                 : CajaPasaportes.NMaxPasaportes))
                                                 .ToString()
                                                 .PadLeft(caja.TipoPasaporte.NDigits, '0'),

                                           (caja.NCaja).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0')));
                    }
                    else
                    {
                        BasFields.FechaLaminacion =
                            (String.Format("Del {0} {1} Al {0} {2}", caja.NSerie,
                                           (caja.NCaja -
                                            (caja.TipoPasaporte.Country.ToString() == "Panama"
                                                 ? 200
                                                 : CajaPasaportes.NMaxPasaportes) + 1).ToString().PadLeft(
                                                     caja.TipoPasaporte.NDigits, '0'),
                                           (caja.NCaja).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0')));
                    }
                    if (caja.TipoPasaporte.TieneFechaDeLaminacion)
                        BasFields.NumeroCaja =
                        (SetFormat(String.Format(@"Fecha lÍmite de laminaciÓn: *{0}", caja.fechaLam)));
                    else
                    {
                        BasFields.NumeroCaja = "";
                    }
                }
                else
                {
                    if (ExcepcionCaja99Pasaportes)
                    {
                        BasFields.Rango =
                            (String.Format("Del {0} {1} Al {0} {2}", caja.NSerie,
                                           (caja.NCaja -
                                            (caja.TipoPasaporte.Country.ToString() == "Panama"
                                                 ? 199
                                                 : CajaPasaportes.NMaxPasaportes) + 1).ToString()
                                                 .PadLeft(caja.TipoPasaporte.NDigits, '0')
                                                 ,
                                           (caja.NCaja).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0')));
                    }
                    else
                    {
                        BasFields.Rango =
                            (String.Format("Del {0} {1} Al {0} {2}", caja.NSerie,
                                           (caja.NCaja -
                                            (caja.TipoPasaporte.Country.ToString() == "Panama"
                                                 ? 200
                                                 : CajaPasaportes.NMaxPasaportes) + 1).ToString().PadLeft(
                                                     caja.TipoPasaporte.NDigits, '0'),
                                           (caja.NCaja).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0')));
                        
                    }
                    if (caja.TipoPasaporte.TieneFechaDeLaminacion)
                        BasFields.FechaLaminacion =
                        (SetFormat(String.Format(@"Fecha lÍmite de laminaciÓn: *{0}", caja.fechaLam)));
                    else
                    {
                        BasFields.FechaLaminacion = "";
                    }
                    BasFields.NumeroCaja =
                        (SetFormat(String.Format("CAJA N\\A7 {0}",
                                                 (caja.NCaja - 1 + CajaPasaportes.NMaxPasaportes).ToString().PadLeft(
                                                     caja.TipoPasaporte.NDigits, '0').Substring(0,
                                                                                                caja.TipoPasaporte.
                                                                                                    NDigits - 2))));
                        //.PadLeft(caja.TipoPasaporte.NDigits - 2, '0'))));//Sumamos 99 al numero de pasaporte,le quitamos las 2 últimas cifras y obtenemos el nímero de caja
                }
                BasFields.TipoOpcional = "";
            }
            else
            {
                BasFields.Responsable = SetFormat(caja.TipoPasaporte.Responsable.ToUpper());
                BasFields.Destinatario = SetFormat(caja.TipoPasaporte.Destinatario.ToUpper());
                BasFields.Contenido =
                    (SetFormat(caja.TipoPasaporte.Country.ToString() == "Panama"
                                   ? "200 DOCUMENTOS MARITIMOS"
                                   : "100 LIBRETAS DE PASAPORTE") + (caja.TipoPasaporte.HasRfid ? "-e" : ""));

                //MDG.2011-04-26.Distincion casos especiales Rep. Dominicana
                if (caja.TipoPasaporte.Country.ToString() == "Panama")
                {
                    BasFields.TipoOpcional =
                        SetFormat(String.Format("PASAPORTE {0} {1}", (caja.TipoPasaporte.Country.ToString().ToUpper()),
                                                SetFormat(caja.TipoPasaporte.Type.ConvertToString().ToUpper())));
                }
                else //Rep Dominicana
                {
                    if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Diplomatico)
                        BasFields.TipoOpcional = "DIPLOMATICO";
                    else if (caja.TipoPasaporte.Type == TipoPasaporte.Types.Oficial)
                        BasFields.TipoOpcional = "OFICIALES";
                    else
                        BasFields.TipoOpcional = "";
                }
            }
            if (ExcepcionCaja99Pasaportes)
            {
                BasFields.Rango =
                    (
                        String.Format
                        (
                            "Del {0} {1} Al {0} {2}", caja.NSerie,
                            (
                                   caja.NCaja -
                                    (caja.TipoPasaporte.Country.ToString() == "Panama"
                                         ? 199
                                         : 99) +//CajaPasaportes.NMaxPasaportes) +
                                    1
                            )
                            .ToString()
                            .PadLeft(caja.TipoPasaporte.NDigits, '0')
                            
                            ,

                            (
                                caja.NCaja
                            )
                            .ToString()
                            .PadLeft
                            (
                                caja.TipoPasaporte.NDigits, '0'
                            )
                        )
                    );
            }
            else
            {
                BasFields.Rango =
                    (String.Format("Del {0} {1} Al {0} {2}", caja.NSerie,
                                   (caja.NCaja -
                                    (caja.TipoPasaporte.Country.ToString() == "Panama"
                                         ? 200
                                         : CajaPasaportes.NMaxPasaportes) +
                                    1).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0'),
                                   (caja.NCaja).ToString().PadLeft(caja.TipoPasaporte.NDigits, '0')));
            }
            if (caja.TipoPasaporte.TieneFechaDeLaminacion)
                BasFields.FechaLaminacion =
                (SetFormat(String.Format(@"Fecha lÍmite de laminaciÓn: *{0}", caja.fechaLam)));
            else
            {
                BasFields.FechaLaminacion = "";
            }

            if (ExcepcionCaja99Pasaportes)
            {
                BasFields.NumeroCaja =
                    (SetFormat(String.Format("CAJA N\\A7 {0}",
                                             (caja.NCaja - 1 + CajaPasaportes.NMaxPasaportes).ToString().PadLeft(
                                                 caja.TipoPasaporte.NDigits, '0').Substring(0,
                                                                                            caja.TipoPasaporte.NDigits -
                                                                                            1))));
            }
            else
            {
                BasFields.NumeroCaja =
                       (SetFormat(String.Format("CAJA N\\A7 {0}",
                                                (caja.NCaja - 1 + CajaPasaportes.NMaxPasaportes).ToString().PadLeft(
                                                    caja.TipoPasaporte.NDigits, '0').Substring(0,
                                                                                               caja.TipoPasaporte.NDigits -
                                                                                               2))));
                //.PadLeft(caja.TipoPasaporte.NDigits - 2, '0'))));//Sumamos 99 al numero de pasaporte,le quitamos las 2 últimas cifras y obtenemos el nímero de caja
                
            }

        }

        private void SetZPLTemplates()
        {
            _formatoZpl_Templates = new Dictionary<int, string>();
            string template0 =
                @"^XA~TA000~JSN^LT0^MMT^MNW^MTD^PON^PMN^LH0,0^JMA^PR4,4^MD0^JUS^LRN^CI0^XZ
                                ^XA^LL0607
                                ^PW807
                                ^FO16,266^GB774,329,8^FS
                                ^CI0^FT672,533^A0I,39,38^FH\^FD&tag1^FS
                                ^CI0^FT672,480^A0I,39,38^FH\^FD&tag2^FS
                                ^CI0^FT673,359^A0I,39,38^FH\^FD&tag4^FS
                                ^CI0^FT671,415^A0I,39,38^FH\^FD&tag3^FS
                                ^CI0^FT672,301^A0I,34,33^FH\^FD&tag5^FS
                                ^BY2,3,160^FT623,50^BCI,,Y,N
                                ^FD>:&CodigoBarras^FS
                                ^PQ1,0,1,Y^XZ";

            _formatoZpl_Templates.Add(0, template0);


            string template1 =
                @"^XA~TA000~JSN^LT0^MMT^MNW^MTD^PON^PMN^LH0,0^JMA^PR4,4^MD0^JUS^LRN^CI0^XZ
                                ^XA^LL1039
                                ^PW711
                                ^BY4,3,160^FT650,861^BCB,,Y,N
                                ^FD>:&CodigoBarras^FS
                                ^CI0^FT449,564^A0B,56,55^FH\^FD&tag5^FS
                                ^CI0^FT350,564^A0B,56,55^FH\^FD&tag4^FS
                                ^CI0^FT271,564^A0B,56,55^FH\^FD&tag3^FS
                                ^CI0^FT177,564^A0B,56,55^FH\^FD&tag2^FS
                                ^CI0^FT99,564^A0B,56,55^FH\^FD&tag1^FS
                                ^PQ1,0,1,Y^XZ";

            _formatoZpl_Templates.Add(1, template1);

            string template2 =
                @"^XA
                                ^PR4                                
                                ^BY4,3,160^FT630,950^BCB,,Y,N^FD>:&CodigoBarras^FS
                                ^CI0^FT429,850^A0B,56,55^FH\^FD&tag5^FS
                                ^CI0^FT330,850^A0B,56,55^FH\^FD&tag4^FS
                                ^CI0^FT251,850^A0B,56,55^FH\^FD&tag3^FS
                                ^CI0^FT157,850^A0B,56,55^FH\^FD&tag2^FS
                                ^CI0^FT79,850^A0B,56,55^FH\^FD&tag1^FS
                                ^PQ1,0,1,Y
                                ^XZ";

            _formatoZpl_Templates.Add(2, template2);

            string template3 =
                @"^XA
                                ^PR4 
                                ^BY3,3,97^FT628,845^BCB,,Y,N
                                ^FD>:&CodigoBarras^FS
                                ^CI0^FT384,910^A0B,34,33^FH\^FD&tag6^FS
                                ^CI0^FT342,910^A0B,42,38^FH\^FD&tag5^FS
                                ^CI0^FT274,910^A0B,40,45^FH\^FD&tag4^FS
                                ^CI0^FT222,910^A0B,52,45^FH\^FD&tag3^FS
                                ^CI0^FT146,910^A0B,48,45^FH\^FD&tag2^FS
                                ^CI0^FT90,910^A0B,48,45^FH\^FD&tag1^FS
                                ^FT443,910^AAB,18,10^FH\^FDNota.- Debe recontarse el paquete sin levantar el precinto. No se admitir\A0^FS
                                ^FT461,910^AAB,18,10^FH\^FD       ninguna reclamaci\A2n en los paquetes que hayan sido desprecintados^FS
                                ^FT500,910^A0B,23,24^FH\^FD* Siempre que se almacene en las condiciones recomendadas^FS                                
                                ^PQ1,0,1,Y
                                ^XZ";

            _formatoZpl_Templates.Add(3, template3);


//            string template4 = @"^XA
//                                ^PR4 
//                                ^BY3,3,97^FT628,845^BCB,,Y,N
//                                ^FD>:&CodigoBarras^FS                                
//                                ^CI0^FT382,910^ADB,40,24^FH\^FD&tag5^FS                               
//                                ^CI0^FT287,910^ADB,48,28^FH\^FD&tag3^FS
//                                ^CI0^FT190,910^ADB,40,24^FH\^FD&tag2^FS
//                                ^CI0^FT110,910^ADB,48,28^FH\^FD&tag1^FS
//                                ^FT458,910^ADB,24,14^FH\^FDNota.- Debe recontarse el paquete sin levantar el precinto.^FS
//                                ^FT476,910^ADB,24,14^FH\^FD       No se admitir\A0 ninguna reclamaci\A2n en los paquetes^FS       
//                                ^FT494,910^ADB,24,14^FH\^FD       que hayan sido desprecintados.^FS                                
//                                ^PQ1,0,1,Y
//                                ^PQ1,0,1,Y
//                                ^XZ";

            //MDG.2011-03-17
            string template4 =
                @"^XA
                                ^PR4 
                                ^BY3,3,97^FT628,845^BCB,,Y,N
                                ^FD>:&CodigoBarras^FS                                
                                ^CI0^FT382,910^ADB,40,24^FH\^FD&tag5^FS                                
                                ^CI0^FT317,910^ADB,48,28^FH\^FD&tag4^FS                               
                                ^CI0^FT257,910^ADB,48,28^FH\^FD&tag3^FS
                                ^CI0^FT190,910^ADB,40,24^FH\^FD&tag2^FS
                                ^CI0^FT110,910^ADB,48,28^FH\^FD&tag1^FS
                                ^FT458,910^ADB,24,14^FH\^FDNota.- Debe recontarse el paquete sin levantar el precinto.^FS
                                ^FT476,910^ADB,24,14^FH\^FD       No se admitir\A0 ninguna reclamaci\A2n en los paquetes^FS       
                                ^FT494,910^ADB,24,14^FH\^FD       que hayan sido desprecintados.^FS                                
                                ^PQ1,0,1,Y
                                ^PQ1,0,1,Y
                                ^XZ";

            _formatoZpl_Templates.Add(4, template4);

            //MDG.2010-07-14.Nueva etiqueta para pasaporte español

            //B540,800,3,1,4,9,85,B," +
            //    "A364,910,3,2,2,2,N,\"&tag6\"\r\n" +
            //    "A405,620,3,2,2,2,N,\"&tag7\"\r\n" +
            //    "A440,910,3,2,1,1,N,\"Nota.- Debe recontarse el paquete sin levantar el precinto. No se admitirá\"\r\n" +
            //    "A469,910,3,2,1,1,N,\"       ninguna reclamación en los paquetes que hayan sido desprecintados\"\r\n" +
            //    "A500,910,3,2,1,1,N,\"*Siempre que se almacene en las condiciones recomendadas\"\r\n" +

            string template5 =
                @"^XA
                                ^PR4 
                                ^BY3,3,97^FT618,845^BCB,,Y,N
                                ^FD>:&CodigoBarras^FS
                                ^CI0^FT90,910^ADB,40,24^FH\^FD&tag1^FS
                                ^CI0^FT160,910^ADB,40,24^FH\^FD&tag2^FS
                                ^CI0^FT215,910^ADB,40,24^FH\^FD&tag3^FS
                                ^CI0^FT274,910^ADB,40,24^FH\^FD&tag4^FS
                                ^CI0^FT320,820^ADB,32,20^FH\^FD&tag5^FS
                                ^CI0^FT364,910^ADB,32,20^FH\^FD&tag6^FS
                                ^CI0^FT405,620^ADB,40,24^FH\^FD&tag7^FS
                                ^FT440,910^ADB,24,14^FH\^FDNota.- Debe recontarse el paquete sin levantar el precinto. No se admitir\A0^FS
                                ^FT469,910^ADB,24,14^FH\^FD       ninguna reclamaci\A2n en los paquetes que hayan sido desprecintados^FS
                                ^FT500,910^ADB,24,14^FH\^FD* Siempre que se almacene en las condiciones recomendadas^FS                                
                                ^PQ1,0,1,Y
                                ^XZ";

            _formatoZpl_Templates.Add(5, template5);

            //JM. 2011-02-11. Formatos en lenguaje EPL. Posiciones 6 y 7.
//            string template6 = @"I8,A,001
//
//
//Q1039,024
//q831
//rN
//S4
//D7
//ZT
//JF
//OD
//R60,0
//f100
//N
//B528,800,3,1,4,10,97,B," + "\"&CodigoBarras\"\r\n" +
//                                  "A382,910,3,2,2,2,N,\"&tag5\"\r\n" +
//                                  "A287,910,3,2,3,3,N,\"&tag3\"\r\n" +
//                                  "A190,910,3,2,2,2,N,\"&tag2\"\r\n" +
//                                  "A110,910,3,2,3,3,N,\"&tag1\"\r\n" +
//                                  "A449,910,3,3,1,1,N,\"Nota.- Debe recontarse el paquete sin levantar el precinto.\"\r\n" +
//                                  "A471,910,3,3,1,1,N,\"       No se admitirá ninguna reclamación en los paquetes\"\r\n" +
//                                  "A493,910,3,3,1,1,N,\"       que hayan sido desprecintados\"\r\n" +
//                                  "P1\r\n";

            //JM. 2011-02-11. Formatos en lenguaje EPL. Posiciones 6 y 7.
            //MDG.2011-03-17.Añado tag 4 a plantilla 6 y 8 para que aparezca el texto opcional del nombre del pasaporte
            string template6 =
                @"I8,A,001


Q1039,024
q831
rN
S4
D7
ZT
JF
OD
R60,0
f100
N
B528,800,3,1,4,10,97,B," +
                "\"&CodigoBarras\"\r\n" +
                "A382,910,3,2,2,2,N,\"&tag5\"\r\n" +
                "A317,910,3,2,3,3,N,\"&tag4\"\r\n" +
                "A257,910,3,2,3,3,N,\"&tag3\"\r\n" +
                "A190,910,3,2,2,2,N,\"&tag2\"\r\n" +
                "LE910,170,500,0\r\n" +
                "A110,910,3,2,3,3,N,\"&tag1\"\r\n" +
                "A449,910,3,3,1,1,N,\"Nota.- Debe recontarse el paquete sin levantar el precinto.\"\r\n" +
                "A471,910,3,3,1,1,N,\"       No se admitirá ninguna reclamación en los paquetes\"\r\n" +
                "A493,910,3,3,1,1,N,\"       que hayan sido desprecintados\"\r\n" +
                "P1\r\n";

            _formatoZpl_Templates.Add(6, template6);


            string template7 =
                @"I8,A,001


Q1039,024
q831
rN
S4
D7
ZT
JF
OD
R60,0
f100
N
B540,800,3,1,4,9,85,B," +
                "\"&CodigoBarras\"\r\n" +
                "A90,910,3,2,2,2,N,\"&tag1\"\r\n" +
                "LE910,100,500,500\r\n" +
                "A160,910,3,2,2,2,N,\"&tag2\"\r\n" +
                "A215,910,3,2,2,2,N,\"&tag3\"\r\n" +
                "A274,910,3,2,2,2,N,\"&tag4\"\r\n" +
                "A320,820,3,2,2,2,N,\"&tag5\"\r\n" +
                "A364,910,3,2,2,2,N,\"&tag6\"\r\n" +
                "A405,620,3,2,2,2,N,\"&tag7\"\r\n" +
                "A440,910,3,2,1,1,N,\"Nota.- Debe recontarse el paquete sin levantar el precinto. No se admitirá\"\r\n" +
                "A469,910,3,2,1,1,N,\"       ninguna reclamación en los paquetes que hayan sido desprecintados\"\r\n" +
                "A500,910,3,2,1,1,N,\"*Siempre que se almacene en las condiciones recomendadas\"\r\n" +
                "P1\r\n";
            _formatoZpl_Templates.Add(7, template7);
        }

        #region Nested type: BarcodeData

        public class BarcodeData
        {
            public Country Pais { get; set; }
            public TipoPasaporte.Types Tipo { get; set; }
            public TipoPasaporte.TypeRfid Rfid { get; set; }
            public string IdCaja { get; set; }
            public int CheckSum { get; set; }

            public bool CorrespondsTo(CajaPasaportes caja)
            {
                if (caja == null) return false;
                if (caja.Id.ToUpper() == IdCaja &&
                    caja.TipoPasaporte.Country.Name == Pais.Name)
                    return true;

                return false;
            }
        }

        #endregion

        #region Nested type: BasicFields

        public struct BasicFields
        {
            public string Contenido;
            public string Destinatario;
            public string FechaLaminacion;
            public string NumeroCaja;
            public string Rango;
            public string Responsable;
            public string TipoOpcional;
        }

        #endregion

        #region Nested type: LabelTokens

        private struct LabelTokens
        {
            public const string Responsable = "&tag1";
            public const string Destinatario = "&tag2";
            public const string Contenido = "&tag3";
            public const string TipoOpcional = "&tag4";
            public const string Rango = "&tag5";
            public const string FechaLaminacion = "&tag6";
            public const string NumeroCaja = "&tag7"; //MDG.2010-07-14
            public const string CodigoBarras = "&CodigoBarras";
        }

        #endregion
    }
}