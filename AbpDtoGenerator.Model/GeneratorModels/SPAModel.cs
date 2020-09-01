using System;
using System.Collections.Generic;
using System.Linq;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.GeneratorModels
{
    public class SPAModel
    {
        public EntityModel Entity { get; set; }

        public bool UsePermission { get; set; }

        public string[] GetListFieldsDisplayName()
        {
            return (from o in this.Entity.Properties
                    where o.ListChecked
                    select o.FieldDisplayName).ToArray<string>();
        }

        public string[] GetListFieldsName()
        {
            return (from o in this.Entity.Properties
                    where o.ListChecked
                    select o.FieldName).ToArray<string>();
        }

        public string[] GetEditFieldsDisplayName()
        {
            return (from o in this.Entity.Properties
                    where o.EditChecked
                    select o.FieldDisplayName).ToArray<string>();
        }

        public string[] GetListFieldsNameFirstLower()
        {
            return (from o in this.Entity.Properties
                    where o.ListChecked
                    select o.FieldNameFirstLower).ToArray<string>();
        }

        public string[] GetEditFieldsNameFirstLower()
        {
            return (from o in this.Entity.Properties
                    where o.EditChecked
                    select o.FieldNameFirstLower).ToArray<string>();
        }

        public string[] GetListFieldsType()
        {
            return (from o in this.Entity.Properties
                    where o.ListChecked
                    select o.FieldTypeStr).ToArray<string>();
        }

        public List<EntityFieldModel> GetListDtoFields()
        {
            return (from o in this.Entity.Properties
                    where o.ListChecked
                    select o).ToList<EntityFieldModel>();
        }

        public List<EntityFieldModel> GetEditCheckedDtoFields()
        {
            return (from o in this.Entity.Properties
                    where o.EditChecked
                    select o).ToList<EntityFieldModel>();
        }

        public string[] GetListDtoControlNames()
        {
            return (from o in this.Entity.Properties
                    where o.ListChecked
                    select o.ContrlType).ToArray<string>();
        }
    }
}
