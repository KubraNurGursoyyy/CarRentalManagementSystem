using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Commons.Concretes.Encryption
{
    public static class Sha1Encryption
    {
        private static byte[] ConvertToByte(string value)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            return ByteConverter.GetBytes(value);
        }
        public static string SHA1(string strEntry)
        {
            try
            {
                if (strEntry == "" || strEntry == null)
                {
                    throw new Exception("Encryption error occured.");
                }
                else
                {
                    SHA1CryptoServiceProvider SHA1password = new SHA1CryptoServiceProvider();
                    byte[] passwordarray = ConvertToByte(strEntry);
                    byte[] hasharray = SHA1password.ComputeHash(passwordarray);
                    return BitConverter.ToString(hasharray);
                }

            }
            catch (Exception ex) 
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.Commons.Encryption::SHA1:Error occured.", ex);
            }
           
        }
    }
}
