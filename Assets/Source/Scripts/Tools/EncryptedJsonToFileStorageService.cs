using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace SanderSaveli.UDK
{
    public class EncryptedJsonToFileStorageService : IStorageService
    {
        private const string SECRET_KEY = "o83uOtrNkgWNRwCKFUy82Zrx+LLZj5RdgSEpFN8w1T8=";
        private const string SALT = "9qSxMF6lV2svjyF54Pf6Fg==";
        private const string HMAC_KEY = "SAjXbYKAKVFbCXQmtH/unIMrNs/Tuclr0K29oUe+fec=";

        public void Save(string key, object data, Action<bool> callback = null)
        {
            try
            {
                string path = BuildPath(key);
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                string json = JsonConvert.SerializeObject(data);
                byte[] encrypted = Encrypt(json);

                byte[] hmac = ComputeHMAC(encrypted);

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    fs.Write(hmac, 0, hmac.Length);      
                    fs.Write(encrypted, 0, encrypted.Length);
                }

                callback?.Invoke(true);
                Debug.Log("Save succsess " + path);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                callback?.Invoke(false);
            }
        }

        public void Load<T>(string key, Action<T> callback)
        {
            try
            {
                string path = BuildPath(key);

                if (!File.Exists(path))
                {
                    callback?.Invoke(default);
                    return;
                }

                byte[] fileBytes = File.ReadAllBytes(path);

                byte[] storedHmac = new byte[32]; 
                byte[] encrypted = new byte[fileBytes.Length - 32];

                Array.Copy(fileBytes, 0, storedHmac, 0, 32);
                Array.Copy(fileBytes, 32, encrypted, 0, encrypted.Length);

                byte[] computedHmac = ComputeHMAC(encrypted);

                if (!CompareBytes(storedHmac, computedHmac))
                {
                    Debug.LogError("Save file tampered!");
                    callback?.Invoke(default);
                    return;
                }

                string json = Decrypt(encrypted);
                T data = JsonConvert.DeserializeObject<T>(json);

                callback?.Invoke(data);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                callback?.Invoke(default);
            }
        }

        private byte[] Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(SECRET_KEY, Encoding.UTF8.GetBytes(SALT), 10000);

                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    return ms.ToArray();
                }
            }
        }

        private string Decrypt(byte[] cipherData)
        {
            using (Aes aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(SECRET_KEY, Encoding.UTF8.GetBytes(SALT), 10000);

                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(cipherData))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private byte[] ComputeHMAC(byte[] data)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(HMAC_KEY)))
            {
                return hmac.ComputeHash(data);
            }
        }

        private bool CompareBytes(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++)
                result |= a[i] ^ b[i];

            return result == 0;
        }

        private string BuildPath(string key) =>
            Path.Combine(Application.persistentDataPath, key + ".dat");
    }
}