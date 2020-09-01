using System;
using AbpDtoGenerator.Base;

namespace AbpDtoGenerator.Models
{
    public class BasicOptionCfg : BaseViewModel
    {
        public BasicOptionCfg()
        {
            this.IsAllGeneratorCode = true;
            this.IsOverrideFile = true;
        }

        public bool IsAllGeneratorCode
        {
            get
            {
                return this.isAllGeneratorCode;
            }
            set
            {
                this.isAllGeneratorCode = value;
                this.SetAll();
            }
        }

        public bool IsOverrideFile
        {
            get
            {
                return this.isOverrideFile;
            }
            set
            {
                this.isOverrideFile = value;
                base.InvokePropertyChanged("IsOverrideFile");
            }
        }

        public Action<bool> OnOnlyCustomDtoChanged { get; set; }

        public bool IsOnlyDto
        {
            get
            {
                return this.isOnlyDto;
            }
            set
            {
                this.isOnlyDto = value;
            }
        }

        public bool UseApplicationServiceCode
        {
            get
            {
                return this.useApplicationServiceCode;
            }
            set
            {
                this.useApplicationServiceCode = value;
                this.CheckIsAllGeneratorCode();
                base.InvokePropertyChanged("UseApplicationServiceCode");
            }
        }

        public bool UseExportExcel
        {
            get
            {
                return this.useExportExcel;
            }
            set
            {
                this.useExportExcel = value;
                this.CheckIsAllGeneratorCode();
                base.InvokePropertyChanged("UseExportExcel");
            }
        }

        public bool UseNgZorro
        {
            get
            {
                return this.useNgZorro;
            }
            set
            {
                this.useNgZorro = value;
                this.CheckIsAllGeneratorCode();
                base.InvokePropertyChanged("UseNgZorro");
            }
        }

        public bool UseDomainAuthorizeCode
        {
            get
            {
                return this.useDomainAuthorizeCode;
            }
            set
            {
                this.useDomainAuthorizeCode = value;
                this.CheckIsAllGeneratorCode();
                base.InvokePropertyChanged("UseDomainAuthorizeCode");
            }
        }

        public bool UseDomainManagerCode
        {
            get
            {
                return this.useDomainManagerCode;
            }
            set
            {
                this.useDomainManagerCode = value;
                this.CheckIsAllGeneratorCode();
                base.InvokePropertyChanged("UseDomainManagerCode");
            }
        }

        public bool UseXUnitTests
        {
            get
            {
                return this.useXUnitTests;
            }
            set
            {
                this.useXUnitTests = value;
                this.CheckIsAllGeneratorCode();
                base.InvokePropertyChanged("UseXUnitTests");
            }
        }

        public bool IsRepositoryExtendCode
        {
            get
            {
                return this.isRepositoryExtendCode;
            }
            set
            {
                this.isRepositoryExtendCode = value;
                this.CheckIsAllGeneratorCode();
                base.InvokePropertyChanged("IsRepositoryExtendCode");
            }
        }

        public bool InitGeneratorCode
        {
            get
            {
                return this._initGeneratorCode;
            }
            set
            {
                this._initGeneratorCode = value;
                base.InvokePropertyChanged("InitGeneratorCode");
            }
        }

        public bool IsAbpZero
        {
            get
            {
                return this.isAbpZero;
            }
            set
            {
                this.isAbpZero = value;
                base.InvokePropertyChanged("IsAbpZero");
            }
        }

        private void SetAll()
        {
            this.useDomainAuthorizeCode = this.isAllGeneratorCode;
            this.useDomainManagerCode = this.isAllGeneratorCode;
            this.useApplicationServiceCode = this.isAllGeneratorCode;
            this.useNgZorro = this.isAllGeneratorCode;
            this.useExportExcel = this.isAllGeneratorCode;
            this.useXUnitTests = this.isAllGeneratorCode;
            base.InvokePropertyChanged("IsAllGeneratorCode");
            base.InvokePropertyChanged("UseDomainAuthorizeCode");
            base.InvokePropertyChanged("UseDomainManagerCode");
            base.InvokePropertyChanged("UseApplicationServiceCode");
            base.InvokePropertyChanged("UseNgZorro");
            base.InvokePropertyChanged("UseExportExcel");
            base.InvokePropertyChanged("UseXUnitTests");
        }

        private void CheckIsAllGeneratorCode()
        {
            this.isAllGeneratorCode = (this.useDomainAuthorizeCode && this.useDomainManagerCode && this.useApplicationServiceCode && this.useNgZorro && this.useExportExcel && this.useXUnitTests);
            base.InvokePropertyChanged("IsAllGeneratorCode");
        }

        public bool HasChecked()
        {
            return this.InitGeneratorCode || this.UseDomainAuthorizeCode || this.UseDomainManagerCode || this.UseApplicationServiceCode;
        }

        public bool DoWithoutSelectProperty()
        {
            return (this.InitGeneratorCode || this.UseDomainAuthorizeCode || this.UseDomainManagerCode) && !this.UseApplicationServiceCode;
        }

        private bool isAllGeneratorCode;

        private bool isOverrideFile;

        private bool isOnlyDto;

        private bool useApplicationServiceCode;

        private bool useExportExcel;

        private bool useNgZorro;

        private bool useDomainAuthorizeCode;

        private bool useDomainManagerCode;

        private bool useXUnitTests;

        private bool isRepositoryExtendCode;

        private bool _initGeneratorCode;

        private bool isAbpZero;
    }
}
