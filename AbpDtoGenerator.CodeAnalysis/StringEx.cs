using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AbpDtoGenerator.CodeAnalysis
{
    public static class StringEx
    {
        public static SyntaxTree ToCSharpeSyntaxTree(this string codeString)
        {
            return CSharpSyntaxTree.ParseText(codeString, null, "", null, default(CancellationToken));
        }

        public static string MakePlural(this string name)
        {
            Regex regex = new Regex("(?<keep>[^aeiou])y$");
            Regex regex2 = new Regex("(?<keep>[aeiou]y)$");
            Regex regex3 = new Regex("(?<keep>[sxzh])$");
            Regex regex4 = new Regex("(?<keep>[^sxzhy])$");
            if (regex.IsMatch(name))
            {
                return regex.Replace(name, "${keep}ies");
            }
            if (regex2.IsMatch(name))
            {
                return regex2.Replace(name, "${keep}s");
            }
            if (regex3.IsMatch(name))
            {
                return regex3.Replace(name, "${keep}es");
            }
            if (regex4.IsMatch(name))
            {
                return regex4.Replace(name, "${keep}s");
            }
            return name;
        }

        public static string GetFileName(this string fullPath)
        {
            return Path.GetFileName(fullPath);
        }

        public static string GetDir(this string fullPath)
        {
            return Path.GetDirectoryName(fullPath);
        }
    }
}