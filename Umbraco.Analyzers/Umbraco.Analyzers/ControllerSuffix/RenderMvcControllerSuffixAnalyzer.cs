using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Umbraco.Analyzers.ControllerSuffix
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RenderMvcControllerSuffixAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Umb002";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: DiagnosticId, 
            title: Title, 
            messageFormat: MessageFormat, 
            category: Category, 
            defaultSeverity: DiagnosticSeverity.Error, 
            isEnabledByDefault: true, 
            description: Description,
            helpLinkUri: "https://our.umbraco.com/documentation/Reference/Routing/custom-controllers");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(ctx => AnalyzeSymbol(ctx), SymbolKind.NamedType);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext ctx)
        {
            var namedTypeSymbol = (INamedTypeSymbol)ctx.Symbol;
            var renderMvcCtrlType = ctx.Compilation.GetTypeByMetadataName("Umbraco.Web.Mvc.RenderMvcController");

            var baseType = namedTypeSymbol.BaseType;

            if (baseType.Equals(renderMvcCtrlType) == false)
            {
                return;
            }

            //We have a class type that inherits/basetype that uses RenderMvcController
            //Now lets check the name of the type - check it ends with 'controller'
            if (namedTypeSymbol.Name.ToLowerInvariant().EndsWith("controller"))
            {
                return;
            }

            //Lets report a diagnostic
            ctx.ReportDiagnostic(Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name));
        }        
    }
}
