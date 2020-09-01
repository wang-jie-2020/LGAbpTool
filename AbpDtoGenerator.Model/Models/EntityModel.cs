using System;
using System.Collections.Generic;
using AbpDtoGenerator.Base;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace AbpDtoGenerator.Models
{
    public class EntityModel : BaseViewModel
    {
        public string EntityKeyName { get; set; }

        public string EntityDisplayName { get; set; }

        public string BaseClassName { get; set; }

        public string BaseClassDtoName { get; set; }

        public string BaseClassNameList { get; set; }

        public string Name { get; set; }

        public string LowerName { get; set; }

        public string ParentDirName { get; set; }

        public string ParentDirLowerName
        {
            get
            {
                return this.ParentDirName.ToLower();
            }
        }

        public string SplitName { get; set; }

        public List<string> NameSplit { get; set; }

        public string Namespace { get; set; }

        public List<EntityFieldModel> Properties { get; set; }

        [JsonIgnore]
        public List<AttributeListSyntax> AttributesList { get; set; }

        public EntityModel Clone()
        {
            return base.DClone<EntityModel>();
        }

        public EntityModel()
        {
        }
    }
}
