using AbpDtoGenerator.CodeAnalysis.Customes;
using AbpDtoGenerator.CodeAnalysis.Enums;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbpDtoGenerator.CodeAnalysis
{
    public static class SyntaxEx
    {
        private static char[] BR = new char[2] { '\r', '\n' };
        private static List<Type> _simpleTypes = new List<Type>()
        {
          typeof (DateTime),
          typeof (TimeSpan),
          typeof (Guid),
          typeof (byte),
          typeof (byte),
          typeof (sbyte),
          typeof (sbyte),
          typeof (char),
          typeof (char),
          typeof (Decimal),
          typeof (Decimal),
          typeof (double),
          typeof (double),
          typeof (float),
          typeof (float),
          typeof (int),
          typeof (int),
          typeof (uint),
          typeof (uint),
          typeof (long),
          typeof (long),
          typeof (ulong),
          typeof (ulong),
          typeof (short),
          typeof (short),
          typeof (ushort),
          typeof (ushort),
          typeof (string),
          typeof (string)
        };

        private static List<string> _classDataAnnotationToPreserve = new List<string>()
        {
        "MetadataType"
        };

        private static List<string> _attributDataAnnotationToPreserve = new List<string>()
        {
          "Key",
          "TimeStamp",
          "ConcurrencyCheck",
          "MaxLength",
          "MinLength",
          "ForeignKey",
          "DisplayName",
          "DisplayFormat",
          "Required",
          "StringLength",
          "RegularExpression",
          "Range",
          "DataType",
          "Validation"
        };

        public const string NewLine = "\r\n";

        public static async Task<SyntaxNode> GetRootNode(this SyntaxTree syntaxTree)
        {
            return await syntaxTree.GetRootAsync(new CancellationToken());
        }

        public static async Task<List<PropertyDeclarationSyntax>> GetPropertysAsync(
          this SyntaxTree syntaxTree)
        {
            return (await syntaxTree.GetRootAsync(new CancellationToken())).GetClassNodes()[0].GetProperties();
        }

        public static List<ClassDeclarationSyntax> GetClassNodes(
          this SyntaxTree syntaxTree)
        {
            return syntaxTree.GetRootNode().Result.GetClassNodes();
        }

        public static List<ClassDeclarationSyntax> GetClassNodes(
          this SyntaxNode root)
        {
            return root.DescendantNodes((Func<SyntaxNode, bool>)(p => !(p is ClassDeclarationSyntax)), false).OfType<ClassDeclarationSyntax>().ToList<ClassDeclarationSyntax>();
        }

        public static ClassDeclarationSyntax GetFirstClassNode(
          this SyntaxTree syntaxTree)
        {
            return syntaxTree.GetRootNode().Result.GetFirstClassNode();
        }

        public static ClassDeclarationSyntax GetFirstClassNode(
          this SyntaxNode root)
        {
            return root.DescendantNodes((Func<SyntaxNode, bool>)(p => !(p is ClassDeclarationSyntax)), false).OfType<ClassDeclarationSyntax>().FirstOrDefault<ClassDeclarationSyntax>();
        }

        public static NamespaceDeclarationSyntax GetNamespaceNode(
          this SyntaxNode root)
        {
            return root.DescendantNodes((Func<SyntaxNode, bool>)(p => !(p is NamespaceDeclarationSyntax)), false).OfType<NamespaceDeclarationSyntax>().FirstOrDefault<NamespaceDeclarationSyntax>();
        }

        public static List<PropertyDeclarationSyntax> GetProperties(
          this ClassDeclarationSyntax classNode)
        {
            return classNode.DescendantNodes((Func<SyntaxNode, bool>)(p => !(p is PropertyDeclarationSyntax)), false).OfType<PropertyDeclarationSyntax>().Where<PropertyDeclarationSyntax>((Func<PropertyDeclarationSyntax, bool>)(p => p.Modifiers.Any<SyntaxToken>((Func<SyntaxToken, bool>)(m => m.Kind() == SyntaxKind.PublicKeyword)))).Where<PropertyDeclarationSyntax>((Func<PropertyDeclarationSyntax, bool>)(p => p.FirstAncestorOrSelf<ClassDeclarationSyntax>((Func<ClassDeclarationSyntax, bool>)null, true) == classNode)).Where<PropertyDeclarationSyntax>((Func<PropertyDeclarationSyntax, bool>)(p => p.AccessorList != null)).Where<PropertyDeclarationSyntax>((Func<PropertyDeclarationSyntax, bool>)(p => p.AccessorList.Accessors.Any<AccessorDeclarationSyntax>((Func<AccessorDeclarationSyntax, bool>)(a => a.Kind() == SyntaxKind.GetAccessorDeclaration)))).Where<PropertyDeclarationSyntax>((Func<PropertyDeclarationSyntax, bool>)(p => p.AccessorList.Accessors.Any<AccessorDeclarationSyntax>((Func<AccessorDeclarationSyntax, bool>)(a => a.Kind() == SyntaxKind.SetAccessorDeclaration)))).ToList<PropertyDeclarationSyntax>();
        }

        public static List<AttributeListSyntax> GetFilteredAttributeList(
          this SyntaxList<AttributeListSyntax> attributeGroups)
        {
            return attributeGroups.Where<AttributeListSyntax>((Func<AttributeListSyntax, bool>)(a => a.Attributes.Any<AttributeSyntax>((Func<AttributeSyntax, bool>)(att => SyntaxEx._attributDataAnnotationToPreserve.Contains(att.Name.ToString()))))).Select<AttributeListSyntax, AttributeListSyntax>((Func<AttributeListSyntax, AttributeListSyntax>)(a => a.RemoveNodes<AttributeListSyntax>((IEnumerable<SyntaxNode>)a.Attributes.Where<AttributeSyntax>((Func<AttributeSyntax, bool>)(att => !SyntaxEx._attributDataAnnotationToPreserve.Contains(att.Name.ToString()))).ToArray<AttributeSyntax>(), SyntaxRemoveOptions.KeepNoTrivia))).ToList<AttributeListSyntax>();
        }

        public static List<string> GetFilteredAttributeStringList(
          this SyntaxList<AttributeListSyntax> attributeGroups)
        {
            List<AttributeListSyntax> list = attributeGroups.Where<AttributeListSyntax>((Func<AttributeListSyntax, bool>)(a => a.Attributes.Any<AttributeSyntax>((Func<AttributeSyntax, bool>)(att => SyntaxEx._attributDataAnnotationToPreserve.Contains(att.Name.ToString()))))).Select<AttributeListSyntax, AttributeListSyntax>((Func<AttributeListSyntax, AttributeListSyntax>)(a => a.RemoveNodes<AttributeListSyntax>((IEnumerable<SyntaxNode>)a.Attributes.Where<AttributeSyntax>((Func<AttributeSyntax, bool>)(att => !SyntaxEx._attributDataAnnotationToPreserve.Contains(att.Name.ToString()))).ToArray<AttributeSyntax>(), SyntaxRemoveOptions.KeepNoTrivia))).ToList<AttributeListSyntax>();
            List<string> stringList = new List<string>();
            foreach (AttributeListSyntax attributeListSyntax in list)
                stringList.Add(attributeListSyntax.ToString());
            return stringList;
        }

        public static async Task<string> GetNameSpace(this SyntaxTree syntaxTree)
        {
            return (await syntaxTree.GetRootAsync(new CancellationToken())).GetNamespaceNode().Name.ToString();
        }

        public static bool IsSimpleProperty(this PropertyDeclarationSyntax propertyNode)
        {
            NullableTypeSyntax type1 = propertyNode.Type as NullableTypeSyntax;
            if (type1 != null)
                return type1.ElementType.IsSimpleType();
            GenericNameSyntax type2 = propertyNode.Type as GenericNameSyntax;
            if (type2 != null && type2.Identifier.ToString() == "Nullable")
                return type2.TypeArgumentList.Arguments.First().IsSimpleType();
            return propertyNode.Type.IsSimpleType();
        }

        public static string GetAnnotationStr(this PropertyDeclarationSyntax propertyNode)
        {
            string str1 = string.Empty;
            string str2 = propertyNode.Modifiers.ToList<SyntaxToken>().FirstOrDefault<SyntaxToken>().LeadingTrivia.ToString();
            if (!string.IsNullOrWhiteSpace(str1))
            {
                foreach (string str3 in str2.Split(SyntaxEx.BR, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrWhiteSpace(str3) && !str3.Contains("summary"))
                    {
                        str1 = str3.Replace("///", string.Empty).Trim();
                        break;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(str1))
                str1 = propertyNode.Identifier.Text;
            return str1;
        }

        public static bool IsRelation(this PropertyDeclarationSyntax p)
        {
            return !p.IsSimpleProperty();
        }

        public static bool IsCollection(this PropertyDeclarationSyntax p)
        {
            GenericNameSyntax type = p.Type as GenericNameSyntax;
            return type != null && type.Identifier.ToString() != "Nullable";
        }

        public static bool IsSimpleType(this TypeSyntax type)
        {
            return SyntaxEx._simpleTypes.Select<Type, string>((Func<Type, string>)(p => p.Namespace + "." + p.Name)).Concat<string>(SyntaxEx._simpleTypes.Select<Type, string>((Func<Type, string>)(p => p.Name))).ToList<string>().Contains(type.ToString()) || type is PredefinedTypeSyntax;
        }

        public static bool HasDataContract(this SyntaxNode rootNode)
        {
            if (rootNode == null)
                return false;
            return rootNode.ToString().Contains("[DataContract]");
        }

        public static bool HasEntities(this SyntaxNode rootNode)
        {
            if (rootNode == null)
                return false;
            return rootNode.GetClassNodes().First<ClassDeclarationSyntax>().GetProperties().Any<PropertyDeclarationSyntax>((Func<PropertyDeclarationSyntax, bool>)(p =>
            {
                if (p.Type.ToString().Length > 3 && p.Type.ToString().Substring(p.Type.ToString().Length - 3, 3) == "DTO")
                    return true;
                if (p.Type.ToString().Length > 14 && p.Type.ToString().Substring(0, 11) == "ICollection")
                    return p.Type.ToString().Substring(p.Type.ToString().Length - 4, 3) == "DTO";
                return false;
            }));
        }

        public static bool HasBaseDto(this SyntaxNode rootNode, string baseDtoName)
        {
            if (rootNode == null)
                return false;
            BaseDtoClassLocator baseDtoClassLocator = new BaseDtoClassLocator();
            baseDtoClassLocator.Visit(rootNode);
            return baseDtoClassLocator.BaseDtoName == baseDtoName;
        }

        public static bool HasDataAnnotations(SyntaxNode rootNode)
        {
            if (rootNode == null)
                return false;
            if (rootNode.ToString().Contains("[MetadataType"))
                return true;
            return rootNode.GetClassNodes().First<ClassDeclarationSyntax>().GetProperties().Any<PropertyDeclarationSyntax>((Func<PropertyDeclarationSyntax, bool>)(p => p.AttributeLists.Any<AttributeListSyntax>((Func<AttributeListSyntax, bool>)(a => a.Attributes.Any<AttributeSyntax>((Func<AttributeSyntax, bool>)(att => SyntaxEx._attributDataAnnotationToPreserve.Contains(att.Name.ToString())))))));
        }

        public static bool HasStyleCop(SyntaxNode rootNode)
        {
            return rootNode != null && rootNode.ToString().Contains("#pragma warning disable CS1591");
        }

        public static bool HasMapEntitiesById(SyntaxNode rootNode)
        {
            if (rootNode == null)
                return false;
            IEnumerable<string> propname = rootNode.GetFirstClassNode().GetProperties().Select<PropertyDeclarationSyntax, string>((Func<PropertyDeclarationSyntax, string>)(p => p.Identifier.ToString()));
            return propname.Any<string>((Func<string, bool>)(p =>
            {
                if (!propname.Contains<string>(p + "Id"))
                    return propname.Contains<string>(p + "Ids");
                return true;
            }));
        }

        public static SyntaxTrivia EndOfLineTrivia
        {
            get
            {
                return SyntaxFactory.EndOfLine("\r\n");
            }
        }

        public static TypeSyntax ToCollectionType(this string type, string collectionType)
        {
            return (TypeSyntax)SyntaxFactory.GenericName(SyntaxFactory.Identifier(collectionType)).WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SingletonSeparatedList<TypeSyntax>((TypeSyntax)SyntaxFactory.IdentifierName(type))));
        }

        public static NameSyntax SyntaxNameFromFullName(this string fullName)
        {
            if (fullName.Count<char>((Func<char, bool>)(p => p == '.')) == 0)
                return (NameSyntax)SyntaxFactory.IdentifierName(fullName);
            string[] strArray = fullName.Split('.');
            if (fullName.Count<char>((Func<char, bool>)(p => p == '.')) == 1)
                return (NameSyntax)SyntaxFactory.QualifiedName((NameSyntax)SyntaxFactory.IdentifierName(((IEnumerable<string>)strArray).First<string>()), (SimpleNameSyntax)SyntaxFactory.IdentifierName(((IEnumerable<string>)strArray).Last<string>()));
            return (NameSyntax)SyntaxFactory.QualifiedName(fullName.Substring(0, fullName.LastIndexOf('.')).SyntaxNameFromFullName(), (SimpleNameSyntax)SyntaxFactory.IdentifierName(((IEnumerable<string>)strArray).Last<string>()));
        }

        public static CompilationUnitSyntax AppendUsing(
          this CompilationUnitSyntax node,
          params string[] usingDirectives)
        {
            List<string> list = node.DescendantNodes((Func<SyntaxNode, bool>)(p => !(p is ClassDeclarationSyntax)), false).OfType<UsingDirectiveSyntax>().Select<UsingDirectiveSyntax, string>((Func<UsingDirectiveSyntax, string>)(p => p.Name.ToString())).ToList<string>();
            SyntaxList<UsingDirectiveSyntax> usings = node.Usings;
            foreach (string usingDirective in usingDirectives)
            {
                if (usingDirective != null && !list.Contains(usingDirective))
                    usings = usings.Add(usingDirective.ToUsing());
            }
            return node.WithUsings(usings);
        }

        public static NamespaceDeclarationSyntax AppendUsing(
          this NamespaceDeclarationSyntax node,
          params string[] usingDirectives)
        {
            List<string> list = node.DescendantNodes((Func<SyntaxNode, bool>)(p => !(p is ClassDeclarationSyntax)), false).OfType<UsingDirectiveSyntax>().Select<UsingDirectiveSyntax, string>((Func<UsingDirectiveSyntax, string>)(p => p.Name.ToString())).ToList<string>();
            foreach (string usingDirective in usingDirectives)
            {
                if (usingDirective != null && !list.Contains(usingDirective))
                    list.Add(usingDirective);
            }
            list.Sort();
            SyntaxList<UsingDirectiveSyntax> usings = node.Usings;
            while (usings.Count > 0)
                usings = usings.RemoveAt(0);
            usings = usings.AddRange(list.Select<string, UsingDirectiveSyntax>((Func<string, UsingDirectiveSyntax>)(u => u.ToUsing())));
            return node.WithUsings(usings);
        }

        public static UsingDirectiveSyntax ToUsing(this string @namespace)
        {
            return @namespace.SyntaxNameFromFullName().ToUsing();
        }

        public static UsingDirectiveSyntax ToUsing(this NameSyntax nameSyntaxNode)
        {
            return SyntaxFactory.UsingDirective(nameSyntaxNode.PrependWhitespace<NameSyntax>()).AppendNewLine<UsingDirectiveSyntax>(true);
        }

        public static TNode AppendNewLine<TNode>(this TNode node, bool preserveExistingTrivia = true) where TNode : SyntaxNode
        {
            SyntaxTriviaList trivia = preserveExistingTrivia ? node.GetTrailingTrivia() : SyntaxFactory.TriviaList();
            trivia = trivia.Add(SyntaxEx.EndOfLineTrivia);
            return node.WithTrailingTrivia<TNode>(trivia);
        }

        public static BaseListSyntax ToBaseClassList(this string baseClass)
        {
            return SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>((BaseTypeSyntax)SyntaxFactory.SimpleBaseType((TypeSyntax)SyntaxFactory.IdentifierName(baseClass).PrependWhitespace<IdentifierNameSyntax>()))).AppendNewLine<BaseListSyntax>(true);
        }

        public static FieldDeclarationSyntax DeclareField(
          string type,
          bool autoCreateNew)
        {
            string text = "_" + char.ToLower(type[0]).ToString() + type.Substring(1);
            return SyntaxFactory.FieldDeclaration(SyntaxFactory.VariableDeclaration((TypeSyntax)SyntaxFactory.IdentifierName(type)).WithVariables(SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(text)).WithInitializer(SyntaxFactory.EqualsValueClause((ExpressionSyntax)SyntaxFactory.ObjectCreationExpression((TypeSyntax)SyntaxFactory.IdentifierName(type)).WithArgumentList(SyntaxFactory.ArgumentList(new SeparatedSyntaxList<ArgumentSyntax>()))))))).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))).NormalizeWhitespace<FieldDeclarationSyntax>("    ", "\r\n", true).AppendNewLine<FieldDeclarationSyntax>(true);
        }

        public static ExpressionStatementSyntax InvocationStatement(
          this string member,
          params string[] args)
        {
            return SyntaxFactory.ExpressionStatement((ExpressionSyntax)member.ToMethodInvocation(((IEnumerable<string>)args).Select<string, ExpressionSyntax>((Func<string, ExpressionSyntax>)(p => p.ToMemberAccess())).ToArray<ExpressionSyntax>()));
        }

        public static ExpressionStatementSyntax AssignmentStatement(
          this string left,
          string right)
        {
            return SyntaxFactory.ExpressionStatement(left.AssignmentExpression(right, (string)null, false)).NormalizeWhitespace<ExpressionStatementSyntax>("    ", "\r\n", true).AppendNewLine<ExpressionStatementSyntax>(true);
        }

        public static InvocationExpressionSyntax ToMethodInvocation(
          this string method,
          params ExpressionSyntax[] args)
        {
            return method.ToMemberAccess().ToMethodInvocation(args);
        }

        public static InvocationExpressionSyntax ToMethodInvocation(
          this ExpressionSyntax methodMember,
          params ExpressionSyntax[] args)
        {
            IEnumerable<ArgumentSyntax> nodes = ((IEnumerable<ExpressionSyntax>)args).Select<ExpressionSyntax, ArgumentSyntax>((Func<ExpressionSyntax, ArgumentSyntax>)(p => SyntaxFactory.Argument(p)));
            return SyntaxFactory.InvocationExpression(methodMember).WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>(nodes)));
        }

        public static SimpleNameSyntax ToName(this string identifier)
        {
            return (SimpleNameSyntax)SyntaxFactory.IdentifierName(identifier);
        }

        public static ExpressionSyntax ToMemberAccess(this string selector)
        {
            string[] strArray = selector.Split('.');
            if (((IEnumerable<string>)strArray).Count<string>() == 1)
                return (ExpressionSyntax)((IEnumerable<string>)strArray).First<string>().ToName();
            if (((IEnumerable<string>)strArray).Count<string>() != 2)
                return (ExpressionSyntax)SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, string.Join(".", ((IEnumerable<string>)strArray).Take<string>(((IEnumerable<string>)strArray).Count<string>() - 1)).ToMemberAccess(), (SimpleNameSyntax)SyntaxFactory.IdentifierName(((IEnumerable<string>)strArray).Last<string>()));
            if (((IEnumerable<string>)strArray).First<string>() == "this")
                return (ExpressionSyntax)SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, (ExpressionSyntax)SyntaxFactory.ThisExpression(), (SimpleNameSyntax)SyntaxFactory.IdentifierName(((IEnumerable<string>)strArray).Last<string>()));
            return (ExpressionSyntax)SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, (ExpressionSyntax)SyntaxFactory.IdentifierName(((IEnumerable<string>)strArray).First<string>()), (SimpleNameSyntax)SyntaxFactory.IdentifierName(((IEnumerable<string>)strArray).Last<string>()));
        }

        public static SyntaxList<AttributeListSyntax> CreateAttributes(
          params string[] attributes)
        {
            return SyntaxFactory.SingletonList<AttributeListSyntax>(SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList<AttributeSyntax>(((IEnumerable<string>)attributes).Select<string, AttributeSyntax>((Func<string, AttributeSyntax>)(p => SyntaxFactory.Attribute((NameSyntax)SyntaxFactory.IdentifierName(p)))))));
        }

        public static PropertyDeclarationSyntax DeclareAutoProperty(
          this TypeSyntax type,
          string identifier)
        {
            return SyntaxFactory.PropertyDeclaration(type.AppendWhitespace<TypeSyntax>(), SyntaxFactory.Identifier(identifier).AppendWhitespace()).WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword).AppendWhitespace())).WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List<AccessorDeclarationSyntax>((IEnumerable<AccessorDeclarationSyntax>)new AccessorDeclarationSyntax[2]
            {
        SyntaxEx.PropertyAccessor(PropertyAccessorType.Get),
        SyntaxEx.PropertyAccessor(PropertyAccessorType.Set)
            }))).AppendNewLine<PropertyDeclarationSyntax>(true);
        }

        public static AccessorDeclarationSyntax PropertyAccessor(
          PropertyAccessorType type)
        {
            AccessorDeclarationSyntax node = SyntaxFactory.AccessorDeclaration(type == PropertyAccessorType.Get ? SyntaxKind.GetAccessorDeclaration : SyntaxKind.SetAccessorDeclaration, (BlockSyntax)null).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)).AppendWhitespace<AccessorDeclarationSyntax>();
            if (type == PropertyAccessorType.Get)
                node = node.PrependWhitespace<AccessorDeclarationSyntax>();
            return node;
        }

        public static TNode PrependWhitespace<TNode>(this TNode node) where TNode : SyntaxNode
        {
            return node.WithLeadingTrivia<TNode>(node.GetLeadingTrivia().Add(SyntaxFactory.Whitespace(" ")));
        }

        public static TNode AppendWhitespace<TNode>(this TNode node) where TNode : SyntaxNode
        {
            return node.WithTrailingTrivia<TNode>(node.GetTrailingTrivia().Add(SyntaxFactory.Whitespace(" ")));
        }

        public static SyntaxToken AppendWhitespace(this SyntaxToken token)
        {
            return token.WithTrailingTrivia(token.TrailingTrivia.Add(SyntaxFactory.Whitespace(" ")));
        }

        public static SyntaxToken AppendNewLine(this SyntaxToken token)
        {
            return token.WithTrailingTrivia(token.TrailingTrivia.Add(SyntaxEx.EndOfLineTrivia));
        }

        public static ExpressionSyntax WrapInConditional(
          this ExpressionSyntax expression,
          string propType)
        {
            List<BinaryExpressionSyntax> source = new List<BinaryExpressionSyntax>();
            for (MemberAccessExpressionSyntax expressionSyntax1 = expression as MemberAccessExpressionSyntax; expressionSyntax1 != null && expressionSyntax1.Expression is MemberAccessExpressionSyntax; expressionSyntax1 = expressionSyntax1.Expression as MemberAccessExpressionSyntax)
            {
                BinaryExpressionSyntax expressionSyntax2 = SyntaxFactory.BinaryExpression(SyntaxKind.NotEqualsExpression, expressionSyntax1.Expression, (ExpressionSyntax)SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));
                source.Add(expressionSyntax2);
            }
            source.Reverse();
            if (source.Count == 0)
                return expression;
            ExpressionSyntax expressionSyntax = (ExpressionSyntax)source.First<BinaryExpressionSyntax>();
            for (int index = 1; index < source.Count; ++index)
                expressionSyntax = (ExpressionSyntax)SyntaxFactory.BinaryExpression(SyntaxKind.LogicalAndExpression, expressionSyntax, (ExpressionSyntax)source[index]);
            ExpressionSyntax whenFalse = propType == null ? (ExpressionSyntax)SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression) : (ExpressionSyntax)SyntaxFactory.DefaultExpression(SyntaxFactory.ParseTypeName(propType, 0, true));
            return (ExpressionSyntax)SyntaxFactory.ConditionalExpression(expressionSyntax, expression, whenFalse).NormalizeWhitespace<ConditionalExpressionSyntax>("    ", "\r\n", false);
        }

        public static ExpressionSyntax AssignmentExpression(
          this string left,
          string right,
          string propType = null,
          bool verifyRightNotNull = false)
        {
            ExpressionSyntax memberAccess = right.ToMemberAccess();
            ExpressionSyntax node = verifyRightNotNull ? memberAccess.WrapInConditional(propType) : memberAccess;
            return (ExpressionSyntax)SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, left.ToMemberAccess().AppendWhitespace<ExpressionSyntax>(), node.PrependWhitespace<ExpressionSyntax>());
        }

        public static PropertyDeclarationSyntax GeneratePropertyDeclarationFromString(
          this string property)
        {
            SyntaxList<MemberDeclarationSyntax> members = ((CompilationUnitSyntax)CSharpSyntaxTree.ParseText("\r\nnamespace MyNameSapce \r\n{\r\n    class MyDTO\r\n    {\r\n        " + property + "\r\n    }\r\n}", (CSharpParseOptions)null, "", (Encoding)null, new CancellationToken()).GetRoot(new CancellationToken())).Members;
            members = ((NamespaceDeclarationSyntax)members[0]).Members;
            members = ((TypeDeclarationSyntax)members[0]).Members;
            return (PropertyDeclarationSyntax)members[0];
        }

        public static ExpressionSyntax GenerateAssignmentExpressionFromString(
          this string expression)
        {
            return (ExpressionSyntax)CSharpSyntaxTree.ParseText("\r\nnamespace MyNameSapce\r\n{\r\n    class MyMapper\r\n    {\r\n        public override Expression<Func<MyEntities, MyDTO>> SelectorExpression\r\n        {\r\n            get\r\n            {\r\n                return ((Expression<Func<MyEntities, MyDTO>>)(p => new MyDTO()\r\n                {\r\n                    " + expression + "\r\n                }));\r\n            }\r\n        }\r\n    }\r\n}", (CSharpParseOptions)null, "", (Encoding)null, new CancellationToken()).GetRoot(new CancellationToken()).DescendantNodes((Func<SyntaxNode, bool>)null, false).OfType<AssignmentExpressionSyntax>().FirstOrDefault<AssignmentExpressionSyntax>();
        }

        public static ExpressionStatementSyntax AssignmentStatementFromString(
          this string expression)
        {
            return CSharpSyntaxTree.ParseText("\r\nnamespace MyNameSapce\r\n{\r\n    class MyMapper\r\n    {\r\n        public override void MapToModel(MyDTO dto, MyEntities model)\r\n        {\r\n            " + expression + "\r\n        }\r\n    }\r\n}", (CSharpParseOptions)null, "", (Encoding)null, new CancellationToken()).GetRoot(new CancellationToken()).DescendantNodes((Func<SyntaxNode, bool>)null, false).OfType<ExpressionStatementSyntax>().FirstOrDefault<ExpressionStatementSyntax>();
        }
    }
}
