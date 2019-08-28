using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Idpsa.Control.User
{
    public class AccesoUsuario
    {       
        private static readonly AccesoUsuario _instance = new AccesoUsuario();
        private readonly byte[] mbytIV = new byte[8];
        private readonly byte[] mbytKey = new byte[8];
        private Dictionary<string, PasswordPrivilegio> UserKeys;

        private AccesoUsuario()
        {
            LoadUsers();
        }


        public TipoUsuario CurrentUser { get; set; }


        public static AccesoUsuario Instance
        {
            get { return _instance; }
        }

        private bool ConvierteLlave(string strLlave)
        {
            try
            {
                var bp = new byte[strLlave.Length];
                var aEnc = new ASCIIEncoding();
                aEnc.GetBytes(strLlave, 0, strLlave.Length, bp, 0);
                var sha = new SHA1CryptoServiceProvider();
                byte[] bpHash = sha.ComputeHash(bp);
                int i;
                for (i = 0; i <= 7; i++)
                    mbytKey[i] = bpHash[i];

                for (i = 8; i <= 15; i++)
                    mbytIV[i - 8] = bpHash[i];

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string EncriptaDato(string strDato)
        {
            string strResultado;
            if (strDato.Length > 92160)
            {
                strResultado = "Error. Data String too large. Keep within 90Kb.";
                return strResultado;
            }
            if (!(ConvierteLlave("LLAVE")))
            {
                strResultado = "Error. Fail to generate key for encryption";
                return strResultado;
            }
            strDato = string.Format("{0,5:00000}" + strDato, strDato.Length);
            var rbData = new byte[strDato.Length];
            var aEnc = new ASCIIEncoding();
            aEnc.GetBytes(strDato, 0, strDato.Length, rbData, 0);
            var descsp = new DESCryptoServiceProvider();
            ICryptoTransform desEncrypt = descsp.CreateEncryptor(mbytKey, mbytIV);
            var mStream = new MemoryStream(rbData);
            var cs = new CryptoStream(mStream, desEncrypt, CryptoStreamMode.Read);
            var mOut = new MemoryStream();
            int bytesRead;
            var output = new byte[1024];
            do
            {
                bytesRead = cs.Read(output, 0, 1024);
                if (!(bytesRead == 0))
                {
                    mOut.Write(output, 0, bytesRead);
                }
            } while ((bytesRead > 0));
            strResultado = mOut.Length == 0 ? "" : Convert.ToBase64String(mOut.GetBuffer(), 0, (int)mOut.Length);
            return strResultado;
        }

        public void ClearUsers()
        {
            UserKeys.Clear();
        }

        private string DesencriptaDato(string strDato)
        {
            string strResultado;
            if (!(ConvierteLlave("LLAVE")))
            {
                strResultado = "Error. Fail to generate key for decryption";
                return strResultado;
            }
            var descsp = new DESCryptoServiceProvider();
            ICryptoTransform desDecrypt = descsp.CreateDecryptor(mbytKey, mbytIV);
            var mOut = new MemoryStream();
            var cs = new CryptoStream(mOut, desDecrypt, CryptoStreamMode.Write);
            byte[] bPlain;
            try
            {
                bPlain = Convert.FromBase64CharArray(strDato.ToCharArray(), 0, strDato.Length);
            }
            catch (Exception)
            {
                strResultado = "Error. Input Data is not base64 encoded.";
                return strResultado;
            }
            long lRead = 0;
            long lTotal = strDato.Length;
            try
            {
                while ((lTotal >= lRead))
                {
                    cs.Write(bPlain, 0, bPlain.Length);
                    long lReadNow = ((bPlain.Length / descsp.BlockSize) * descsp.BlockSize);
                    lRead = lReadNow + lRead;
                }
                var aEnc = new ASCIIEncoding();
                strResultado = aEnc.GetString(mOut.GetBuffer(), 0, (int)mOut.Length);
                string strLen = strResultado.Substring(0, 5);
                int nLen = int.Parse(strLen);
                strResultado = strResultado.Substring(5, nLen);
                //var _length = (int)mOut.Length;
                return strResultado;
            }
            catch (Exception)
            {
                strResultado = "Error. Decryption Failed. Possibly due to incorrect Key or corrupted data";
            }
            return strResultado;
        }

        private bool LoadUsers()
        {
            bool well = true;
            UserKeys = new Dictionary<string, PasswordPrivilegio>();
            Hashtable UserKeysEncode;

            if (File.Exists(ConfigFiles.Access))
            {
                try
                {
                    var readFile = new FileStream(ConfigFiles.Access, FileMode.Open, FileAccess.Read);
                    var BFormatter = new BinaryFormatter();
                    UserKeysEncode = (Hashtable)BFormatter.Deserialize(readFile);
                    readFile.Close();
                    IDictionaryEnumerator e = UserKeysEncode.GetEnumerator();
                    while (e.MoveNext())
                    {
                        var pp = (PasswordPrivilegio)e.Value;
                        pp.Password = DesencriptaDato(pp.Password);
                        UserKeys.Add(DesencriptaDato((string)e.Key), pp);
                    }
                }
                catch (Exception)
                {
                    well = false;
                }
            }
            else
            {
                well = false;
            }
            return well;
        }

        public Dictionary<string, PasswordPrivilegio> GetUsers()
        {
            return UserKeys;
        }

        public bool SaveUsers()
        {
            var userKeysEncode = new Hashtable();
            // Try
            IDictionaryEnumerator e = UserKeys.GetEnumerator();
            while (e.MoveNext())
            {
                var pp = (PasswordPrivilegio)e.Value;
                pp.Password = EncriptaDato(pp.Password);
                userKeysEncode.Add(EncriptaDato((string)e.Key), pp);
            }
            var writeFile = new FileStream(ConfigFiles.Access, FileMode.Create, FileAccess.Write);
            var BFormatter = new BinaryFormatter();
            BFormatter.Serialize(writeFile, userKeysEncode);
            writeFile.Close();
            //Catch ex As System.Exception
            //   End Try
            return false;
        }

        public bool DeleteUser(string user)
        {
            bool well = true;
            if (UserKeys.ContainsKey(user))
            {
                UserKeys.Remove(user);
            }
            else
            {
                well = false;
            }
            return well;
        }

        public bool Exists(string user)
        {
            return UserKeys.ContainsKey(user);
        }

        public void AddChangeUser(string user, string passW, string priv)
        {
            var pp = new PasswordPrivilegio(passW, priv);
            AddChangeUser(user, pp);
        }

        public void AddChangeUser(string user, PasswordPrivilegio passPriv)
        {
            if (UserKeys.ContainsKey(user))
            {
                UserKeys[user] = passPriv;
            }
            else
            {
                UserKeys.Add(user, passPriv);
            }            
        }

        public TipoUsuario GetUserType(string user, string pass)
        {
            TipoUsuario TipoDeUsuario = TipoUsuario.None;
            if (UserKeys.ContainsKey(user))
            {
                if (pass.Trim() == GetUserPassword(user))
                {
                    TipoDeUsuario = UserKeys[user].Privilegio;
                }
            }
            return TipoDeUsuario;
        }

        public string GetUserPassword(string user)
        {
            string passW = "";
            if (UserKeys.ContainsKey(user))
            {
                passW = UserKeys[user].Password;
            }
            return passW;
        }

        public bool SetUserType(string user, TipoUsuario TipoDeUsuario)
        {            
            if (UserKeys.ContainsKey(user))
            {
                PasswordPrivilegio pp = UserKeys[user];
                UserKeys.Remove(user);
                pp.Privilegio = TipoDeUsuario;
                UserKeys.Add(user, pp);
                return true;
            }
            return false;            
        }

        public bool SetUserPassword(string user, string passW)
        {
            if (UserKeys.ContainsKey(user))
            {
                PasswordPrivilegio pp = UserKeys[user];
                UserKeys.Remove(user);
                pp.Password = passW;
                UserKeys.Add(user, pp);
                return true;
            }
            return false;
        }

        
        #region Nested type: PasswordPrivilegio

        [Serializable]
        public struct PasswordPrivilegio
        {
            public string Password;
            public TipoUsuario Privilegio;

            public PasswordPrivilegio(string password, string privilegio)
            {
                Password = password;
                Privilegio = (TipoUsuario)Enum.Parse(typeof (TipoUsuario), privilegio, true);
            }
        }

        #endregion
    }
}