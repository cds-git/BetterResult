namespace BetterResult.Tests.Operations;

public class ZipTests
{
    [Fact]
    public void Zip2_Should_ReturnTuple_When_BothSucceed()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var result2 = Result<string>.Success("two");

        // Act
        var combined = result1.Zip(result2);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, "two"));
    }

    [Fact]
    public void Zip2_Should_ReturnFirstError_When_FirstFails()
    {
        // Arrange
        var error1 = Error.Validation("E1", "Error 1");
        var result1 = Result<int>.Failure(error1);
        var result2 = Result<string>.Success("two");

        // Act
        var combined = result1.Zip(result2);

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error1);
    }

    [Fact]
    public void Zip2_Should_ReturnSecondError_When_OnlySecondFails()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var error2 = Error.Validation("E2", "Error 2");
        var result2 = Result<string>.Failure(error2);

        // Act
        var combined = result1.Zip(result2);

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error2);
    }

    [Fact]
    public void Zip2WithSelector_Should_ApplySelector_When_BothSucceed()
    {
        // Arrange
        var result1 = Result<int>.Success(10);
        var result2 = Result<int>.Success(5);

        // Act
        var combined = result1.Zip(result2, (a, b) => a + b);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(15);
    }

    [Fact]
    public void Zip2WithSelector_Should_ReturnError_When_AnyFails()
    {
        // Arrange
        var error = Error.Validation("E1", "Error");
        var result1 = Result<int>.Failure(error);
        var result2 = Result<int>.Success(5);

        // Act
        var combined = result1.Zip(result2, (a, b) => a + b);

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error);
    }

    // 3 parameter tests
    [Fact]
    public void Zip3_Should_ReturnTuple_When_AllSucceed()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var result2 = Result<string>.Success("two");
        var result3 = Result<bool>.Success(true);

        // Act
        var combined = result1.Zip(result2, result3);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, "two", true));
    }

    [Fact]
    public void Zip3WithSelector_Should_ApplySelector_When_AllSucceed()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var result2 = Result<int>.Success(2);
        var result3 = Result<int>.Success(3);

        // Act
        var combined = result1.Zip(result2, result3, (a, b, c) => a + b + c);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(6);
    }

    [Fact]
    public void Zip3_Should_ReturnFirstError_When_AnyFails()
    {
        // Arrange
        var result1 = Result<int>.Success(1);
        var error2 = Error.Validation("E2", "Error 2");
        var result2 = Result<string>.Failure(error2);
        var result3 = Result<bool>.Success(true);

        // Act
        var combined = result1.Zip(result2, result3);

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Error.Should().Be(error2);
    }

    // 4 parameter tests
    [Fact]
    public void Zip4_Should_ReturnTuple_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);

        // Act
        var combined = r1.Zip(r2, r3, r4);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, 2, 3, 4));
    }

    [Fact]
    public void Zip4WithSelector_Should_ApplySelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);

        // Act
        var combined = r1.Zip(r2, r3, r4, (a, b, c, d) => a + b + c + d);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(10);
    }

    // 5 parameter tests
    [Fact]
    public void Zip5_Should_ReturnTuple_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);

        // Act
        var combined = r1.Zip(r2, r3, r4, r5);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, 2, 3, 4, 5));
    }

    [Fact]
    public void Zip5WithSelector_Should_ApplySelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);

        // Act
        var combined = r1.Zip(r2, r3, r4, r5, (a, b, c, d, e) => a + b + c + d + e);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(15);
    }

    // 6 parameter tests
    [Fact]
    public void Zip6_Should_ReturnTuple_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);

        // Act
        var combined = r1.Zip(r2, r3, r4, r5, r6);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, 2, 3, 4, 5, 6));
    }

    [Fact]
    public void Zip6WithSelector_Should_ApplySelector_When_AllSucceed()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);
        var r4 = Result<int>.Success(4);
        var r5 = Result<int>.Success(5);
        var r6 = Result<int>.Success(6);

        // Act
        var combined = r1.Zip(r2, r3, r4, r5, r6, (a, b, c, d, e, f) => a + b + c + d + e + f);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(21);
    }

    // 7 parameter tests
    [Fact]
    public void Zip7_Should_ReturnTuple_When_AllSucceed()
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
        var combined = r1.Zip(r2, r3, r4, r5, r6, r7);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, 2, 3, 4, 5, 6, 7));
    }

    [Fact]
    public void Zip7WithSelector_Should_ApplySelector_When_AllSucceed()
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
        var combined = r1.Zip(r2, r3, r4, r5, r6, r7, (a, b, c, d, e, f, g) => a + b + c + d + e + f + g);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(28);
    }

    // 8 parameter tests
    [Fact]
    public void Zip8_Should_ReturnTuple_When_AllSucceed()
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
        var combined = r1.Zip(r2, r3, r4, r5, r6, r7, r8);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be((1, 2, 3, 4, 5, 6, 7, 8));
    }

    [Fact]
    public void Zip8WithSelector_Should_ApplySelector_When_AllSucceed()
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
        var combined = r1.Zip(r2, r3, r4, r5, r6, r7, r8, (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h);

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Be(36);
    }

    // Real-world scenario tests
    [Fact]
    public void ZipWithSelector_Should_AcceptDifferentTypes()
    {
        // Arrange
        var user = Result<User>.Success(new User { Id = 1, Name = "Alice" });
        var settings = Result<Settings>.Success(new Settings { Theme = "Dark" });
        var permissions = Result<Permissions>.Success(new Permissions { CanEdit = true });

        // Act
        var combined = user.Zip(
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
    public async Task Zip_Should_CombineAwaitedResults()
    {
        // Arrange - simulating parallel async operations
        var userTask = Task.FromResult(Result<User>.Success(new User { Id = 1, Name = "Alice" }));
        var settingsTask = Task.FromResult(Result<Settings>.Success(new Settings { Theme = "Dark" }));

        // Act - wait for all, then combine
        await Task.WhenAll(userTask, settingsTask);
        var combined = (await userTask).Zip(
            await settingsTask,
            (u, s) => new { User = u, Settings = s });

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.User.Name.Should().Be("Alice");
    }

    [Fact]
    public void Zip_Should_CollectEveryError_When_SeveralFail()
    {
        var error1 = Error.Validation("E1", "First error");
        var error2 = Error.Validation("E2", "Second error");
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Failure(error1);
        var r3 = Result<int>.Failure(error2);

        var combined = r1.Zip(r2, r3);

        combined.IsFailure.Should().BeTrue();
        combined.Error.Type.Should().Be(ErrorType.Validation);
        combined.Error.Code.Should().Be("General.Multiple");
        combined.Error.Should().BeOfType<ErrorList>().Which.Errors.Should().Equal(error1, error2);
    }

    [Fact]
    public void Zip_Should_NotRunSelector_When_AnyFails()
    {
        var selectorCalled = false;

        var combined = Result<int>.Failure(Error.Validation("E1", "Error")).Zip(
            Result<int>.Success(2),
            Result<int>.Failure(Error.Conflict("E2", "Error")),
            (a, b, c) =>
            {
                selectorCalled = true;
                return a + b + c;
            });

        combined.IsFailure.Should().BeTrue();
        combined.Error.Type.Should().Be(ErrorType.Failure);
        combined.Error.Should().BeOfType<ErrorList>().Which.Errors.Should().HaveCount(2);
        selectorCalled.Should().BeFalse();
    }

    [Fact]
    public void Zip8_Should_CollectEveryError_When_AllFail()
    {
        var errors = Enumerable.Range(1, 8).Select(i => Error.Validation($"E{i}", "Error")).ToArray();

        var combined = Result<int>.Failure(errors[0]).Zip(
            Result<int>.Failure(errors[1]),
            Result<int>.Failure(errors[2]), Result<int>.Failure(errors[3]),
            Result<int>.Failure(errors[4]), Result<int>.Failure(errors[5]),
            Result<int>.Failure(errors[6]), Result<int>.Failure(errors[7]));

        combined.Error.Should().BeOfType<ErrorList>().Which.Errors.Should().Equal(errors);
    }

    [Fact]
    public void Zip_Should_CollectBothErrors_When_BothFail()
    {
        var error1 = Error.Validation("E1", "Error 1");
        var error2 = Error.Validation("E2", "Error 2");

        var zipped = Result<int>.Failure(error1).Zip(Result<string>.Failure(error2));

        zipped.Error.Should().BeOfType<ErrorList>().Which.Errors.Should().Equal(error1, error2);
    }

    [Fact]
    public void Zip_Should_WorkInFluentChain()
    {
        // Arrange
        var user = GetUser(1);
        var settings = GetSettings(1);

        // Act
        var result = user.Zip(settings, (u, s) => new
        {
            UserName = u.Name,
            Theme = s.Theme
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserName.Should().Be("User1");
        result.Value.Theme.Should().Be("Dark");
    }

    [Fact]
    public void Zip_Should_ChainMultipleTimes()
    {
        // Arrange
        var r1 = Result<int>.Success(1);
        var r2 = Result<int>.Success(2);
        var r3 = Result<int>.Success(3);

        // Act - chaining Zip operations
        var result = r1
            .Zip(r2, (a, b) => a + b)  // 1 + 2 = 3
            .Zip(r3, (ab, c) => ab + c); // 3 + 3 = 6

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(6);
    }

    [Fact]
    public void Zip_Should_WorkWithDifferentTypes()
    {
        // Arrange
        var intResult = Result<int>.Success(42);
        var stringResult = Result<string>.Success("answer");
        var boolResult = Result<bool>.Success(true);

        // Act
        var zipped = intResult
            .Zip(stringResult, (i, s) => $"{s}: {i}")
            .Zip(boolResult, (str, b) => (Message: str, IsValid: b));

        // Assert
        zipped.IsSuccess.Should().BeTrue();
        zipped.Value.Message.Should().Be("answer: 42");
        zipped.Value.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Zip_Should_FeedAsyncStep_When_FollowedByMapAsync()
    {
        // Arrange
        async Task<Result<int>> FetchDataAsync()
        {
            await Task.Delay(1);
            return 100;
        }

        async Task<Result<int>> FetchMoreDataAsync()
        {
            await Task.Delay(1);
            return 50;
        }

        // Act
        var result = await (await FetchDataAsync())
            .Zip(await FetchMoreDataAsync())
            .MapAsync(async pair =>
            {
                await Task.Delay(1);
                return pair.Item1 + pair.Item2;
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(150);
    }

    [Fact]
    public async Task ZipAsync_Should_ReturnTuple_When_BothTasksSucceed()
    {
        var zipped = await Task.FromResult(Result<int>.Success(1)).ZipAsync(Task.FromResult(Result<string>.Success("a")));

        zipped.Value.Should().Be((1, "a"));
    }

    [Fact]
    public async Task ZipAsync_Should_ApplySelector_When_BothTasksSucceed()
    {
        var zipped = await Task.FromResult(Result<int>.Success(1))
            .ZipAsync(Task.FromResult(Result<int>.Success(2)), (a, b) => a + b);

        zipped.Value.Should().Be(3);
    }

    [Fact]
    public async Task ZipAsync_Should_ReturnTuple_When_SecondIsAResult()
    {
        var zipped = await Task.FromResult(Result<int>.Success(1)).ZipAsync(Result<string>.Success("a"));

        zipped.Value.Should().Be((1, "a"));
    }

    [Fact]
    public async Task ZipAsync_Should_ApplySelector_When_SecondIsAResult()
    {
        var zipped = await Task.FromResult(Result<int>.Success(1)).ZipAsync(Result<int>.Success(2), (a, b) => a + b);

        zipped.Value.Should().Be(3);
    }

    [Fact]
    public async Task ZipAsync_Should_CollectBothErrors_When_BothTasksFail()
    {
        var error1 = Error.Validation("E1", "Error 1");
        var error2 = Error.Validation("E2", "Error 2");

        var zipped = await Task.FromResult(Result<int>.Failure(error1))
            .ZipAsync(Task.FromResult(Result<int>.Failure(error2)), (a, b) => a + b);

        zipped.Error.Should().BeOfType<ErrorList>().Which.Errors.Should().Equal(error1, error2);
    }

    private static Result<User> GetUser(int id) =>
        new User { Id = id, Name = $"User{id}" };

    private static Result<Settings> GetSettings(int id) =>
        new Settings { Theme = "Dark" };

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
