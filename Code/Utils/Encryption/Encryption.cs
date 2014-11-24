using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

/// <summary>
/// Helpers for generating Encryption
/// </summary>

namespace CRM.Code.Utils.Encryption
{
    public static class Encryption
    {
        public static string HashString(string value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

        public static string RandomPassword()
        {
            string chars = "ABCDEFGHIJKMLNOPQRSTUVWXYZabcdefghijkmlnopqrstuvwxyz123456789";
            Random rand = new Random();

            string s = String.Empty;

            for (int i = 0; i < rand.Next(8, 14); i++)
                s += chars[rand.Next(chars.Length)];

            return s;
        }

    }
}