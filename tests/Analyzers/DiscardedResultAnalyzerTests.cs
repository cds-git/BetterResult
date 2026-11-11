using System.Collections.Immutable;
using BetterResult.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace BetterResult.Tests.Analyzers;

public class DiscardedResultAnalyzerTests
{
    [Theory]
    [InlineData("Save();", "Save")]
    [InlineData("await SaveAsync();", "SaveAsync")]
    [InlineData("await SaveValueAsync();", "SaveValueAsync")]
    [InlineData("SaveAsync();", "SaveAsync")]
    [InlineData("Save().Map(x => x + 1);", "Map")]
    [InlineData("await SaveAsync().MapAsync(x => x + 1);", "MapAsync")]
    [InlineData("Other?.Instance();", "Instance")]
    public async Task Analyzer_Should_Warn_When_ResultIsDiscarded(string statement, string method)
    {
        var diagnostics = await AnalyzeAsync(statement);

        diagnostics.Should().ContainSingle()
            .Which.GetMessage().Should().Contain($"'{method}'");
    }

    [Fact]
    public async Task Analyzer_Should_Warn_When_LambdaDiscardsResult()
    {
        var diagnostics = await AnalyzeAsync("Action save = () => Save();");

        diagnostics.Should().ContainSingle().Which.Id.Should().Be("BR0001");
    }

    [Fact]
    public async Task Analyzer_Should_Warn_When_AwaitedTaskVariableIsDiscarded()
    {
        var diagnostics = await AnalyzeAsync("var pending = SaveAsync(); await pending;");

        diagnostics.Should().ContainSingle().Which.GetMessage().Should().Contain("'pending'");
    }

    [Theory]
    [InlineData("_ = Save();")]
    [InlineData("_ = await SaveAsync();")]
    [InlineData("var result = Save();")]
    [InlineData("if (Save().IsFailure) return;")]
    [InlineData("Count();")]
    [InlineData("await Task.Delay(1);")]
    [InlineData("Func<Result<int>> save = () => Save();")]
    public async Task Analyzer_Should_NotWarn_When_ResultIsUsedOrNotAResult(string statement)
    {
        var diagnostics = await AnalyzeAsync(statement);

        diagnostics.Should().BeEmpty();
    }

    private static async Task<ImmutableArray<Diagnostic>> AnalyzeAsync(string statement)
    {
        var source = $$"""
            using System;
            using System.Threading.Tasks;
            using BetterResult;

            public class Subject
            {
                static Result<int> Save() => 1;
                static Task<Result<int>> SaveAsync() => Task.FromResult<Result<int>>(1);
                static ValueTask<Result<int>> SaveValueAsync() => new(Result<int>.Success(1));
                static int Count() => 1;
                Subject? Other => null;
                Result<int> Instance() => 1;

                async Task Run()
                {
                    {{statement}}
                }
            }
            """;

        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Select(path => MetadataReference.CreateFromFile(path));

        var compilation = CSharpCompilation.Create(
            "Subject",
            [CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken)],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));

        compilation.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .Should().BeEmpty();

        return await compilation
            .WithAnalyzers([new DiscardedResultAnalyzer()])
            .GetAnalyzerDiagnosticsAsync(TestContext.Current.CancellationToken);
    }
}
