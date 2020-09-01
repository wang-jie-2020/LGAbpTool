using System;
using System.IO;

namespace AbpDtoGenerator
{
    public static class TypeEx
    {
        public static string ReadTemplate(this Type type, string templatePath)
        {
            string result;
            using (Stream manifestResourceStream = type.Assembly.GetManifestResourceStream(templatePath))
            {
                result = new StreamReader(manifestResourceStream, EncodingEx.Utf8WithoutBom).ReadToEnd();
            }
            return result;
        }
    }
}