﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AbpDtoGenerator;
using AbpDtoGenerator.CodeAnalysis.GeneratorCodeServices;
using AbpDtoGenerator.Enums;
using AbpDtoGenerator.GeneratorModels;
using AbpDtoGenerator.Models;
using AbpDtoGenerator.ViewModels;

namespace YoyoAbpCodePowerProject.WPF
{
    public static class CodeGen
    {
        public static void Gen()
        {
            try
            {
                ViewModel viewModel = new ViewModel
                {
                    MainWindowsOptionCfg = Global.Option
                };
                GeneratorCodeReplaceInfo replaceInfo = GeneratorCodeService.Create(Global.SolutionInfo, Global.Option, Global.Entity);
                ServerModel serverModel = new ServerModel
                {
                    ReplaceInfo = replaceInfo,
                    Entity = Global.Entity
                };
                StringBuilder stringBuilder = new StringBuilder();
                StringBuilder stringBuilder2 = new StringBuilder();
                foreach (EntityFieldModel entityFieldModel in serverModel.Entity.Properties)
                {
                    if (entityFieldModel.EditChecked)
                    {
                        entityFieldModel.AppendFieldCode(stringBuilder);
                    }
                    if (entityFieldModel.ListChecked)
                    {
                        entityFieldModel.AppendFieldCode(stringBuilder2);
                    }
                }
                serverModel.EditDtoFieldCode = stringBuilder.ToString();
                serverModel.ListDtoFieldCode = stringBuilder2.ToString();
                viewModel.Server = serverModel;
                SPAModel spaclient = new SPAModel
                {
                    Entity = Global.Entity,
                    UsePermission = Global.Option.UseDomainAuthorizeCode
                };
                viewModel.SPAClient = spaclient;
                List<CodeTemplateInfo> list = CodeGen.CreateCodeTemplates();
                foreach (CodeTemplateInfo codeTemplateInfo in list)
                {
                    if (!File.Exists(codeTemplateInfo.BuildPath) || Global.Option.IsOverrideFile)
                    {
                        codeTemplateInfo.BuildCode = codeTemplateInfo.Path.GeneratorCode(viewModel, typeof(ViewModel), codeTemplateInfo.OldCustomCode);
                    }
                }
                foreach (CodeTemplateInfo codeTemplateInfo2 in list)
                {
                    if (!File.Exists(codeTemplateInfo2.BuildPath) || Global.Option.IsOverrideFile)
                    {
                        codeTemplateInfo2.BuildPath.CreateFile(codeTemplateInfo2.BuildCode);
                    }
                }
                "所有代码已经生成完毕，第一次使用可阅读生成的Readme.md文件!".InfoMsg();
            }
            catch (Exception ex)
            {
                ("代码生成出错! \r\n" + ex.ToString()).ErrorMsg();
            }
        }

