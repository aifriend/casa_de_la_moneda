using MonitorWrapper;

namespace Idpsa.Paletizado
{
    public static class ScreenPlay
    {
        private const ushort MonitorAddress = 50;
        private const ushort PassportIdAddress = 150;
        private const ushort ConfirmationButtomAddress = 200;

        public static void DoState(int state)
        {
            try
            {
                switch (state)
                {
                    case Estado.PuestoKo:
                        PuestoNoPreparado();
                        break;
                    case Estado.PuestoOk:
                        PuestoPreparadoEnAutomatico();
                        break;
                    case Estado.Init:
                        Inicializando();
                        break;
                    case Estado.Error:
                        ErrorGeneral();
                        break;
                    case Estado.ErrorDeAtasco:
                        ErrorAtasco();
                        break;
                    case Estado.ErrorDePesaje:
                        ErrorPesaje();
                        break;
                    case Estado.ErrorDeLectura:
                        ErrorLectura();
                        break;
                    case Estado.ErrProceso:
                        ErrorDeProceso();
                        break;
                    case Estado.GrupoOk:
                        GrupoCorrecto();
                        break;
                    case Estado.PesoMedidoIncorrecto:
                        PesoIncorrecto();
                        break;
                    case Estado.YaGestionado:
                        GrupoYaGestionado();
                        break;
                    case Estado.CodigoRfidIncorrecto:
                        RfidIncorrecto();
                        break;
                    case Estado.SiEntrada:
                        GrupoSinEntrada();
                        break;
                    case Estado.IntroGrupo:
                        IntroducirGrupo();
                        break;
                    case Estado.ComprobarGrupo:
                        ComprobarParametros();
                        break;
                    case Estado.OpenBox:
                        AbrirCajon();
                        break;
                    case Estado.CloseBox:
                        CerrarCajon();
                        break;
                }
            }
            catch
            {
            }
        }

        private static void Inicializando()
        {
            ushort wData = ushort.Parse(Estado.Init.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void ErrorGeneral()
        {
            ushort wData = ushort.Parse(Estado.Error.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void ErrorAtasco()
        {
            ushort wData = ushort.Parse(Estado.ErrorDeAtasco.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void ErrorLectura()
        {
            ushort wData = ushort.Parse(Estado.ErrorDeLectura.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void ErrorPesaje()
        {
            ushort wData = ushort.Parse(Estado.ErrorDePesaje.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void PuestoPreparadoEnAutomatico()
        {
            ushort wData = ushort.Parse(Estado.PuestoOk.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void PuestoNoPreparado()
        {
            ushort wData = ushort.Parse(Estado.PuestoKo.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void GrupoCorrecto()
        {
            ushort wData = ushort.Parse(Estado.GrupoOk.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void PesoIncorrecto()
        {
            ushort wData = ushort.Parse(Estado.PesoMedidoIncorrecto.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void RfidIncorrecto()
        {
            ushort wData = ushort.Parse(Estado.CodigoRfidIncorrecto.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void ErrorDeProceso()
        {
            ushort wData = ushort.Parse(Estado.ErrProceso.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void GrupoYaGestionado()
        {
            ushort wData = ushort.Parse(Estado.YaGestionado.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static int PassportMsgConverion(char idChar)
        {
            int idCode = 0;
            var idNum = (int) char.GetNumericValue(idChar);
            if (idNum == -1)
            {
                switch (idChar)
                {
                    case 'A':
                        idCode = 10;
                        break;
                    case 'B':
                        idCode = 11;
                        break;
                    case 'C':
                        idCode = 12;
                        break;
                    case 'D':
                        idCode = 13;
                        break;
                    case 'E':
                        idCode = 14;
                        break;
                    case 'F':
                        idCode = 15;
                        break;
                    case 'G':
                        idCode = 16;
                        break;
                    case 'H':
                        idCode = 17;
                        break;
                    case 'I':
                        idCode = 18;
                        break;
                    case 'J':
                        idCode = 19;
                        break;
                    case 'K':
                        idCode = 20;
                        break;
                    case 'L':
                        idCode = 21;
                        break;
                    case 'M':
                        idCode = 22;
                        break;
                    case 'N':
                        idCode = 23;
                        break;
                    case 'O':
                        idCode = 24;
                        break;
                    case 'P':
                        idCode = 25;
                        break;
                    case 'Q':
                        idCode = 26;
                        break;
                    case 'R':
                        idCode = 27;
                        break;
                    case 'S':
                        idCode = 28;
                        break;
                    case 'T':
                        idCode = 29;
                        break;
                    case 'U':
                        idCode = 30;
                        break;
                    case 'V':
                        idCode = 31;
                        break;
                    case 'W':
                        idCode = 32;
                        break;
                    case 'X':
                        idCode = 33;
                        break;
                    case 'Y':
                        idCode = 34;
                        break;
                    case 'Z':
                        idCode = 35;
                        break;
                }
            }
            else
            {
                idCode = idNum;
            }
            return idCode;
        }

        public static bool IsOk()
        {
            int confirmation = SendData.ReadData(1, ConfirmationButtomAddress);
            return confirmation > 0;
        }

        public static void UpdateIntroGrupo(string pId)
        {
            char[] idList = pId.ToCharArray();
            if (idList.Length <= 9)
            {
                ushort passMsgAddress = PassportIdAddress;
                foreach (char id in idList)
                {
                    var showId = (uint) PassportMsgConverion(id);
                    SendData.Update(showId, 1, ++passMsgAddress);
                }
            }
        }

        private static void IntroducirGrupo()
        {
            ushort wData = ushort.Parse(Estado.IntroGrupo.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void ComprobarParametros()
        {
            ushort wData = ushort.Parse(Estado.ComprobarGrupo.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void GrupoSinEntrada()
        {
            ushort wData = ushort.Parse(Estado.SiEntrada.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void AbrirCajon()
        {
            ushort wData = ushort.Parse(Estado.OpenBox.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        private static void CerrarCajon()
        {
            ushort wData = ushort.Parse(Estado.CloseBox.ToString());
            SendData.Update(wData, 1, MonitorAddress);
        }

        #region Nested type: Estado

        public struct Estado
        {
            public const int PuestoKo = 8;
            public const int PuestoOk = 1;
            public const int Init = 0;
            public const int Error = 6;
            public const int ErrorDeAtasco = 9;
            public const int ErrorDePesaje = 15;
            public const int ErrorDeLectura = 16;
            public const int GrupoOk = 2;
            public const int PesoMedidoIncorrecto = 3;
            public const int YaGestionado = 4;
            public const int CodigoRfidIncorrecto = 5;
            public const int SiEntrada = 7;
            public const int IntroGrupo = 10;
            public const int ComprobarGrupo = 12;
            public const int OpenBox = 17;
            public const int CloseBox = 18;
            public const int ErrProceso = 19;
        }

        #endregion
    }
}