using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using AbpDtoGenerator.Enums;

namespace AbpDtoGenerator.GeneratorModels
{
    public class CodeTemplateInfo
    {
        public CodeTemplateType TemplateType { get; set; }

        public string Path { get; set; }

        public string BuildPath { get; set; }

        public string OldPath { get; private set; }

        public string OldCustomCode { get; private set; }

        public string FieldCode { get; set; }

        public void SetOldPath(string path)
        {
            this.OldPath = path;
            this.OldCustomCode = string.Empty;
            if (File.Exists(this.OldPath))
            {
                string code = File.ReadAllText(this.OldPath, Encoding.UTF8);
                this.OldCustomCode = CodeTemplateInfo.QueryCustomCode(code, "//// custom codes", "//// custom codes end");
            }
        }

        public string BuildCode { get; set; }

        public bool Build { get; set; }

        public string FileName { get; set; }

        public string FullDir { get; private set; }

        public void SetBuildPath(string buildPath)
        {
            this.BuildPath = buildPath;
            this.FileName = System.IO.Path.GetFileName(buildPath);
            this.FullDir = System.IO.Path.GetDirectoryName(buildPath);
        }

        public static CodeTemplateInfo Create(CodeTemplateType templateType, string templatePath, string buildPath)
        {
            CodeTemplateInfo codeTemplateInfo = new CodeTemplateInfo
            {
                TemplateType = templateType,
                Path = templatePath
            };
            codeTemplateInfo.SetBuildPath(buildPath);
            if (File.Exists(buildPath))
            {
                codeTemplateInfo.SetOldPath(buildPath);
            }
            codeTemplateInfo.FieldCode = string.Empty;
            return codeTemplateInfo;
        }

        private static string QueryCustomCode(string code, string start, string end)
        {
            Match match = new Regex(string.Concat(new string[]
            {
                "(?<=(",
                start,
                "))[.\\s\\S]*?(?=(",
                end,
                "))"
            }), RegexOptions.Multiline | RegexOptions.Singleline).Match(code);
            if (match.Groups.Count <= 0)
            {
                return string.Empty;
            }
            return match.Groups[0].Value;
        }
    }
}
