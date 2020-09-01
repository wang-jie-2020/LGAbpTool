using System;
using System.Collections.Generic;
using AbpDtoGenerator.CodeAnalysis.TranslationServices;
using AbpDtoGenerator.GeneratorModels;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.CodeAnalysis.GeneratorCodeServices
{
    public static class GeneratorCodeService
    {
        public static GeneratorCodeReplaceInfo Create(SolutionInfoModel solutionInfo, BasicOptionCfg option, EntityModel entity)
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
                generatorCodeReplaceInfo.BaseClassDtoReplacement = generatorCodeReplaceInfo.BaseClassDtoReplacement + "," + entity.BaseClassNameList;
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
                    entity.Name,
                    "_Node)]"
                });
                generatorCodeReplaceInfo.PermissionQuery = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    entity.Name,
                    "_Query)]"
                });
                generatorCodeReplaceInfo.PermissionCreate = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    entity.Name,
                    "_Create)]"
                });
                generatorCodeReplaceInfo.PermissionEdit = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    entity.Name,
                    "_Edit)]"
                });
                generatorCodeReplaceInfo.PermissionDelete = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    entity.Name,
                    "_Delete)]"
                });
                generatorCodeReplaceInfo.PermissionBatchDelete = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    entity.Name,
                    "_BatchDelete)]"
                });
                generatorCodeReplaceInfo.PermissionExportExcel = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    entity.Name,
                    "_ExportExcel)]"
                });
                generatorCodeReplaceInfo.EntityCreateOrEditPermission = string.Concat(new string[]
                {
                    "[AbpAuthorize(",
                    entity.Name,
                    "Permissions.",
                    entity.Name,
                    "_Create,",
                    entity.Name,
                    "Permissions.",
                    entity.Name,
                    "_Edit)]"
                });
            }
            return generatorCodeReplaceInfo;
        }

        public static List<TranslatorTextList> BaiduTranslatorTextLists(EntityModel entity)
        {
            List<TranslatorTextList> list = new List<TranslatorTextList>();
            TranslatorTextList translatorTextList = new TranslatorTextList
            {
                CultureDisplayName = "中文",
                CultureSortName = "zh-Hans",
                TransTestList = new List<KeyValuePair<string, string>>()
            };
            foreach (EntityFieldModel entityFieldModel in entity.Properties)
            {
                if (entityFieldModel.EditChecked || entityFieldModel.ListChecked)
                {
                    if (entityFieldModel.FieldTypeStr == "string")
                    {
                        translatorTextList.TransTestList.Add(new KeyValuePair<string, string>(entityFieldModel.FieldName, entityFieldModel.FieldDisplayName));
                        translatorTextList.TransTestList.Add(new KeyValuePair<string, string>(entity.Name + entityFieldModel.FieldName, "请输入" + entityFieldModel.FieldDisplayName));
                    }
                    else
                    {
                        translatorTextList.TransTestList.Add(new KeyValuePair<string, string>(entityFieldModel.FieldName, entityFieldModel.FieldDisplayName));
                    }
                }
            }
            list.Add(translatorTextList);
            TranslatorTextList translatorTextList2 = new TranslatorTextList
            {
                CultureSortName = "zh-Hant",
                CultureDisplayName = "繁体中文",
                TransTestList = new List<KeyValuePair<string, string>>()
            };
            TranslatorTextList translatorTextList3 = new TranslatorTextList
            {
                CultureSortName = "viet",
                CultureDisplayName = "越南文",
                TransTestList = new List<KeyValuePair<string, string>>()
            };
            TranslatorTextList translatorTextList4 = new TranslatorTextList
            {
                CultureSortName = "en",
                CultureDisplayName = "英语",
                TransTestList = new List<KeyValuePair<string, string>>()
            };
            foreach (KeyValuePair<string, string> keyValuePair in translatorTextList.TransTestList)
            {
                TransRoot translate = BaiduTranslateService.GetTranslate(keyValuePair.Value, BaiduTrranslateEnum.auto, BaiduTrranslateEnum.cht);
                if (translate == null)
                {
                    break;
                }
                string dst = translate.trans_result[0].dst;
                translatorTextList2.TransTestList.Add(new KeyValuePair<string, string>(keyValuePair.Key, dst));
                string dst2 = BaiduTranslateService.GetTranslate(keyValuePair.Value, BaiduTrranslateEnum.auto, BaiduTrranslateEnum.vie).trans_result[0].dst;
                translatorTextList3.TransTestList.Add(new KeyValuePair<string, string>(keyValuePair.Key, dst2));
                string dst3 = BaiduTranslateService.GetTranslate(keyValuePair.Value, BaiduTrranslateEnum.auto, BaiduTrranslateEnum.en).trans_result[0].dst;
                translatorTextList4.TransTestList.Add(new KeyValuePair<string, string>(keyValuePair.Key, dst3));
            }
            list.Add(translatorTextList2);
            list.Add(translatorTextList3);
            list.Add(translatorTextList4);
            return list;
        }
    }
}
