using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AbpDtoGenerator.CodeAnalysis.Customes
{
    public class BaseDtoClassLocator : CSharpSyntaxWalker
    {
        public string BaseDtoName { get; set; }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (!node.Identifier.ToString().Contains("Mapper") && node != null)
            {
                BaseListSyntax baseList = node.BaseList;
                SeparatedSyntaxList<BaseTypeSyntax>? separatedSyntaxList = (baseList != null) ? new SeparatedSyntaxList<BaseTypeSyntax>?(baseList.Types) : null;
                if (separatedSyntaxList != null)
                {
                    this.BaseDtoName = node.BaseList.Types[0].Type.ToString();
                }
            }
            base.VisitClassDeclaration(node);
        }

        public BaseDtoClassLocator() : base(SyntaxWalkerDepth.Node)
        {
        }
    }
}