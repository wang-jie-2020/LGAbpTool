using System;
using System.Text;

namespace AbpDtoGenerator
{
    public static class EncodingEx
    {
        public static Encoding Utf8WithoutBom
        {
            get
            {
                return EncodingEx.utf8WithoutBom;
            }
        }

        private static Encoding utf8WithoutBom = new UTF8Encoding(false);
    }
}