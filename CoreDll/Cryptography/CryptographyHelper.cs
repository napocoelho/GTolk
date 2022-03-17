using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CoreDll.Cryptography
{
    public class CryptographyHelper
    {
        public static string Code(string text, string key)
        {
            try
            {
                var objcriptografaSenha = new TripleDESCryptoServiceProvider();
                var objcriptoMd5 = new MD5CryptoServiceProvider();

                byte[] byteHash, byteBuff;
                string strTempKey = key;

                byteHash = objcriptoMd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objcriptoMd5 = null;
                objcriptografaSenha.Key = byteHash;
                objcriptografaSenha.Mode = CipherMode.ECB;

                byteBuff = ASCIIEncoding.ASCII.GetBytes(text);

                return Convert.ToBase64String(objcriptografaSenha.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            }
            catch (Exception ex)
            {
                return string.Format("Digite os valores Corretamente : {0}", ex.Message);
            }
        }

        public static string Decode(string codedText, string key)
        {
            try
            {
                var objdescriptografaSenha = new TripleDESCryptoServiceProvider();
                var objcriptoMd5 = new MD5CryptoServiceProvider();

                byte[] byteHash, byteBuff;
                string strTempKey = key;

                byteHash = objcriptoMd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objcriptoMd5 = null;
                objdescriptografaSenha.Key = byteHash;
                objdescriptografaSenha.Mode = CipherMode.ECB;

                byteBuff = Convert.FromBase64String(codedText);
                string strDecrypted = ASCIIEncoding.ASCII.GetString(objdescriptografaSenha.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                objdescriptografaSenha = null;

                return strDecrypted;
            }
            catch (Exception ex)
            {
                return string.Format("Digite os valores Corretamente : {0}", ex.Message);
            }
        }

        public static byte[] CodeBytes(byte[] sequenceToCode, string key)
        {
            //byte[] decByte3 = Convert.FromBase64String(s3);
            //string text = Convert.ToBase64String(sequenceToCode);

            string text = Convert.ToBase64String(sequenceToCode);  // gsjqFw==

            string textoCriptografado = CryptographyHelper.Code(text, key);

            return Convert.FromBase64String(textoCriptografado);
        }

        public static byte[] DecodeBytes(byte[] sequenceToDecode, string key)
        {
            string textoCriptografado = Convert.ToBase64String(sequenceToDecode);  // gsjqFw==

            string texto = CryptographyHelper.Decode(textoCriptografado, key);

            return Convert.FromBase64String(texto);
        }
    }
}