﻿using System.Text;
using Tweetinvi.Security;

namespace Library_Web_Api.Identity
{
    public class PasswordHashing
    {
        public static string HashPassword(string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] passBytes = Encoding.ASCII.GetBytes(password);
            byte[] encryptBytes = sha1.ComputeHash(passBytes);
            return Convert.ToBase64String(encryptBytes);
        }
    }
}
