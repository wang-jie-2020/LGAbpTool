using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbpDtoGenerator.GeneratorModels;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.LGFeature
{
    public static class GeneratorCodeServiceExtension
    {
        public static GeneratorCodeReplaceInfo Create(SolutionInfoModel solutionInfo, BasicOptionCfg option,
            EntityModel entity)
        {
            GeneratorCodeReplaceInfo generatorCodeReplaceInfo = new GeneratorCodeReplaceInfo();
            generatorCodeReplaceInfo.Multi_Language = new List<TranslatorTextList>();
            generatorCodeReplaceInfo.SolutionNamespace = solutionInfo.SolutionNamespace;
            generatorCodeReplaceInfo.CompanyNamespace = solutionInfo.CompanyNamespace;
            generatorCodeReplaceInfo.BaseClassDtoReplacement = ": " + entity.BaseClassDtoName;
            if (generatorCodeReplaceInfo.BaseClassDtoReplacement == ": ")
            {
                generatorCodeReplaceInfo.BaseClassDtoReplacement = string.Empty;
            }
            else if (!string.IsNullOrEmpty(entity.BaseClassNameList))
            {
                generatorCodeReplaceInfo.BaseClassDtoReplacement =
                    generatorCodeReplaceInfo.BaseClassDtoReplacement + "," + entity.BaseClassNameList;
            }

            if (option.UseDomainManagerCode || option.IsAllGeneratorCode)
            {
                generatorCodeReplaceInfo.DomainServicesNamespace = "using " + entity.Namespace + ".DomainService;";
                generatorCodeReplaceInfo.PrivateIEntityNameManager = string.Concat(new string[]
                {
                    "private readonly I",
                    entity.Name,
                    "Manager _",
                    entity.LowerName,
                    "Manager;"
                });
                generatorCodeReplaceInfo.IEntityNameManager = string.Concat(new string[]
                {
                    ",I",
                    entity.Name,
                    "Manager ",
                    entity.LowerName,
                    "Manager"
                });
                generatorCodeReplaceInfo.EntityNameManager = string.Concat(new string[]
                {
                    " _",
                    entity.LowerName,
                    "Manager=",
                    entity.LowerName,
                    "Manager;"
                });
            }

            if (option.UseDomainAuthorizeCode || option.IsAllGeneratorCode)
            {
                generatorCodeReplaceInfo.DomainAuthorizationNamespace = "using " + entity.Namespace + ".Authorization;";
                generatorCodeReplaceInfo.PermissionNode = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    "Node)]"
                });
                generatorCodeReplaceInfo.PermissionQuery = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    "Query)]"
                });
                generatorCodeReplaceInfo.PermissionCreate = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    "Create)]"
                });
                generatorCodeReplaceInfo.PermissionEdit = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    "Edit)]"
                });
                generatorCodeReplaceInfo.PermissionDelete = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    "Delete)]"
                });
                generatorCodeReplaceInfo.PermissionBatchDelete = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    "BatchDelete)]"
                });
                generatorCodeReplaceInfo.PermissionExportExcel = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    "ExportExcel)]"
                });
                generatorCodeReplaceInfo.EntityCreateOrEditPermission = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    "Create, ",
                    entity.Name,
                    "Permissions.",
                    "Edit)]"
                });
            }

            return generatorCodeReplaceInfo;
        }
    }
}
