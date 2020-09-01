using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbpDtoGenerator.Base;
using Newtonsoft.Json;

namespace AbpDtoGenerator.Models
{
    public class EntityFieldModel : BaseViewModel
    {
        public bool Checked
        {
            get
            {
                return this.@checked;
            }
            set
            {
                this.@checked = value;
                base.InvokePropertyChanged("Checked");
            }
        }

        public bool EditChecked
        {
            get
            {
                return this.editChecked;
            }
            set
            {
                this.editChecked = value;
                base.InvokePropertyChanged("EditChecked");
            }
        }

        public bool ListChecked
        {
            get
            {
                return this.listChecked;
            }
            set
            {
                this.listChecked = value;
                base.InvokePropertyChanged("ListChecked");
            }
        }

        public string FieldName
        {
            get
            {
                return this.fieldName;
            }
            set
            {
                this.fieldName = value;
                base.InvokePropertyChanged("FieldName");
            }
        }

        public string FieldDisplayName
        {
            get
            {
                return this.fieldDisplayName;
            }
            set
            {
                this.fieldDisplayName = value;
                base.InvokePropertyChanged("FieldDisplayName");
            }
        }

        public bool Required
        {
            get
            {
                return this.required;
            }
            set
            {
                this.required = value;
                base.InvokePropertyChanged("Required");
            }
        }

        public int? MinLength
        {
            get
            {
                return this.minLength;
            }
            set
            {
                this.minLength = value;
                base.InvokePropertyChanged("MinLength");
            }
        }

        public int? MaxLength
        {
            get
            {
                return this.maxLength;
            }
            set
            {
                this.maxLength = value;
                base.InvokePropertyChanged("MaxLength");
            }
        }

        public string RegularExpression
        {
            get
            {
                return this.regularExpression;
            }
            set
            {
                this.regularExpression = value;
                base.InvokePropertyChanged("RegularExpression");
            }
        }

        public string FieldTypeStr
        {
            get
            {
                return this.fieldTypeStr;
            }
            set
            {
                this.fieldTypeStr = value;
                base.InvokePropertyChanged("FieldTypeStr");
            }
        }

        public string FieldTypeStrFirstCharToLower
        {
            get
            {
                return this.fieldTypeStrFirstCharToLower;
            }
            set
            {
                this.fieldTypeStrFirstCharToLower = value;
                base.InvokePropertyChanged("FieldTypeStrFirstCharToLower");
            }
        }

        public Type FieldType
        {
            get
            {
                return this.fieldType;
            }
            set
            {
                this.fieldType = value;
                base.InvokePropertyChanged("FieldType");
            }
        }

        public ObservableCollection<string> CtrlTypes
        {
            get
            {
                return this.ctrlTypes;
            }
            set
            {
                this.ctrlTypes = value;
                base.InvokePropertyChanged("CtrlTypes");
            }
        }
        public int CtrlTypeIndex
        {
            get
            {
                return this.ctrlTypeIndex;
            }
            set
            {
                this.ctrlTypeIndex = value;
                base.InvokePropertyChanged("CtrlTypeIndex");
            }
        }

        public bool IsCollection { get; set; }

        public string FieldNameFirstLower { get; set; }

        public string ContrlType
        {
            get
            {
                return this.CtrlTypes[this.CtrlTypeIndex];
            }
        }

        public List<string> AttributesList { get; set; }

        public bool IsSimpleProperty { get; set; }

        public bool IsRelation { get; set; }

        public string RelatedEntityName { get; set; }

        [JsonIgnore]
        public EntityModel RelationMetadata { get; set; }

        public EntityFieldModel()
        {
            this.CtrlTypeIndex = 0;
        }

        public void HasAttributes()
        {
            if (this.AttributesList == null)
            {
                this.AttributesList = new List<string>();
                return;
            }
            foreach (string text in this.AttributesList)
            {
                if (text.StartsWith("[Required"))
                {
                    this.Required = true;
                }
                else if (text.StartsWith("[MaxLength("))
                {
                    this.MaxLength = new int?(64);
                }
                else if (text.StartsWith("[MinLength("))
                {
                    this.MinLength = new int?(0);
                }
            }
        }

        public EntityFieldModel Clone()
        {
            return base.DClone<EntityFieldModel>();
        }

        private bool @checked;

        private bool editChecked;

        private bool listChecked;

        private string fieldName;

        private string fieldDisplayName;

        private bool required;

        private int? minLength;

        private int? maxLength;

        private string regularExpression;

        private string fieldTypeStr;

        private string fieldTypeStrFirstCharToLower;

        private Type fieldType;

        private ObservableCollection<string> ctrlTypes;

        private int ctrlTypeIndex;
    }
}
