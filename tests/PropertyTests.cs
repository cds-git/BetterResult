using CsCheck;

namespace BetterResult.Tests;

/// <summary>
/// Property-based tests over the Result/Error API. These complement the example-based
/// xUnit tests by asserting algebraic invariants (monad/functor laws, short-circuit
/// behaviour, identities) hold across CsCheck-generated inputs.
/// </summary>
public class PropertyTests
{
    private static readonly Gen<Result<int>> ResultGen =
        Gen.Int.Select(i => i >= 0
            ? Result<int>.Success(i)
            : Result<int>.Failure(Error.Failure($"E{-i}", $"msg{-i}")));

    private static readonly Gen<Error> ErrorGen =
        Gen.Int.Select(i => Error.Failure($"E{i}", $"msg{i}"));

    // -------- Functor laws --------

    [Fact]
    public void Property_Map_IdentityLaw()
    {
        // r.Map(x => x) ≡ r
        ResultGen.Sample(r => r.Map(x => x).Should().Be(r));
    }

    [Fact]
    public void Property_Map_CompositionLaw()
    {
        // r.Map(f).Map(g) ≡ r.Map(x => g(f(x)))
        Gen.Select(ResultGen, Gen.Int, Gen.Int).Sample((r, a, b) =>
        {
            int F(int x) => unchecked(x + a);
            int G(int x) => unchecked(x * b);
            r.Map(F).Map(G).Should().Be(r.Map(x => G(F(x))));
        });
    }

    // -------- Monad laws --------

    [Fact]
    public void Property_Bind_LeftIdentityLaw()
    {
        // Success(a).Bind(f) ≡ f(a)
        Gen.Select(Gen.Int, Gen.Int).Sample((a, b) =>
        {
            Result<int> F(int x) => x % 2 == 0
                ? Result<int>.Success(x + b)
                : Result<int>.Failure(Error.Failure("ODD", $"{x}"));
            Result<int>.Success(a).Bind(F).Should().Be(F(a));
        });
    }

    [Fact]
    public void Property_Bind_RightIdentityLaw()
    {
        // r.Bind(Success) ≡ r
        ResultGen.Sample(r => r.Bind(Result<int>.Success).Should().Be(r));
    }

    [Fact]
    public void Property_Bind_AssociativityLaw()
    {
        // r.Bind(f).Bind(g) ≡ r.Bind(x => f(x).Bind(g))
        Gen.Select(ResultGen, Gen.Int, Gen.Int).Sample((r, a, b) =>
        {
            Result<int> F(int x) => unchecked(x + a) % 3 == 0
                ? Result<int>.Failure(Error.Failure("F", "from F"))
                : Result<int>.Success(unchecked(x + a));
            Result<int> G(int x) => unchecked(x + b) % 5 == 0
                ? Result<int>.Failure(Error.Failure("G", "from G"))
                : Result<int>.Success(unchecked(x + b));
            r.Bind(F).Bind(G).Should().Be(r.Bind(x => F(x).Bind(G)));
        });
    }

    // -------- Failure propagation --------

    [Fact]
    public void Property_Map_PropagatesError_Unchanged()
    {
        // For any failed result, Map preserves the error and never invokes the function.
        ErrorGen.Sample(err =>
        {
            var failed = Result<int>.Failure(err);
            var called = false;
            var mapped = failed.Map(x => { called = true; return x * 2; });
            mapped.IsFailure.Should().BeTrue();
            mapped.Error.Should().Be(err);
            called.Should().BeFalse();
        });
    }

    [Fact]
    public void Property_Bind_PropagatesError_Unchanged()
    {
        ErrorGen.Sample(err =>
        {
            var failed = Result<int>.Failure(err);
            var called = false;
            var bound = failed.Bind(x => { called = true; return Result<int>.Success(x); });
            bound.IsFailure.Should().BeTrue();
            bound.Error.Should().Be(err);
            called.Should().BeFalse();
        });
    }

    // -------- Sequence --------

    [Fact]
    public void Property_Sequence_AllSuccess_CollectsValues()
    {
        Gen.Int[0, 50].SelectMany(n => Gen.Int.Array[n]).Sample(values =>
        {
            var results = values.Select(i => Result<int>.Success(i));
            var sequenced = results.Sequence();
            sequenced.IsSuccess.Should().BeTrue();
            sequenced.Value.Should().Equal(values);
        });
    }

