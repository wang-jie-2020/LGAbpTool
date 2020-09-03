using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbpDtoGenerator.CodeAnalysis;
using AbpDtoGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AbpDtoGenerator.LGFeature
{
    public static class EntityExtension
    {
        /// <summary>
        /// EntityExtension
        /// </summary>
        /// <param name="entity"></param>
        public static void UseLGFeature(this EntityModel entity)
        {
            #region 类名

            if (entity.AttributesList != null)
            {
                entity.EntityDisplayName = entity.AttributesList.GetDisplayName().Replace("\"", "");
            }

            #endregion

            #region 属性名

            foreach (var prop in entity.Properties)
            {
                prop.FieldDisplayName = prop.Attributes.GetDisplayName().Replace("\"", "");
            }

            #endregion

            #region 属性长度

            foreach (var prop in entity.Properties)
            {
                prop.MinLength = prop.Attributes.GetMinLength();
                prop.MaxLength = prop.Attributes.GetMaxLength();
            }

            #endregion
        }

        private static string GetDisplayName(this List<AttributeListSyntax> attributeGroups)
        {
            string[] attributeDataAnnotation = { "DisplayName" };

            var attributeListSyntax = attributeGroups.GetFilteredAttributeList(attributeDataAnnotation).FirstOrDefault();
            if (attributeListSyntax == null)
                return string.Empty;

            //var t = attributeListSyntax.Attributes;
            //var t1 = attributeListSyntax.Attributes.FirstOrDefault();
            //var t2 = attributeListSyntax.Attributes.FirstOrDefault().ArgumentList;
            //var t3 = attributeListSyntax.Attributes.FirstOrDefault().ArgumentList.Arguments;
            //var t4 = attributeListSyntax.Attributes.FirstOrDefault().ArgumentList.Arguments.FirstOrDefault();

            return attributeListSyntax.Attributes.FirstOrDefault()?.ArgumentList?.Arguments.FirstOrDefault()?.ToString();
        }

        private static int? GetMinLength(this List<AttributeListSyntax> attributeGroups)
        {
            string[] attributeDataAnnotation = { "MinLength", "StringLength" };

            var attributeListSyntax = attributeGroups.GetFilteredAttributeList(attributeDataAnnotation).FirstOrDefault();

            var attr = attributeListSyntax?.Attributes.FirstOrDefault();
            if (attr == null)
                return null;

            if (attr.ToString().StartsWith("StringLength"))
            {
                var min = attr.ArgumentList?.Arguments.FirstOrDefault(predicate =>
                    predicate.ToString().StartsWith("MinimumLength"));

                return (int?)min?.Expression.GetFirstToken().Value;
            }

            if (int.TryParse(attr.ArgumentList?.Arguments.FirstOrDefault()?.ToString(), out int result))
            {
                return result;
            }

            return null;
        }

        private static int? GetMaxLength(this List<AttributeListSyntax> attributeGroups)
        {
            string[] attributeDataAnnotation = { "MaxLength", "StringLength" };

            var attributeListSyntax = attributeGroups.GetFilteredAttributeList(attributeDataAnnotation).FirstOrDefault();

            var attr = attributeListSyntax?.Attributes.FirstOrDefault();
            if (attr == null)
                return null;

            if (int.TryParse(attr.ArgumentList?.Arguments.FirstOrDefault()?.ToString(), out int result))
            {
                return result;
            }

            return null;
        }

        private static List<AttributeListSyntax> GetFilteredAttributeList(
            this List<AttributeListSyntax> attributeGroups, string[] attributeDataAnnotation)
        {
            return attributeGroups.Where(a => a.Attributes.Any(att => attributeDataAnnotation.Contains(att.Name.ToString()))).ToList();
        }

        private static List<string> GetFilteredAttributeStringList(
            this List<AttributeListSyntax> attributeGroups, string[] attributeDataAnnotation)
        {
            List<AttributeListSyntax> list = attributeGroups.Where(a => a.Attributes.Any(att => attributeDataAnnotation.Contains(att.Name.ToString()))).ToList();
            List<string> stringList = new List<string>();
            foreach (AttributeListSyntax attributeListSyntax in list)
                stringList.Add(attributeListSyntax.ToString());
            return stringList;
        }
    }
}
