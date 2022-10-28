namespace OpenAPI.SourceGenerator;
internal class OpenApiGeneratorGeneratorSyntaxReceiver : ISyntaxContextReceiver
{
    public List<Diagnostic> Warnings { get; set; } = new List<Diagnostic>();
    public List<ClassDeclarationSyntax> ClassesToGenerate { get; set; } = new List<ClassDeclarationSyntax>();
    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is ClassDeclarationSyntax node)
        {
            var allAttributes = node.AttributeLists.SelectMany(a => a.Attributes);
            if (!allAttributes.Any())
                return;

            var declared = context.SemanticModel.GetDeclaredSymbol(node);


            if (node.Modifiers.Any(c => c.Text == "partial"))
            {
                Warnings.Add(Diagnostic.Create(new DiagnosticDescriptor(
                    "OPENAPI-001",
                    "Class is configured for generation",
                    $"Class {node.Identifier.Text} is a partial class Everything is well!",
                    "yeet",
                    DiagnosticSeverity.Info, true), null));
            }
            else
                Warnings.Add(Diagnostic.Create(new DiagnosticDescriptor(
                                 "OPENAPI-002",
                                 "Class is configured for generation",
                                 $"Class {node.Identifier.Text} is a not a partial class, generation will not run properly",
                                 "yeet",
                                 DiagnosticSeverity.Error, true), null));
            ClassesToGenerate.Add(node);

            return;
        }
    }
    }
