using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AbpDtoGenerator
{
    public static class StringEx
    {
        public static string FirstCharToLower(this string str)
        {
            string str2 = str.Substring(0, 1).ToLower();
            string str3 = str.Substring(1, str.Length - 1);
            return str2 + str3;
        }

        public static List<string> ConvertLowerSplitArray(this string str)
        {
            List<string> list = new List<string>();
            char[] array = str.ToCharArray();
            bool flag = true;
            string text = string.Empty;
            foreach (char c in array)
            {
                if (StringEx.R.IsMatch(c.ToString()) && !flag)
                {
                    list.Add(text);
                    text = string.Empty;
                }
                text += c.ToString().ToLower();
                flag = false;
            }
            list.Add(text);
            return list;
        }

        private static Regex R = new Regex("[A-Z]");
    }
}