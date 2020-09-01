using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbpDtoGenerator.Base;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.ViewModels
{
    public class PropertySelectorPageModel : BaseViewModel
    {
        public ObservableCollection<EntityFieldModel> EntityFields { get; set; }

        public string EntityDisplayName
        {
            get
            {
                return this.entityDisplayName;
            }
            set
            {
                this.entityDisplayName = value;
                base.InvokePropertyChanged("EntityDisplayName");
            }
        }

        public double DtoWidth
        {
            get
            {
                return this.dtoWidth;
            }
            set
            {
                base.InvokePropertyChanged("DtoWidth");
            }
        }

        public double EditListDtoWidth
        {
            get
            {
                return this.editListDtoWidth;
            }
            set
            {
                base.InvokePropertyChanged("EditListDtoWidth");
            }
        }

        public double CtrlWidth
        {
            get
            {
                return this.ctrlWidth;
            }
            set
            {
                base.InvokePropertyChanged("CtrlWidth");
            }
        }

        public void OnlyShowEditAndListDtoCheckbox(bool useCtrl = false)
        {
            if (useCtrl)
            {
                this.ctrlWidth = 140.0;
            }
            this.editListDtoWidth = 48.0;
            this.dtoWidth = 0.0;
        }

        public void OnlyShowDtoCheckbox()
        {
            this.ctrlWidth = 0.0;
            this.editListDtoWidth = 0.0;
            this.dtoWidth = 30.0;
        }

        public bool IsBackMain { get; set; }

        public static PropertySelectorPageModel Create(List<EntityFieldModel> entityFields)
        {
            PropertySelectorPageModel propertySelectorPageModel = new PropertySelectorPageModel();
            propertySelectorPageModel.EntityFields = new ObservableCollection<EntityFieldModel>();
            foreach (EntityFieldModel item in entityFields)
            {
                propertySelectorPageModel.EntityFields.Add(item);
            }
            return propertySelectorPageModel;
        }

        public PropertySelectorPageModel()
        {
        }

        private string entityDisplayName;

        private double dtoWidth;

        private double editListDtoWidth;

        private double ctrlWidth;
    }
}
