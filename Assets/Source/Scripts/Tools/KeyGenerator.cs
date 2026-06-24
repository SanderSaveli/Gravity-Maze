using System;
using System.Security.Cryptography;
using UnityEngine;

public static class KeyGenerator
{
    public static void Generate()
    {
        byte[] key = new byte[32];
        byte[] salt = new byte[16];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(key);
            rng.GetBytes(salt);
        }

        Debug.Log("KEY:  " + Convert.ToBase64String(key));
        Debug.Log("SALT: " + Convert.ToBase64String(salt));
    }
}