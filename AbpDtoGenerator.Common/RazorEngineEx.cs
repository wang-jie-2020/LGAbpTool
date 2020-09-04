using System;
using System.IO;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace AbpDtoGenerator
{
    public static class RazorEngineEx
    {
        private static bool CreateEngineAfter { get; set; }

        public static string GeneratorCode(this Type type, string tempaltePath, object viewModel)
        {
            if (!RazorEngineEx.CreateEngineAfter)
            {
                RazorEngineEx.CreateEngine();
            }
            string templateSource = type.ReadTemplate(tempaltePath);
            return Engine.Razor.RunCompile(templateSource, tempaltePath, null, viewModel, null).Replace("&#39;", "'").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&lt;", "<");
        }

        public static string GeneratorCode(this string tempaltePath, object viewModel, Type viewModelType, string oldCustomCode)
        {
            string result;
            try
            {
                DynamicViewBag dynamicViewBag = new DynamicViewBag();
                dynamicViewBag.AddValue("OldCustomCode", oldCustomCode);
                if (!RazorEngineEx.CreateEngineAfter)
                {
                    RazorEngineEx.CreateEngine();
                }
                string templateSource = File.ReadAllText(tempaltePath, EncodingEx.Utf8WithoutBom);
                result = Engine.Razor.RunCompile(templateSource, tempaltePath, viewModelType, viewModel, dynamicViewBag);
                result = result.Replace("&#39;", "'").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&lt;", "<").Replace("<pre>", "").Replace("</pre>", "");
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public static void CreateEngine()
        {
            TemplateServiceConfiguration templateServiceConfiguration = new TemplateServiceConfiguration();
            templateServiceConfiguration.Language = Language.CSharp;
            templateServiceConfiguration.DisableTempFileLocking = true;
            templateServiceConfiguration.CachingProvider = new DefaultCachingProvider(delegate (string t)
            {
                Directory.Delete(t, true);
            });
            Engine.Razor = RazorEngineService.Create(templateServiceConfiguration);
            RazorEngineEx.CreateEngineAfter = true;
        }
    }
}
