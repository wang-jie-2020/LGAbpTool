using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AbpDtoGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace AbpDtoGenerator.CodeAnalysis.GeneratorCodeServices
{
    public static class LoadEntityService
    {
        public static EntityModel LoadEntityInConfigJson(string entityConfigPath, string entityName)
        {
            try
            {
                if (File.Exists(entityConfigPath))
                {
                    return JsonConvert.DeserializeObject<EntityModel>(File.ReadAllText(entityConfigPath, EncodingEx.Utf8WithoutBom));
                }
            }
            catch (Exception ex)
            {
                ("读取实体配置文件信息出错！" + ex.ToString()).ErrorMsg();
            }
            return null;
        }

        public static MainExtendedCfg LoadMainExtendedInConfigJson(string entityConfigPath)
        {
            try
            {
                if (File.Exists(entityConfigPath))
                {
                    return JsonConvert.DeserializeObject<MainExtendedCfg>(File.ReadAllText(entityConfigPath, EncodingEx.Utf8WithoutBom));
                }
            }
            catch (Exception ex)
            {
                ("读取实体配置文件信息出错！" + ex.ToString()).ErrorMsg();
            }
            return null;
        }

        public static EntityModel LoadEntityInfoInCurrentSelectFile(string currentSelectFilePath, string entityName)
        {
            EntityModel entityModel = new EntityModel();
            SyntaxTree syntaxTree = File.ReadAllText(currentSelectFilePath, EncodingEx.Utf8WithoutBom).ToCSharpeSyntaxTree();
            ClassDeclarationSyntax firstClassNode = syntaxTree.GetFirstClassNode();
            entityModel.Name = entityName;
            entityModel.LowerName = entityModel.Name.FirstCharToLower();
            entityModel.Namespace = syntaxTree.GetNameSpace().Result;
            LoadEntityService.GetEntityBaseClassNamesAndEntityKeyTypeStr(entityModel, firstClassNode);
            entityModel.AttributesList = firstClassNode.AttributeLists.GetFilteredAttributeList();
            entityModel.Properties = (from p in firstClassNode.GetProperties()
                                      select new EntityFieldModel
                                      {
                                          FieldTypeStr = p.Type.ToString(),
                                          FieldName = p.Identifier.Text,
                                          FieldDisplayName = p.GetAnnotationStr(),
                                          FieldNameFirstLower = p.Identifier.Text.FirstCharToLower(),
                                          IsSimpleProperty = p.IsSimpleProperty(),
                                          IsRelation = p.IsRelation(),
                                          IsCollection = p.IsCollection(),
                                          AttributesList = p.AttributeLists.GetFilteredAttributeStringList(),
                                          Attributes = p.AttributeLists.GetFilteredAttributeList(),
                                          CtrlTypes = new ObservableCollection<string>
                {
                    "Text",
                    "bool",
                    "Checkbox",
                    "Textarea",
                    "DatePicker",
                    "DateTimePicker",
                    "TimePicker",
                    "DropdownList",
                    "Enums",
                    "Radio",
                    "double",
                    "float",
                    "int",
                    "long"
                },
                                          CtrlTypeIndex = 0,
                                          Checked = true,
                                          EditChecked = true,
                                          ListChecked = true
                                      }).ToList<EntityFieldModel>();
            foreach (EntityFieldModel entityFieldModel in entityModel.Properties)
            {
                string fieldTypeStr = entityFieldModel.FieldTypeStr;
                if (fieldTypeStr.Contains("ICollection<") || fieldTypeStr.Contains("List<"))
                {
                    entityFieldModel.CtrlTypeIndex = 7;
                }
                if (fieldTypeStr == "bool")
                {
                    entityFieldModel.CtrlTypeIndex = 1;
                }
                if (fieldTypeStr == "DateTime")
                {
                    entityFieldModel.CtrlTypeIndex = 5;
                }
                if (fieldTypeStr.Contains("Type") || fieldTypeStr.Contains("Enum"))
                {
                    entityFieldModel.CtrlTypeIndex = 8;
                }
                if (fieldTypeStr == "float" || fieldTypeStr == "double")
                {
                    entityFieldModel.CtrlTypeIndex = 10;
                }
                if (fieldTypeStr == "long" || fieldTypeStr == "int")
                {
                    entityFieldModel.CtrlTypeIndex = 12;
                }
                entityFieldModel.HasAttributes();
            }
            entityModel.NameSplit = entityModel.Name.ConvertLowerSplitArray();
            entityModel.SplitName = string.Join("-", entityModel.NameSplit);
            return entityModel;
        }

        private static void GetEntityBaseClassNamesAndEntityKeyTypeStr(EntityModel entityMeta, ClassDeclarationSyntax classNode)
        {
            if (classNode.BaseList != null && classNode.BaseList.Types.Count > 0)
            {
                List<string> list = (from a in classNode.BaseList.Types
                                     select a.ToString()).ToList<string>();
                string text = list.First<string>();
                list.RemoveRange(0, 1);
                if (text.Length <= 2 || !text.StartsWith("I") || !char.IsUpper(text[1]))
                {
                    entityMeta.BaseClassName = text;
                    string baseClassDtoName = string.Empty;
                    Match match = Regex.Match(text, ".*?<(.*?)>");
                    string entityKeyName;
                    if (match != null && match.Success)
                    {
                        entityKeyName = match.Groups[1].Value;
                        baseClassDtoName = text.Replace("<", "Dto<");
                    }
                    else
                    {
                        entityKeyName = "int";
                        baseClassDtoName = text + "Dto";
                    }
                    entityMeta.BaseClassNameList = string.Join(",", list);
                    entityMeta.BaseClassDtoName = baseClassDtoName;
                    entityMeta.EntityKeyName = entityKeyName;
                }
            }
        }
    }
}
