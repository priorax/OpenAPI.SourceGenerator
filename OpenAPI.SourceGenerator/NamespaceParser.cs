namespace OpenAPI.SourceGenerator;
internal class NamespaceParser
    {
    public static string GetNamespaceName(ClassDeclarationSyntax classDeclaration)
    {
        var parent = classDeclaration.Parent;
        string namespaceName = "demo";
        while (true)
        {
            if (parent is NamespaceDeclarationSyntax ns)
            {
                namespaceName = ns.Name.ToFullString();
                break;
            }
            if (parent is FileScopedNamespaceDeclarationSyntax fs)
            {
                namespaceName = fs.Name.ToFullString();
                break;
            }
        }
        return namespaceName;
    }
}
