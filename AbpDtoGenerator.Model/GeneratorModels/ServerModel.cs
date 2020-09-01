using System;
using System.Collections.Generic;
using System.Linq;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.GeneratorModels
{
    public class ServerModel
    {
        public GeneratorCodeReplaceInfo ReplaceInfo { get; set; }

        public EntityModel Entity { get; set; }

        public string EditDtoFieldCode { get; set; }

        public string ListDtoFieldCode { get; set; }

        public ServerModel()
        {
            this.EditDtoFieldCode = string.Empty;
            this.ListDtoFieldCode = string.Empty;
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
    }
}