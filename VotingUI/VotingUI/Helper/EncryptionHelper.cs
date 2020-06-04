using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingUI.Helper
{
    public class EncryptionHelper
    {
        static string PassPhrase = "Pas5pr@se123467890bcdf"; // can be any string
        static string SaltValue = "voting2020";        // can be any string
        static string HashAlgorithm = "SHA1";             // can be "MD5"
        static int PasswordIterations = 99;                  // can be any number
        static string InitVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        static int KeySize = 256;

        public static string Encrypt(string decrypted)
        {
            DateTime now = DateTime.UtcNow;
            string Result = EncryptionExtensions.Encrypt(decrypted, PassPhrase, SaltValue, HashAlgorithm, PasswordIterations, InitVector, KeySize);
            return Result;
        }

        public static string Decrypt(string encrypted)
        {
            string Result = EncryptionExtensions.Decrypt(encrypted, PassPhrase, SaltValue, HashAlgorithm, PasswordIterations, InitVector, KeySize);
            return Result;
        }
    }
}
