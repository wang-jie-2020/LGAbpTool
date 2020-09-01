using System;
using AbpDtoGenerator.Base;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public BasicOptionCfg OptionCfg
        {
            get
            {
                return this.optionCfg;
            }
            set
            {
                this.optionCfg = value;
                base.InvokePropertyChanged("OptionCfg");
            }
        }

        public MainExtendedCfg MainExtendedCfg
        {
            get
            {
                return this.mainExtendedViewModel;
            }
            set
            {
                this.mainExtendedViewModel = value;
                base.InvokePropertyChanged("MainExtendedCfg");
            }
        }

        public string VersionInfo
        {
            get
            {
                return this.versionInfo;
            }
            set
            {
                this.versionInfo = value;
                base.InvokePropertyChanged("VersionInfo");
            }
        }

        public static MainPageViewModel Create(SolutionInfoModel model)
        {
            MainPageViewModel mainPageViewModel = new MainPageViewModel();
            mainPageViewModel.OptionCfg = new BasicOptionCfg
            {
                IsAbpZero = model.IsAbpZero
            };
            if (mainPageViewModel.OptionCfg.IsAbpZero)
            {
                mainPageViewModel.VersionInfo = "当前项目为ABP Zero";
            }
            return mainPageViewModel;
        }

        private BasicOptionCfg optionCfg;

        private MainExtendedCfg mainExtendedViewModel;

        private string versionInfo;
    }
}
