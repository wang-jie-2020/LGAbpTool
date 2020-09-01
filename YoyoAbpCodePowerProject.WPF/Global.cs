using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AbpDtoGenerator.CodeAnalysis.GeneratorCodeServices;
using AbpDtoGenerator.Models;
using AbpDtoGenerator.ViewModels;
using Newtonsoft.Json;

namespace YoyoAbpCodePowerProject.WPF
{
    public static class Global
    {
        public static string SolutionPath { get; set; }

        public static SolutionInfoModel SolutionInfo { get; set; }

        public static MainPageViewModel MainViewModel { get; set; }

        public static PropertySelectorPageModel PropertyViewModel { get; set; }

        public static BasicOptionCfg Option { get; set; }

        public static EntityModel Entity { get; set; }

        public static void InitApplication(string path)
        {
            Global.SolutionInfo = JsonConvert.DeserializeObject<SolutionInfoModel>(File.ReadAllText(Global.SolutionPath, Encoding.UTF8));
            Global.LoadEntityInfos();
            Global.CreateViewModels();
        }

        private static void LoadEntityInfos()
        {
            string text = Path.Combine(Global.SolutionInfo.SolutionPath, "52abp_code_power");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Global.SolutionInfo.CurrentSelectFilePath);
            EntityModel entityModel = LoadEntityService.LoadEntityInConfigJson(Path.Combine(text, fileNameWithoutExtension + ".json"), fileNameWithoutExtension);
            EntityModel entityModel2 = LoadEntityService.LoadEntityInfoInCurrentSelectFile(Global.SolutionInfo.CurrentSelectFilePath, fileNameWithoutExtension);
            string directoryName = Path.GetDirectoryName(Global.SolutionInfo.CurrentSelectFilePath);
            string parentDirName = (directoryName != null) ? directoryName.Split(new char[]
            {
                '\\',
                '/'
            }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault<string>() : null;
            if (entityModel == null)
            {
                Global.Entity = entityModel2;
                Global.Entity.ParentDirName = parentDirName;
                return;
            }
            using (List<EntityFieldModel>.Enumerator enumerator = entityModel2.Properties.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    EntityFieldModel item = enumerator.Current;
                    if (!entityModel.Properties.Exists((EntityFieldModel o) => o.FieldName == item.FieldName))
                    {
                        entityModel.Properties.Add(item);
                    }
                }
            }
            List<EntityFieldModel> list = new List<EntityFieldModel>();
            using (List<EntityFieldModel>.Enumerator enumerator = entityModel.Properties.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    EntityFieldModel item = enumerator.Current;
                    if (!entityModel2.Properties.Exists((EntityFieldModel o) => o.FieldName == item.FieldName))
                    {
                        list.Add(item);
                    }
                }
            }
            foreach (EntityFieldModel item2 in list)
            {
                entityModel.Properties.Remove(item2);
            }
            entityModel.Namespace = entityModel2.Namespace;
            entityModel.NameSplit = entityModel2.NameSplit;
            entityModel.EntityKeyName = entityModel2.EntityKeyName;
            entityModel.BaseClassDtoName = entityModel2.BaseClassDtoName;
            entityModel.BaseClassName = entityModel2.BaseClassName;
            entityModel.BaseClassNameList = entityModel2.BaseClassNameList;
            Global.Entity = entityModel;
            Global.Entity.ParentDirName = parentDirName;
        }

        private static void CreateViewModels()
        {
            Global.MainViewModel = MainPageViewModel.Create(Global.SolutionInfo);
            string text = Path.Combine(Global.SolutionInfo.SolutionPath, "52abp_code_power");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            MainExtendedCfg mainExtendedCfg = LoadEntityService.LoadMainExtendedInConfigJson(Path.Combine(text, "52ABP_CodePowerExtendedModel.json"));
            if (mainExtendedCfg == null)
            {
                Global.MainViewModel.MainExtendedCfg = new MainExtendedCfg();
            }
            else
            {
                Global.MainViewModel.MainExtendedCfg = mainExtendedCfg;
            }
            Global.PropertyViewModel = PropertySelectorPageModel.Create(Global.Entity.Properties);
            Global.PropertyViewModel.EntityDisplayName = Global.Entity.EntityDisplayName;
            Global.Option = Global.MainViewModel.OptionCfg;
        }
    }
}
