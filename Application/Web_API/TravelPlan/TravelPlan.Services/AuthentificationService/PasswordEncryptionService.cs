using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace TravelPlan.Services.AuthentificationService
{
    public class PasswordEncryptionService
    {
        public static string EncryptPassword(string password, int saltLength)
        {
            byte[] salt = new byte[saltLength];
            System.Security.Cryptography.RandomNumberGenerator.Create().GetBytes(salt);
            byte[] pwdBytes = System.Text.Encoding.Unicode.GetBytes(password);
            byte[] combinedBytes = new byte[pwdBytes.Length + salt.Length];
            Buffer.BlockCopy(pwdBytes, 0, combinedBytes, 0, pwdBytes.Length);
            Buffer.BlockCopy(salt, 0, combinedBytes, pwdBytes.Length, salt.Length);
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = hashAlgo.ComputeHash(combinedBytes);
            byte[] hashPlusSalt = new byte[hash.Length + salt.Length];
            Buffer.BlockCopy(hash, 0, hashPlusSalt, 0, hash.Length);
            Buffer.BlockCopy(salt, 0, hashPlusSalt, hash.Length, salt.Length);
            return Convert.ToBase64String(hashPlusSalt);
        }

        public static bool IsPasswordCorrect(string encryptedPassword, string rawPassword, int saltLength)
        {
            byte[] savedPwdBytes = Convert.FromBase64String(encryptedPassword);
            byte[] saltBytes = savedPwdBytes.Skip(savedPwdBytes.Length - saltLength).ToArray();
            byte[] hashedPwdBytes = savedPwdBytes.Take(savedPwdBytes.Length - 32).ToArray();
            string hashedPwdString = System.Text.Encoding.UTF8.GetString(hashedPwdBytes);

            byte[] pwdBytes = System.Text.Encoding.Unicode.GetBytes(rawPassword);
            byte[] combinedBytes = new byte[pwdBytes.Length + saltBytes.Length];
            Buffer.BlockCopy(pwdBytes, 0, combinedBytes, 0, pwdBytes.Length);
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, pwdBytes.Length, saltBytes.Length);
            System.Security.Cryptography.HashAlgorithm hashAlgo = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = hashAlgo.ComputeHash(combinedBytes);
            string stringHash = System.Text.Encoding.UTF8.GetString(hash);

            return hashedPwdString == stringHash;
        }
    }
}
