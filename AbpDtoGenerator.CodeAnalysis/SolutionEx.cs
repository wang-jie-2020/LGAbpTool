using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace AbpDtoGenerator.CodeAnalysis
{
    public static class SolutionEx
    {
        public static Project GetProjectByName(this Solution solution, string domainProjectName)
        {
            return (from a in solution.Projects
                    where a.Name.Contains(domainProjectName)
                    orderby a.Name
                    select a).FirstOrDefault<Project>();
        }

        public static Document GetDocumentForSymbol(this Compilation compilation, Solution solution, string name)
        {
            List<ISymbol> list = compilation.GetSymbolsWithName((string p) => p == name, SymbolFilter.Type, default(CancellationToken)).ToList<ISymbol>();
            if (list.Count != 1)
            {
                return null;
            }
            if ((list.First<ISymbol>() as ITypeSymbol).TypeKind == TypeKind.Enum)
            {
                return null;
            }
            Location location = (from p in list
                                 select p.Locations.FirstOrDefault<Location>()).FirstOrDefault<Location>();
            DocumentId documentId = solution.GetDocumentIdsWithFilePath(location.SourceTree.FilePath).FirstOrDefault<DocumentId>();
            return solution.GetDocument(documentId);
        }

        public static Document GetDocumentByFilePath(this Solution solution, string fullName)
        {
            return (from p in solution.GetDocumentIdsWithFilePath(fullName)
                    select solution.GetDocument(p)).FirstOrDefault<Document>();
        }

        public static List<string> GetPossibleProjects(this Document doc)
        {
            return (from p in doc.Project.Solution.Projects
                    select p.Name).ToList<string>();
        }

        public static List<DocumentId> GetDocumentIdsToOpen(this Solution newSolution, Solution oldSolution)
        {
            List<DocumentId> result;
            try
            {
                SolutionChanges changes = newSolution.GetChanges(oldSolution);
                IEnumerable<DocumentId> first = changes.GetProjectChanges().SelectMany((ProjectChanges p) => p.GetAddedDocuments());
                IEnumerable<DocumentId> second = changes.GetProjectChanges().SelectMany((ProjectChanges p) => p.GetChangedDocuments());
                result = first.Concat(second).ToList<DocumentId>();
            }
            catch (Exception)
            {
                result = new List<DocumentId>();
            }
            return result;
        }

        public static string GetProjectPath(this Project project)
        {
            return Path.GetDirectoryName(project.FilePath);
        }
    }
}
