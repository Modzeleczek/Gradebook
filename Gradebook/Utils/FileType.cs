using System;
using System.IO;
using System.Text;

namespace Gradebook.Models
{
    public static class FileType
    {
        /*[Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }*/

        public const string TXT = "text/plain";
        public const string PDF = "application/pdf";
        public const string ZIP = "application/zip";

        // https://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array
        public static byte[] HexStringToByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");
            byte[] arr = new byte[hex.Length >> 1];
            for (int i = 0; i < hex.Length >> 1; ++i)
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            return arr;
        }

        private static int GetHexVal(char hex)
        {
            int val = (int)hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static string StreamToHexString(Stream stream)
        {
            stream.Position = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                var bytes = ms.ToArray();
                var hex = new StringBuilder();
                foreach (var b in bytes)
                    hex.Append(b.ToString("x2"));
                return hex.ToString();
            }
        }

        public static bool IsTypeSupported(string mimeType)
        {
            return mimeType == TXT || mimeType == PDF || mimeType == ZIP;
        }
    }
}