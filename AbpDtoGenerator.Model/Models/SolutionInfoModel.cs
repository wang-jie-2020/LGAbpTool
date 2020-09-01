using System;
using AbpDtoGenerator.GeneratorModels;

namespace AbpDtoGenerator.Models
{
    public class SolutionInfoModel
    {
        public ProjectPathInfo Application { get; set; }

        public ProjectPathInfo Application_Shared { get; set; }

        public ProjectPathInfo Core { get; set; }

        public ProjectPathInfo Core_Shared { get; set; }

        public ProjectPathInfo EF { get; set; }

        public ProjectPathInfo Tests { get; set; }

        public ProjectPathInfo MVC { get; set; }

        public ProjectPathInfo Host { get; set; }

        public ProjectPathInfo WebCore { get; set; }

        public ProjectPathInfo Portal { get; set; }

        public string SolutionNamespace { get; set; }

        public string CompanyNamespace { get; set; }

        public string CurrentProjectName { get; set; }

        public string CurrentSelectFilePath { get; set; }

        public bool IsAbpZero { get; set; }

        public string SolutionPath { get; set; }
    }
}
