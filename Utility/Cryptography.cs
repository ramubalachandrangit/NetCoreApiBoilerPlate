using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Utility.Model;

namespace Utility
{
    public class Cryptography
    {
        public EncryptResModel AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = GetRandomBytes();

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return new EncryptResModel
            {
                Password = encryptedBytes,
                Salt = saltBytes

            };
        }
        public EncryptResModel AES_Decrypt(EncryptResModel crpt)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = crpt.Salt;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(crpt.Key, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(crpt.Password, 0, crpt.Password.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return new EncryptResModel
            {
                Password = decryptedBytes,

            };
        }
        public EncryptResModel EncryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            EncryptResModel ob = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            // string result = Convert.ToBase64String(ob.Password);

            return ob;
        }
        public string DecryptText(string input, string password, string salt)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] salts = Convert.FromBase64String(salt);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            EncryptResModel ob = new EncryptResModel();
            ob.Password = bytesToBeDecrypted;
            ob.Salt = salts;
            ob.Key = passwordBytes;

            EncryptResModel obj = AES_Decrypt(ob);

            string result = Encoding.UTF8.GetString(obj.Password);

            return result;
        }
        public static byte[] GetRandomBytes()
        {
            int saltLength = GetSaltLength();
            byte[] ba = new byte[saltLength];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        public static int GetSaltLength()
        {
            return 8;
        }

        public static string MD5Hashing(string PlainData)
        {
            MD5 mD5hash = MD5.Create();
            string HashedData = string.Empty;
            Byte[] dbytes = mD5hash.ComputeHash(Encoding.UTF8.GetBytes(PlainData.Trim()));
            StringBuilder sBuilder = new StringBuilder();
            foreach (int n in dbytes)
            {
                sBuilder.Append((n).ToString("x2"));
            }
            HashedData = sBuilder.ToString();
            return HashedData;
        }
        public static string CreateHash(string valueToHash)
        {
            string hashResp;
            try
            {
                using (MD5 hasher = MD5.Create())
                {
                    // Convert to byte array and get hash
                    byte[] dbytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(valueToHash.Trim()));

                    // sb to create string from bytes
                    var sBuilder = new StringBuilder();

                    // convert byte data to hex string
                    for (int n = 0, loopTo = dbytes.Length - 1; n <= loopTo; n++)
                        sBuilder.Append(dbytes[n].ToString("x2"));
                    hashResp = sBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                hashResp = string.Empty;
            }

            return hashResp;
        }
        public static string ComputeHash(string valueToHash)
        {
            // Create a SHA256   
            RSACryptoServiceProvider sha512Hash = new RSACryptoServiceProvider();
            SHA1Managed hash = new SHA1Managed();
            byte[] hashedData;
            // ComputeHash - returns byte array  
            byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));
            hashedData = sha512Hash.SignHash(bytes, CryptoConfig.MapNameToOID("SHA1"));
            // Convert byte array to a string   

            return Convert.ToBase64String(hashedData).ToString().ToUpper();

        }

        public static bool VerifyHash(string valueToHash, string Signature)
        {
            // Create a SHA256   
            RSACryptoServiceProvider sha512Hash = new RSACryptoServiceProvider();
            SHA1Managed hash = new SHA1Managed();
            byte[] hashedData;
            byte[] sig;
            // ComputeHash - returns byte array  
            byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));
            hashedData = sha512Hash.SignHash(bytes, CryptoConfig.MapNameToOID("SHA1"));
            sig = Encoding.UTF8.GetBytes(Signature);            // Convert byte array to a string   

            bool res = sha512Hash.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA1"), sig);

            return res;
        }
    }
}