    [Fact]
    public void Property_Sequence_ShortCircuits_OnFirstFailure()
    {
        // Pre-success values, then a failure, then more (which must NOT be enumerated).
        Gen.Select(Gen.Int.Array[0, 20], ErrorGen).Sample((prefix, err) =>
        {
            var enumeratedAfterFailure = false;
            IEnumerable<Result<int>> Source()
            {
                foreach (var v in prefix) yield return Result<int>.Success(v);
                yield return Result<int>.Failure(err);
                enumeratedAfterFailure = true;
                yield return Result<int>.Success(999);
            }

            var sequenced = Source().Sequence();
            sequenced.IsFailure.Should().BeTrue();
            sequenced.Error.Should().Be(err);
            enumeratedAfterFailure.Should().BeFalse();
        });
    }

    // -------- Combine --------

    [Fact]
    public void Property_Combine_ReturnsFirstError_InArgOrder()
    {
        // For two results where r1 fails, Combine returns r1.Error regardless of r2.
        Gen.Select(ErrorGen, ResultGen).Sample((err1, r2) =>
        {
            var combined = Result.Combine(Result<int>.Failure(err1), r2);
            combined.IsFailure.Should().BeTrue();
            combined.Error.Should().Be(err1);
        });
    }

    [Fact]
    public void Property_Combine_AllSuccess_ProducesTuple()
    {
        Gen.Select(Gen.Int, Gen.Int).Sample((a, b) =>
        {
            var combined = Result.Combine(Result<int>.Success(a), Result<int>.Success(b));
            combined.IsSuccess.Should().BeTrue();
            combined.Value.Should().Be((a, b));
        });
    }

    // -------- Recover --------

    [Fact]
    public void Property_Recover_OnSuccess_IsIdentity()
    {
        Gen.Int.Sample(i =>
        {
            var r = Result<int>.Success(i);
            r.Recover(ErrorType.Failure, _ => Result<int>.Success(-1)).Should().Be(r);
            r.Recover(_ => true, _ => Result<int>.Success(-1)).Should().Be(r);
            r.Recover(ErrorType.Failure, -1).Should().Be(r);
        });
    }

    [Fact]
    public void Property_Recover_NonMatchingType_PreservesError()
    {
        // If the actual error type isn't in the recover set, the error passes through.
        Gen.Int.Sample(i =>
        {
            var failed = Result<int>.Failure(Error.Validation("V", $"v{i}"));
            var recovered = failed.Recover(ErrorType.NotFound, 999);
            recovered.IsFailure.Should().BeTrue();
            recovered.Error.Type.Should().Be(ErrorType.Validation);
        });
    }

    // -------- MapError --------

    [Fact]
    public void Property_MapError_OnSuccess_IsIdentity()
    {
        Gen.Int.Sample(i =>
        {
            var r = Result<int>.Success(i);
            r.MapError(err => err.WithMessage("ctx")).Should().Be(r);
            r.MapError(err => Result<int>.Failure(err.WithMessage("ctx"))).Should().Be(r);
        });
    }

    [Fact]
    public void Property_MapError_OnFailure_AppliesTransform()
    {
        ErrorGen.Sample(err =>
        {
            var failed = Result<int>.Failure(err);
            var mapped = failed.MapError(e => e.WithMessage("ctx"));
            mapped.IsFailure.Should().BeTrue();
            mapped.Error.Code.Should().Be(err.Code);
            mapped.Error.Message.Should().StartWith("ctx: ");
        });
    }

    // -------- Tap / TapError --------

    [Fact]
    public void Property_Tap_PreservesResult_AndOnlyRunsOnSuccess()
    {
        ResultGen.Sample(r =>
        {
            var ran = false;
            var tapped = r.Tap(_ => ran = true);
            tapped.Should().Be(r);
            ran.Should().Be(r.IsSuccess);
        });
    }

    [Fact]
    public void Property_TapError_PreservesResult_AndOnlyRunsOnFailure()
    {
        ResultGen.Sample(r =>
        {
            var ran = false;
            var tapped = r.TapError(_ => ran = true);
            tapped.Should().Be(r);
            ran.Should().Be(r.IsFailure);
        });
    }
}
