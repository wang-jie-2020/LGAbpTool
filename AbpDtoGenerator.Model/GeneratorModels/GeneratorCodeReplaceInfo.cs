using System;
using System.Collections.Generic;

namespace AbpDtoGenerator.GeneratorModels
{
    public class GeneratorCodeReplaceInfo
    {
        public string CompanyNamespace { get; set; }

        public string SolutionNamespace { get; set; }

        public string BaseClassDtoReplacement { get; set; }

        public string DomainAuthorizationNamespace { get; set; }

        public string DomainServicesNamespace { get; set; }

        public string PrivateIEntityNameManager { get; set; }

        public string IEntityNameManager { get; set; }

        public string EntityNameManager { get; set; }

        public string PermissionNode { get; set; }

        public string PermissionQuery { get; set; }

        public string PermissionCreate { get; set; }

        public string PermissionEdit { get; set; }

        public string EntityCreateOrEditPermission { get; set; }

        public string PermissionDelete { get; set; }

        public string PermissionBatchDelete { get; set; }

        public string PermissionExportExcel { get; set; }

        public List<TranslatorTextList> Multi_Language { get; set; }
    }
}
