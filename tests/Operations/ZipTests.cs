namespace BetterResult.Tests.Operations;

public class ZipTests
{
    [Fact]
    public void Zip_Should_ReturnTuple_When_BothSucceed()
    {
        // Arrange
        var result1 = Result<int>.Success(42);
        var result2 = Result<string>.Success("hello");

        // Act
        var zipped = result1.Zip(result2);

        // Assert
        zipped.IsSuccess.Should().BeTrue();
        zipped.Value.Should().Be((42, "hello"));
    }

    [Fact]
    public void Zip_Should_ReturnFirstError_When_FirstFails()
    {
        // Arrange
        var error = Error.Validation("E1", "Error 1");
        var result1 = Result<int>.Failure(error);
        var result2 = Result<string>.Success("hello");

        // Act
        var zipped = result1.Zip(result2);

        // Assert
        zipped.IsFailure.Should().BeTrue();
        zipped.Error.Should().Be(error);
    }

    [Fact]
    public void Zip_Should_ReturnSecondError_When_OnlySecondFails()
    {
        // Arrange
        var result1 = Result<int>.Success(42);
        var error = Error.Validation("E2", "Error 2");
        var result2 = Result<string>.Failure(error);

        // Act
        var zipped = result1.Zip(result2);

        // Assert
        zipped.IsFailure.Should().BeTrue();
        zipped.Error.Should().Be(error);
    }

    [Fact]
    public void ZipWithSelector_Should_ApplySelector_When_BothSucceed()
    {
        // Arrange
        var result1 = Result<int>.Success(10);
        var result2 = Result<int>.Success(5);

        // Act
        var zipped = result1.Zip(result2, (a, b) => a + b);

        // Assert
        zipped.IsSuccess.Should().BeTrue();
        zipped.Value.Should().Be(15);
    }

    [Fact]
    public void ZipWithSelector_Should_ReturnError_When_AnyFails()
    {
        // Arrange
        var error = Error.Validation("E1", "Error");
        var result1 = Result<int>.Failure(error);
        var result2 = Result<int>.Success(5);

        // Act
        var zipped = result1.Zip(result2, (a, b) => a + b);

        // Assert
        zipped.IsFailure.Should().BeTrue();
        zipped.Error.Should().Be(error);
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
    public async Task TaskZip_Should_AwaitAndZip_When_FirstIsTask()
    {
        // Arrange
        var resultTask = Task.FromResult(Result<int>.Success(42));
        var result2 = Result<string>.Success("hello");

        // Act
        var zipped = await resultTask.Zip(result2);

        // Assert
        zipped.IsSuccess.Should().BeTrue();
        zipped.Value.Should().Be((42, "hello"));
    }

    [Fact]
    public async Task TaskZipWithSelector_Should_ApplySelector_When_FirstIsTask()
    {
        // Arrange
        var resultTask = Task.FromResult(Result<int>.Success(10));
        var result2 = Result<int>.Success(5);

        // Act
        var zipped = await resultTask.Zip(result2, (a, b) => a + b);

        // Assert
        zipped.IsSuccess.Should().BeTrue();
        zipped.Value.Should().Be(15);
    }

    [Fact]
    public async Task TaskZip_Should_AwaitBothAndZip_When_BothAreTasks()
    {
        // Arrange
        var resultTask1 = Task.FromResult(Result<int>.Success(42));
        var resultTask2 = Task.FromResult(Result<string>.Success("hello"));

        // Act
        var zipped = await resultTask1.Zip(resultTask2);

        // Assert
        zipped.IsSuccess.Should().BeTrue();
        zipped.Value.Should().Be((42, "hello"));
    }

    [Fact]
    public async Task TaskZipWithSelector_Should_ApplySelector_When_BothAreTasks()
    {
        // Arrange
        var resultTask1 = Task.FromResult(Result<int>.Success(10));
        var resultTask2 = Task.FromResult(Result<int>.Success(5));

        // Act
        var zipped = await resultTask1.Zip(resultTask2, (a, b) => a + b);

        // Assert
        zipped.IsSuccess.Should().BeTrue();
        zipped.Value.Should().Be(15);
    }

    [Fact]
    public async Task ZipAsync_Should_ApplyAsyncSelector_When_BothAreTasks()
    {
        // Arrange
        var resultTask1 = Task.FromResult(Result<int>.Success(10));
        var resultTask2 = Task.FromResult(Result<int>.Success(5));

        // Act
        var zipped = await resultTask1.ZipAsync(resultTask2, async (a, b) =>
        {
            await Task.Delay(1); // Simulate async work
            return a + b;
        });

        // Assert
        zipped.IsSuccess.Should().BeTrue();
        zipped.Value.Should().Be(15);
    }

    [Fact]
    public async Task ZipAsync_Should_ApplyAsyncSelector_When_SecondIsTask()
    {
        // Arrange
        var result1 = Result<int>.Success(10);
        var resultTask2 = Task.FromResult(Result<int>.Success(5));

        // Act
        var zipped = await result1.ZipAsync(resultTask2, async (a, b) =>
        {
            await Task.Delay(1); // Simulate async work
            return a + b;
        });

        // Assert
        zipped.IsSuccess.Should().BeTrue();
        zipped.Value.Should().Be(15);
    }

    [Fact]
    public async Task ZipAsync_Should_ReturnError_When_FirstFails()
    {
        // Arrange
        var error = Error.Validation("E1", "Error");
        var resultTask1 = Task.FromResult(Result<int>.Failure(error));
        var resultTask2 = Task.FromResult(Result<int>.Success(5));

        // Act
        var zipped = await resultTask1.ZipAsync(resultTask2, async (a, b) =>
        {
            await Task.Delay(1);
            return a + b;
        });

        // Assert
        zipped.IsFailure.Should().BeTrue();
        zipped.Error.Should().Be(error);
    }

    [Fact]
    public async Task Zip_Should_WorkInAsyncPipeline()
    {
        // Arrange
        async Task<Result<User>> GetUserAsync(int id)
        {
            await Task.Delay(1);
            return new User { Id = id, Name = $"User{id}" };
        }

        async Task<Result<Settings>> GetSettingsAsync(int id)
        {
            await Task.Delay(1);
            return new Settings { Theme = "Dark" };
        }

        // Act
        var result = await GetUserAsync(1)
            .Zip(await GetSettingsAsync(1), (u, s) => new
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
    public async Task Zip_Should_WorkWithComplexAsyncPipeline()
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

        // Act - combining multiple async operations
        var result = await FetchDataAsync()
            .ZipAsync(FetchMoreDataAsync(), async (a, b) =>
            {
                await Task.Delay(1); // Simulate processing
                return a + b;
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(150);
    }

    // Helper methods
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
}
