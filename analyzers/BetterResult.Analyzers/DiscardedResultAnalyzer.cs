using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace BetterResult.Analyzers;

/// <summary>
/// Warns when a statement drops a <c>Result</c>, or a task of one, so a failure would go unnoticed.
/// Assigning to <c>_</c> silences it.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DiscardedResultAnalyzer : DiagnosticAnalyzer
{
    /// <summary>The diagnostic id.</summary>
    public const string DiagnosticId = "BR0001";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        title: "Result is discarded",
        messageFormat: "The Result from '{0}' is discarded, so a failure would go unnoticed. Handle it, or assign it to '_' to ignore it on purpose.",
        category: "Reliability",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        helpLinkUri: "https://github.com/cds-git/BetterResult#discarded-results");

    /// <inheritdoc/>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <inheritdoc/>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(start =>
        {
            var result = start.Compilation.GetTypeByMetadataName("BetterResult.Result`1");
            if (result is null)
                return;

            var types = new ResultTypes(
                result,
                start.Compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1"),
                start.Compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask`1"));

            start.RegisterOperationAction(c => AnalyzeStatement(c, types), OperationKind.ExpressionStatement);
        });
    }

    private static void AnalyzeStatement(OperationAnalysisContext context, ResultTypes types)
    {
        var operation = ((IExpressionStatementOperation)context.Operation).Operation;

        while (operation is IConditionalAccessOperation conditional)
            operation = conditional.WhenNotNull;

        var discarded = operation switch
        {
            IAwaitOperation awaited when types.IsResult(awaited.Type) => awaited.Operation,
            IInvocationOperation invocation when types.IsResultOrTaskOfResult(invocation.Type) => invocation,
            _ => null,
        };

        if (discarded is null)
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, operation.Syntax.GetLocation(), Describe(discarded)));
    }

    private static string Describe(IOperation operation) => operation switch
    {
        IInvocationOperation invocation => invocation.TargetMethod.Name,
        _ => operation.Syntax.ToString(),
    };

    private sealed class ResultTypes(INamedTypeSymbol result, INamedTypeSymbol? task, INamedTypeSymbol? valueTask)
    {
        public bool IsResult(ITypeSymbol? type) =>
            type is INamedTypeSymbol named && SymbolEqualityComparer.Default.Equals(named.OriginalDefinition, result);

        public bool IsResultOrTaskOfResult(ITypeSymbol? type)
        {
            if (IsResult(type))
                return true;

            return type is INamedTypeSymbol { TypeArguments.Length: 1 } named
                && (SymbolEqualityComparer.Default.Equals(named.OriginalDefinition, task)
                    || SymbolEqualityComparer.Default.Equals(named.OriginalDefinition, valueTask))
                && IsResult(named.TypeArguments[0]);
        }
    }
}