        private static void AppendFieldCode(this EntityFieldModel entityField, StringBuilder code)
        {
            string newValue = entityField.FieldName;
            if (!string.IsNullOrWhiteSpace(entityField.FieldDisplayName))
            {
                code.AppendLine("");
                code.AppendLine("\t\t/// <summary>");
                code.AppendLine("\t\t/// {{PropAnnotation}}");
                code.AppendLine("\t\t/// </summary>");
                newValue = entityField.FieldDisplayName;
            }
            if (entityField.FieldTypeStr.Contains("ICollection<"))
            {
                entityField.FieldTypeStr = entityField.FieldTypeStr.Replace("ICollection<", "List<");
            }
            entityField.FieldTypeStrFirstCharToLower = entityField.FieldTypeStr.FirstCharToLower();
            if (entityField.MaxLength != null)
            {
                string text = entityField.AttributesList.Find((string o) => o.StartsWith("[MaxLength("));
                if (text != null)
                {
                    code.AppendLine(text);
                }
                else
                {
                    code.AppendLine("\t\t[MaxLength({{MaxLength}}, ErrorMessage=\"{{PropAnnotation}}超出最大长度\")]");
                }
            }
            if (entityField.MinLength != null)
            {
                string text2 = entityField.AttributesList.Find((string o) => o.StartsWith("[MinLength("));
                if (text2 != null)
                {
                    code.AppendLine(text2);
                }
                else
                {
                    code.AppendLine("\t\t[MinLength({{MinLength}}, ErrorMessage=\"{{PropAnnotation}}小于最小长度\")]");
                }
            }
            if (!string.IsNullOrWhiteSpace(entityField.RegularExpression))
            {
                code.AppendLine("\t\t[RegularExpression(\"{{RegularExpression}}\",ErrorMessage=\"{{PropAnnotation}}格式错误\")]");
            }
            if (entityField.Required)
            {
                code.AppendLine("\t\t[Required(ErrorMessage=\"{{PropAnnotation}}不能为空\")]");
            }
            if (entityField.FieldName == "Id")
            {
                code.AppendLine("\t\tpublic {{PropType}}? {{PropName}} { get; set; }");
            }
            else
            {
                code.AppendLine("\t\tpublic {{PropType}} {{PropName}} { get; set; }");
            }
            code = code.Replace("{{PropAnnotation}}", newValue).Replace("{{MaxLength}}", entityField.MaxLength.GetValueOrDefault(0).ToString()).Replace("{{MinLength}}", entityField.MinLength.GetValueOrDefault(0).ToString()).Replace("{{RegularExpression}}", entityField.RegularExpression).Replace("{{PropType}}", entityField.FieldTypeStr).Replace("{{PropName}}", entityField.FieldName);
            code.Append("\r\n\r\n");
        }

