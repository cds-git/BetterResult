namespace BetterResult.Tests.Operations;

public class CombineTests
{
    // 2 parameter tests
    [Fact]
    public void Combine2_Should_ReturnTuple_When_BothSucceed()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var result2 = Result<string>.Success("two");

        // Act
        var combined = Result.Combine(result1, result2);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, "two"));
    }

    [Fact]
    public void Combine2_Should_ReturnFirstError_When_FirstFails()
    {
        // Arrange
        var error1 = Error.Validation("E1", "Error 1");
        var result1 = Result<int>.Failure(error1);
        var result2 = Result<string>.Success("two");

        // Act
        var combined = Result.Combine(result1, result2);

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error1);
    }

    [Fact]
    public void Combine2_Should_ReturnSecondError_When_OnlySecondFails()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var error2 = Error.Validation("E2", "Error 2");
        var result2 = Result<string>.Failure(error2);

        // Act
        var combined = Result.Combine(result1, result2);

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error2);
    }

    [Fact]
    public void Combine2WithSelector_Should_ApplySelector_When_BothSucceed()
    {
        // Arrange
        var result1 = Result<int>.Success(10);
        var result2 = Result<int>.Success(5);

        // Act
        var combined = Result.Combine(result1, result2, (a, b) => a + b);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(15);
    }

    [Fact]
    public void Combine2WithSelector_Should_ReturnError_When_AnyFails()
    {
        // Arrange
        var error = Error.Validation("E1", "Error");
        var result1 = Result<int>.Failure(error);
        var result2 = Result<int>.Success(5);

        // Act
        var combined = Result.Combine(result1, result2, (a, b) => a + b);

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error);
    }

    // 3 parameter tests
    [Fact]
    public void Combine3_Should_ReturnTuple_When_AllSucceed()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var result2 = Result<string>.Success("two");
        var result3 = Result<bool>.Success(true);

        // Act
        var combined = Result.Combine(result1, result2, result3);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, "two", true));
    }

    [Fact]
    public void Combine3WithSelector_Should_ApplySelector_When_AllSucceed()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var result2 = Result<int>.Success(2);
        var result3 = Result<int>.Success(3);

        // Act
        var combined = Result.Combine(result1, result2, result3, (a, b, c) => a + b + c);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(6);
    }

    [Fact]
    public void Combine3_Should_ReturnFirstError_When_AnyFails()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var error2 = Error.Validation("E2", "Error 2");
        var result2 = Result<string>.Failure(error2);
        var result3 = Result<bool>.Success(true);

        // Act
        var combined = Result.Combine(result1, result2, result3);

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error2);
    }

    // 4 parameter tests
    [Fact]
    public void Combine4_Should_ReturnTuple_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);

        // Act
        var combined = Result.Combine(r1, r2, r3, r4);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, 2, 3, 4));
    }

    [Fact]
    public void Combine4WithSelector_Should_ApplySelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);

        // Act
        var combined = Result.Combine(r1, r2, r3, r4, (a, b, c, d) => a + b + c + d);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(10);
    }

    // 5 parameter tests
    [Fact]
    public void Combine5_Should_ReturnTuple_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);

        // Act
        var combined = Result.Combine(r1, r2, r3, r4, r5);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, 2, 3, 4, 5));
    }

    [Fact]
    public void Combine5WithSelector_Should_ApplySelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);

        // Act
        var combined = Result.Combine(r1, r2, r3, r4, r5, (a, b, c, d, e) => a + b + c + d + e);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(15);
    }

    // 6 parameter tests
    [Fact]
    public void Combine6_Should_ReturnTuple_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);

        // Act
        var combined = Result.Combine(r1, r2, r3, r4, r5, r6);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, 2, 3, 4, 5, 6));
    }

    [Fact]
    public void Combine6WithSelector_Should_ApplySelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);

        // Act
        var combined = Result.Combine(r1, r2, r3, r4, r5, r6, (a, b, c, d, e, f) => a + b + c + d + e + f);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(21);
    }

    // 7 parameter tests
    [Fact]
    public void Combine7_Should_ReturnTuple_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);
        var r7 = Result<int>.Success(7);

        // Act
        var combined = Result.Combine(r1, r2, r3, r4, r5, r6, r7);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, 2, 3, 4, 5, 6, 7));
    }

    [Fact]
    public void Combine7WithSelector_Should_ApplySelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);
        var r7 = Result<int>.Success(7);

        // Act
        var combined = Result.Combine(r1, r2, r3, r4, r5, r6, r7, (a, b, c, d, e, f, g) => a + b + c + d + e + f + g);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(28);
    }

    // 8 parameter tests
    [Fact]
    public void Combine8_Should_ReturnTuple_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);
        var r7 = Result<int>.Success(7);
        var r8 = Result<int>.Success(8);

        // Act
        var combined = Result.Combine(r1, r2, r3, r4, r5, r6, r7, r8);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, 2, 3, 4, 5, 6, 7, 8));
    }

    [Fact]
    public void Combine8WithSelector_Should_ApplySelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);
        var r7 = Result<int>.Success(7);
        var r8 = Result<int>.Success(8);

        // Act
        var combined = Result.Combine(r1, r2, r3, r4, r5, r6, r7, r8, (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(36);
    }

    // Real-world scenario tests
    [Fact]
    public void Combine_Should_WorkWithDifferentTypes()
    {
        // Arrange
        var user = Result<User>.Success(new User { Id = 1, Name = "Alice" });
        var settings = Result<Settings>.Success(new Settings { Theme = "Dark" });
        var permissions = Result<Permissions>.Success(new Permissions { CanEdit = true });

        // Act
        var combined = Result.Combine(
            user,
            settings,
            permissions,
            (u, s, p) => new Dashboard { User = u, Settings = s, Permissions = p }
        );

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.User.Name.Should().Be("Alice");
        combined.Value.Settings.Theme.Should().Be("Dark");
        combined.Value.Permissions.CanEdit.Should().BeTrue();
    }

    [Fact]
    public async Task Combine_Should_WorkInParallelPattern()
    {
        // Arrange - simulating parallel async operations
        var userTask = Task.FromResult(Result<User>.Success(new User { Id = 1, Name = "Alice" }));
        var settingsTask = Task.FromResult(Result<Settings>.Success(new Settings { Theme = "Dark" }));

        // Act - wait for all, then combine
        await Task.WhenAll(userTask, settingsTask);
        var combined = Result.Combine(
            await userTask,
            await settingsTask,
            (u, s) => new { User = u, Settings = s }
        );

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.User.Name.Should().Be("Alice");
    }

    [Fact]
    public void Combine_Should_ShortCircuitOnFirstError()
    {
        // Arrange
        var error1 = Error.Validation("E1", "First error");
        var error2 = Error.Validation("E2", "Second error");
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Failure(error1);
        var r3 = Result<int>.Failure(error2);

        // Act
        var combined = Result.Combine(r1, r2, r3);

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error1); // First error encountered
    }

    // CombineAsync with async selector tests
    [Fact]
    public async Task CombineAsync2_Should_ApplyAsyncSelector_When_BothSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(10);
        var r2 = Result<int>.Success(5);

        // Act
        var combined = await Result.CombineAsync(r1, r2, async (a, b) =>
        {
            await Task.Delay(1); // Simulate async work
            return a + b;
        });

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(15);
    }

    [Fact]
    public async Task CombineAsync2_Should_ReturnError_When_FirstFails()
    {
        // Arrange
        var error = Error.Validation("E1", "Error");
        var r1 = Result<int>.Failure(error);
        var r2 = Result<int>.Success(5);

        // Act
        var combined = await Result.CombineAsync(r1, r2, async (a, b) =>
        {
            await Task.Delay(1);
            return a + b;
        });

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error);
    }

    [Fact]
    public async Task CombineAsync3_Should_ApplyAsyncSelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);

        // Act
        var combined = await Result.CombineAsync(r1, r2, r3, async (a, b, c) =>
        {
            await Task.Delay(1);
            return a + b + c;
        });

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(6);
    }

    [Fact]
    public async Task CombineAsync4_Should_ApplyAsyncSelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);

        // Act
        var combined = await Result.CombineAsync(r1, r2, r3, r4, async (a, b, c, d) =>
        {
            await Task.Delay(1);
            return a + b + c + d;
        });

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(10);
    }

    [Fact]
    public async Task CombineAsync5_Should_ApplyAsyncSelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);

        // Act
        var combined = await Result.CombineAsync(r1, r2, r3, r4, r5, async (a, b, c, d, e) =>
        {
            await Task.Delay(1);
            return a + b + c + d + e;
        });

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(15);
    }

    [Fact]
    public async Task CombineAsync6_Should_ApplyAsyncSelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);

        // Act
        var combined = await Result.CombineAsync(r1, r2, r3, r4, r5, r6, async (a, b, c, d, e, f) =>
        {
            await Task.Delay(1);
            return a + b + c + d + e + f;
        });

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(21);
    }

    [Fact]
    public async Task CombineAsync7_Should_ApplyAsyncSelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);
        var r7 = Result<int>.Success(7);

        // Act
        var combined = await Result.CombineAsync(r1, r2, r3, r4, r5, r6, r7, async (a, b, c, d, e, f, g) =>
        {
            await Task.Delay(1);
            return a + b + c + d + e + f + g;
        });

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(28);
    }

    [Fact]
    public async Task CombineAsync8_Should_ApplyAsyncSelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);
        var r7 = Result<int>.Success(7);
        var r8 = Result<int>.Success(8);

        // Act
        var combined = await Result.CombineAsync(r1, r2, r3, r4, r5, r6, r7, r8, async (a, b, c, d, e, f, g, h) =>
        {
            await Task.Delay(1);
            return a + b + c + d + e + f + g + h;
        });

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(36);
    }

    [Fact]
    public async Task CombineAsync_Should_WorkWithComplexAsyncOperations()
    {
        // Arrange
        var user = Result<User>.Success(new User { Id = 1, Name = "Alice" });
        var settings = Result<Settings>.Success(new Settings { Theme = "Dark" });

        // Act
        var combined = await Result.CombineAsync(
            user,
            settings,
            async (u, s) =>
            {
                await Task.Delay(1); // Simulate async processing
                return new Dashboard
                {
                    User = u,
                    Settings = s,
                    Permissions = new Permissions { CanEdit = true }
                };
            });

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.User.Name.Should().Be("Alice");
        combined.Value.Settings.Theme.Should().Be("Dark");
    }

    [Fact]
    public async Task CombineAsync_Should_ShortCircuit_When_ErrorOccurs()
    {
        // Arrange
        var error = Error.Validation("E1", "Error");
        var r1 = Result<int>.Failure(error);
        var r2 = Result<int>.Success(2);
        var selectorCalled = false;

        // Act
        var combined = await Result.CombineAsync(r1, r2, async (a, b) =>
        {
            selectorCalled = true;
            await Task.Delay(1);
            return a + b;
        });

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error);
        selectorCalled.Should().BeFalse(); // Selector should not be called
    }

    // Helper classes for testing
    private record User
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    private record Settings
    {
        public string Theme { get; init; } = string.Empty;
    }

    private record Permissions
    {
        public bool CanEdit { get; init; }
    }

    private record Dashboard
    {
        public User User { get; init; } = null!;
        public Settings Settings { get; init; } = null!;
        public Permissions Permissions { get; init; } = null!;
    }
}
