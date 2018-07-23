using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Helpers
{
    public class SHAHelper
    {
        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        public static string ComputeHash(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Compute hash for bytes
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        public static string ComputeHash(byte[] bytes)
        {
            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        //private static string ComputeHash(string instance)
        //{
        //    var cryptoServiceProvider = new MD5CryptoServiceProvider();

        //    using (var memoryStream = new MemoryStream())
        //    using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
        //    using (var jsonWriter = new Writer(streamWriter))
        //    {
        //        var serializer = new JsonSerializer();
        //        serializer.Serialize(jsonWriter, instance);
        //        cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
        //        return String.Join("", cryptoServiceProvider.Hash.Select(c => c.ToString("x2")));
        //    }
        //}
    }
}
