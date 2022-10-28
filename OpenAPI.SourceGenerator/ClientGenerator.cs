using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;


namespace OpenAPI.SourceGenerator
{
    public sealed class OpenApiClientAttribute: Attribute
    {
        public string OpenApiFile { get; }

        public OpenApiClientAttribute(string openApiFile)
        {
            this.OpenApiFile = openApiFile;
        }
    }
    [Generator]
    public sealed class ClientGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            OpenApiGeneratorGeneratorSyntaxReceiver found = context.SyntaxContextReceiver as OpenApiGeneratorGeneratorSyntaxReceiver;
            if (found is null) return;
            IEnumerable<SyntaxNode> allNodes = context.Compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
            IEnumerable<ClassDeclarationSyntax> allClasses = allNodes
                .Where(d => d.IsKind(SyntaxKind.ClassDeclaration))
                .OfType<ClassDeclarationSyntax>();

            var newFiles = allClasses
                .SelectMany(c => HttpClientGenerator.GenerateHttpClientClass(c, context));
            foreach (var file in newFiles) context.AddSource(file.filename, file.content);
            foreach (var warning in found.Warnings) context.ReportDiagnostic(warning);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new OpenApiGeneratorGeneratorSyntaxReceiver());
        }
    }
}