        private static List<CodeTemplateInfo> CreateCodeTemplates()
        {
            BasicOptionCfg option = Global.Option;
            EntityModel entity = Global.Entity;
            SolutionInfoModel solutionInfo = Global.SolutionInfo;
            string directoryName = Path.GetDirectoryName(typeof(CommonConsts).Assembly.Location);
            string directoryName2 = Path.GetDirectoryName(Global.SolutionInfo.CurrentSelectFilePath);
            string text = string.Empty;
            string str = directoryName2.Split(new char[]
            {
                '\\',
                '/'
            }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault<string>();
            string uiParentDirName = string.Join("-", str.ConvertLowerSplitArray());
            if (directoryName2.StartsWith(Global.SolutionInfo.Application.BasePath))
            {
                text = directoryName2.Replace(Global.SolutionInfo.Application.BasePath + "\\", string.Empty);
            }
            if (directoryName2.StartsWith(Global.SolutionInfo.Core.BasePath))
            {
                text = directoryName2.Replace(Global.SolutionInfo.Core.BasePath + "\\", string.Empty);
            }
            if (directoryName2.StartsWith(Global.SolutionInfo.EF.BasePath))
            {
                text = directoryName2.Replace(Global.SolutionInfo.EF.BasePath + "\\", string.Empty);
            }
            if (option.IsAbpZero)
            {
                if (directoryName2.StartsWith(Global.SolutionInfo.Application_Shared.BasePath))
                {
                    text = directoryName2.Replace(Global.SolutionInfo.Application_Shared.BasePath + "\\", string.Empty);
                }
                if (directoryName2.StartsWith(Global.SolutionInfo.Core_Shared.BasePath))
                {
                    text = directoryName2.Replace(Global.SolutionInfo.Core_Shared.BasePath + "\\", string.Empty);
                }
            }
            string.IsNullOrWhiteSpace(text);
            CodeGenDto codeGenDto = new CodeGenDto();
            codeGenDto.EntityDir = text;
            codeGenDto.TemplateBasePath = directoryName;
            codeGenDto.option = option;
            codeGenDto.UiParentDirName = uiParentDirName;
            codeGenDto.Entity = entity;
            codeGenDto.solutionInfoModel = solutionInfo;
            if (Global.MainViewModel.MainExtendedCfg.IsXstSolution)
            {
                return CodeGen.GetMoonsXStCodeTemplates(codeGenDto);
            }
            return CodeGen.Get52abpDefaultCodeTemplates(codeGenDto);
        }

        private static List<CodeTemplateInfo> GetMoonsXStCodeTemplates(CodeGenDto dto)
        {
            List<CodeTemplateInfo> list = new List<CodeTemplateInfo>();
            BasicOptionCfg option = dto.option;
            string entityDir = dto.EntityDir;
            string templateBasePath = dto.TemplateBasePath;
            EntityModel entity = dto.Entity;
            string uiParentDirName = dto.UiParentDirName;
            SolutionInfoModel solutionInfoModel = dto.solutionInfoModel;
            if (option.IsAllGeneratorCode || option.UseApplicationServiceCode)
            {
                string basePath = Global.SolutionInfo.Application_Shared.BasePath;
                string buildPath = Path.Combine(basePath, entityDir, "I" + entity.Name + "AppService.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Application\\IEntityApplicationService.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, "Dtos", "Get" + entity.Name + "ForEditOutput.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Application\\Dtos\\GetEntityForEditOutput.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, "Dtos", entity.Name + "EditDto.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Application\\Dtos\\EntityEditDto.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, "Dtos", "Get" + entity.Name + "ForViewDto.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Application\\Dtos\\GetEntityForViewDto.txt"), buildPath));
                basePath = Global.SolutionInfo.Application.BasePath;
                buildPath = Path.Combine(basePath, entityDir, "Mapper", entity.Name + "DtoAutoMapper.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Application\\Mapper\\EntityDtoAutoMapper.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, entity.Name + "AppService.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Application\\EntityApplicationService.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, "Readme.md");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Readme.txt"), buildPath));
                basePath = Global.SolutionInfo.Host.BasePath;
                buildPath = Path.Combine(new string[]
                {
                    basePath,
                    "wwwroot",
                    "ConfigFiles",
                    "ListViews",
                    entity.ParentDirName + "." + entity.Name + ".xml"
                });
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\WebHost\\wwwroot\\EntityListViews.txt"), buildPath));
                buildPath = Path.Combine(new string[]
                {
                    basePath,
                    "wwwroot",
                    "ConfigFiles",
                    "PageFilters",
                    entity.ParentDirName + "." + entity.Name + ".json"
                });
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\WebHost\\wwwroot\\PageFilters.txt"), buildPath));
                if (!option.IsAbpZero && option.UseNgZorro)
                {
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        "Readme.md"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Moons\\Client\\NGZorro\\Readme.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        "create-or-edit-" + entity.SplitName,
                        "create-or-edit-" + entity.SplitName + ".component.less"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Moons\\Client\\NGZorro\\EntityEditViewCss.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        "create-or-edit-" + entity.SplitName,
                        "create-or-edit-" + entity.SplitName + ".component.html"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Moons\\Client\\NGZorro\\EntityEditViewHtml.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        "create-or-edit-" + entity.SplitName,
                        "create-or-edit-" + entity.SplitName + ".component.ts"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Moons\\Client\\NGZorro\\EntityEditViewTs.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        entity.SplitName + ".component.less"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Moons\\Client\\NGZorro\\EntityListViewCss.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        entity.SplitName + ".component.html"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Client\\NGZorro\\EntityListViewHtml.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        entity.SplitName + ".component.ts"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Client\\NGZorro\\EntityListViewTs.txt"), buildPath));
                }
            }
            if (option.IsAllGeneratorCode || option.UseDomainAuthorizeCode || option.UseDomainManagerCode)
            {
                string basePath2 = Global.SolutionInfo.Core.BasePath;
                string buildPath2 = string.Empty;
                if (option.UseDomainAuthorizeCode)
                {
                    buildPath2 = Path.Combine(basePath2, entityDir, "Authorization", entity.Name + "Permissions.cs");
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Domain\\Authorization\\EntityPermissions.txt"), buildPath2));
                }
                if (option.UseDomainManagerCode)
                {
                    buildPath2 = Path.Combine(basePath2, entityDir, "DomainService", "I" + entity.Name + "Manager.cs");
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Domain\\DomainService\\IEntityManager.txt"), buildPath2));
                    buildPath2 = Path.Combine(basePath2, entityDir, "DomainService", entity.Name + "Manager.cs");
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Domain\\DomainService\\EntityManager.txt"), buildPath2));
                }
                buildPath2 = Path.Combine(basePath2, entityDir, "Readme.md");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Moons\\Server\\Readme.txt"), buildPath2));
            }
            return list;
        }

        private static List<CodeTemplateInfo> Get52abpDefaultCodeTemplates(CodeGenDto dto)
        {
            List<CodeTemplateInfo> list = new List<CodeTemplateInfo>();
            BasicOptionCfg option = dto.option;
            string entityDir = dto.EntityDir;
            string templateBasePath = dto.TemplateBasePath;
            EntityModel entity = dto.Entity;
            string uiParentDirName = dto.UiParentDirName;
            SolutionInfoModel solutionInfoModel = dto.solutionInfoModel;
            if (option.IsAllGeneratorCode || option.UseApplicationServiceCode)
            {
                string basePath = Global.SolutionInfo.Application.BasePath;
                string buildPath = Path.Combine(basePath, entityDir, "Mapper", entity.Name + "DtoAutoMapper.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Mapper\\EntityDtoAutoMapper.txt"), buildPath));
                if (option.UseExportExcel)
                {
                    buildPath = Path.Combine(basePath, entityDir, "Exporting", "I" + entity.Name + "ListExcelExporter.cs");
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Exporting\\IEntityListExcelExporter.txt"), buildPath));
                    buildPath = Path.Combine(basePath, entityDir, "Exporting", entity.Name + "ListExcelExporter.cs");
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Exporting\\EntityListExcelExporter.txt"), buildPath));
                }
                buildPath = Path.Combine(basePath, entityDir, "I" + entity.Name + "AppService.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\IEntityApplicationService.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, entity.Name + "AppService.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\EntityApplicationService.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, "Dtos", "CreateOrUpdate" + entity.Name + "Input.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Dtos\\CreateOrUpdateEntityInput.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, "Dtos", "Get" + entity.Name + "ForEditOutput.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Dtos\\GetEntityForEditOutput.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, "Dtos", "Get" + entity.Name + "sInput.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Dtos\\GetEntitysInput.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, "Dtos", entity.Name + "EditDto.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Dtos\\EntityEditDto.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, "Dtos", entity.Name + "ListDto.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Application\\Dtos\\EntityListDto.txt"), buildPath));
                buildPath = Path.Combine(basePath, entityDir, "Readme.md");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Readme.txt"), buildPath));
                if (option.UseNgZorro)
                {
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        "Readme.md"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\Readme.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        "create-or-edit-" + entity.SplitName,
                        "create-or-edit-" + entity.SplitName + ".component.less"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityEditViewCss.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        "create-or-edit-" + entity.SplitName,
                        "create-or-edit-" + entity.SplitName + ".component.html"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityEditViewHtml.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        "create-or-edit-" + entity.SplitName,
                        "create-or-edit-" + entity.SplitName + ".component.ts"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityEditViewTs.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        entity.SplitName + ".component.less"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Client, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityListViewCss.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        entity.SplitName + ".component.html"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityListViewHtml.txt"), buildPath));
                    buildPath = Path.Combine(new string[]
                    {
                        basePath,
                        entityDir,
                        "Client",
                        "NGZorro",
                        uiParentDirName,
                        entity.SplitName + ".component.ts"
                    });
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Client\\NGZorro\\EntityListViewTs.txt"), buildPath));
                }
            }
            if (option.IsAllGeneratorCode || option.UseDomainAuthorizeCode || option.UseDomainManagerCode)
            {
                string basePath2 = Global.SolutionInfo.Core.BasePath;
                string buildPath2 = string.Empty;
                if (option.UseDomainAuthorizeCode)
                {
                    buildPath2 = Path.Combine(basePath2, entityDir, "Authorization", entity.Name + "AuthorizationProvider.cs");
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Domain\\Authorization\\EntityAuthorizationProvider.txt"), buildPath2));
                    buildPath2 = Path.Combine(basePath2, entityDir, "Authorization", entity.Name + "Permissions.cs");
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Domain\\Authorization\\EntityPermissions.txt"), buildPath2));
                }
                if (option.UseDomainManagerCode)
                {
                    buildPath2 = Path.Combine(basePath2, entityDir, "DomainService", "I" + entity.Name + "Manager.cs");
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Domain\\DomainService\\IEntityManager.txt"), buildPath2));
                    buildPath2 = Path.Combine(basePath2, entityDir, "DomainService", entity.Name + "Manager.cs");
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Domain\\DomainService\\EntityManager.txt"), buildPath2));
                    buildPath2 = Path.Combine(basePath2, entityDir, "Duoyuyan.md");
                    list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Domain\\Duoyuyan.txt"), buildPath2));
                }
                buildPath2 = Path.Combine(basePath2, entityDir, "Readme.md");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Readme.txt"), buildPath2));
            }
            string buildPath3 = Path.Combine(Global.SolutionInfo.EF.BasePath, "EntityMapper", entity.Name + "s", entity.Name + "Cfg.cs");
            list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\EntityFrameworkCore\\EntityMapper\\EntityCfg.txt"), buildPath3));
            if (option.UseXUnitTests)
            {
                string buildPath4 = Path.Combine(Global.SolutionInfo.Tests.BasePath, entity.Name + "s", entity.Name + "AppService_Tests.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\Tests\\EntityAppService_Tests.txt"), buildPath4));
            }
            if (option.InitGeneratorCode)
            {
                string basePath3 = Global.SolutionInfo.Application.BasePath;
                string buildPath5 = Path.Combine(basePath3, "Dtos", "PagedAndFilteredInputDto.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\Dtos\\PagedAndFilteredInputDto.txt"), buildPath5));
                buildPath5 = Path.Combine(basePath3, "Dtos", "PagedAndSortedInputDto.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\Dtos\\PagedAndSortedInputDto.txt"), buildPath5));
                buildPath5 = Path.Combine(basePath3, "Dtos", "PagedInputDto.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\Dtos\\PagedInputDto.txt"), buildPath5));
                buildPath5 = Path.Combine(basePath3, "Dtos", "PagedSortedAndFilteredInputDto.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\Dtos\\PagedSortedAndFilteredInputDto.txt"), buildPath5));
                basePath3 = Global.SolutionInfo.Core.BasePath;
                buildPath5 = Path.Combine(basePath3, "Authorization", "AppLtmPermissions.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\AppLtmPermissions.txt"), buildPath5));
                buildPath5 = Path.Combine(basePath3, solutionInfoModel.SolutionNamespace + "DomainServiceBase.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\SolutionNameDomainServiceBase.txt"), buildPath5));
                buildPath5 = Path.Combine(basePath3, "AppLtmConsts.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\AppLtmConsts.txt"), buildPath5));
                buildPath5 = Path.Combine(basePath3, "YoYoAbpefCoreConsts.cs");
                list.Add(CodeTemplateInfo.Create(CodeTemplateType.Server, Path.Combine(templateBasePath, "Templates\\Server\\InitGeneratorCode\\YoYoAbpefCoreConsts.txt"), buildPath5));
            }
            return list;
        }
    }
}
