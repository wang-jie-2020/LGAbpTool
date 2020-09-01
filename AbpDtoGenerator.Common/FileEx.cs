using System;
using System.IO;

namespace AbpDtoGenerator
{
    public static class FileEx
    {
        public static string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public static void CreateFile(this string path, string content)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = EncodingEx.Utf8WithoutBom.GetBytes(content);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}