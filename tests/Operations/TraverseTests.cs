namespace BetterResult.Tests.Operations;

public class TraverseTests
{
    [Fact]
    public void Traverse_Should_ReturnSuccessWithAllTransformedValues_When_AllSelectorsSucceed()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };

        // Act
        var result = Result.Traverse(numbers, n => Result<int>.Success(n * 2));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new[] { 2, 4, 6 });
    }

    [Fact]
    public void Traverse_Should_ReturnFirstError_When_OneSelectorFails()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };
        var error1 = Error.Validation("ERROR1", "First error");
        var error2 = Error.Validation("ERROR2", "Second error");

        // Act
        var result = Result.Traverse(numbers, n => n switch
        {
            2 => Result<int>.Failure(error1),
            4 => Result<int>.Failure(error2),
            _ => Result<int>.Success(n * 2)
        });

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error1);
    }

    [Fact]
    public void Traverse_Should_ReturnEmptyList_When_SourceIsEmpty()
    {
        // Arrange
        var numbers = Array.Empty<int>();

        // Act
        var result = Result.Traverse(numbers, n => Result<int>.Success(n * 2));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public void Traverse_Should_ShortCircuit_When_FirstSelectorFails()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };
        var error = Error.Validation("ERROR", "Error");
        var executionCount = 0;

        // Act
        var result = Result.Traverse(numbers, n =>
        {
            executionCount++;
            return n == 1 ? Result<int>.Failure(error) : Result<int>.Success(n);
        });

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
        executionCount.Should().Be(1); // Should only execute for first item
    }

    [Fact]
    public void Traverse_Should_WorkWithDifferentTypes()
    {
        // Arrange
        var userIds = new[] { 1, 2, 3 };

        // Act
        var result = Result.Traverse(userIds, id => Result<User>.Success(new User { Id = id, Name = $"User{id}" }));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
        result.Value.Select(u => u.Id).Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public void Traverse_Should_WorkInPipeline()
    {
        // Arrange
        var userIds = new[] { 1, 2, 3 };

        // Act
        var result = Result.Traverse(userIds, id => GetUser(id))
            .Map(users => users.Count());

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(3);
    }

    [Fact]
    public void Traverse_Should_FailInPipeline_When_OneSelectorFails()
    {
        // Arrange
        var userIds = new[] { 1, 999, 3 }; // 999 doesn't exist

        // Act
        var result = Result.Traverse(userIds, id => GetUser(id));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("NOT_FOUND");
    }

    [Fact]
    public void Traverse_Should_ReturnReadOnlyList()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };

        // Act
        var result = Result.Traverse(numbers, n => Result<int>.Success(n * 2));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeAssignableTo<IReadOnlyList<int>>();
    }

    [Fact]
    public async Task TraverseAsync_Should_ReturnSuccessWithAllTransformedValues_When_AllSelectorsSucceed()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };

        // Act
        var result = await Result.TraverseAsync(numbers, n => Task.FromResult(Result<int>.Success(n * 2)));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new[] { 2, 4, 6 });
    }

    [Fact]
    public async Task TraverseAsync_Should_ReturnFirstError_When_OneSelectorFails()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };
        var error1 = Error.Validation("ERROR1", "First error");
        var error2 = Error.Validation("ERROR2", "Second error");

        // Act
        var result = await Result.TraverseAsync(numbers, n => Task.FromResult(n switch
        {
            2 => Result<int>.Failure(error1),
            4 => Result<int>.Failure(error2),
            _ => Result<int>.Success(n * 2)
        }));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error1);
    }

    [Fact]
    public async Task TraverseAsync_Should_ReturnEmptyList_When_SourceIsEmpty()
    {
        // Arrange
        var numbers = Array.Empty<int>();

        // Act
        var result = await Result.TraverseAsync(numbers, n => Task.FromResult(Result<int>.Success(n * 2)));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task TraverseAsync_Should_ShortCircuit_When_FirstSelectorFails()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };
        var error = Error.Validation("ERROR", "Error");
        var executionCount = 0;

        // Act
        var result = await Result.TraverseAsync(numbers, async n =>
        {
            executionCount++;
            await Task.Delay(1); // Simulate async work
            return n == 1 ? Result<int>.Failure(error) : Result<int>.Success(n);
        });

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
        executionCount.Should().Be(1); // Should only execute for first item
    }

    [Fact]
    public async Task TraverseAsync_Should_WorkWithDifferentTypes()
    {
        // Arrange
        var userIds = new[] { 1, 2, 3 };

        // Act
        var result = await Result.TraverseAsync(userIds, async id =>
        {
            await Task.Delay(1); // Simulate async work
            return Result<User>.Success(new User { Id = id, Name = $"User{id}" });
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
        result.Value.Select(u => u.Id).Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task TraverseAsync_Should_WorkInPipeline()
    {
        // Arrange
        var userIds = new[] { 1, 2, 3 };

        // Act
        var result = await Result.TraverseAsync(userIds, async id => await GetUserAsync(id))
            .BindAsync(users => Task.FromResult(Result<int>.Success(users.Count())));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(3);
    }

    [Fact]
    public async Task TraverseAsync_Should_FailInPipeline_When_OneSelectorFails()
    {
        // Arrange
        var userIds = new[] { 1, 999, 3 }; // 999 doesn't exist

        // Act
        var result = await Result.TraverseAsync(userIds, async id => await GetUserAsync(id));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("NOT_FOUND");
    }

    [Fact]
    public void Traverse_Should_HandleComplexTransformations()
    {
        // Arrange
        var data = new[] { "1", "2", "3" };

        // Act
        var result = Result.Traverse(data, s =>
        {
            if (int.TryParse(s, out var n))
                return Result<int>.Success(n);
            return Error.Validation("PARSE_ERROR", $"Cannot parse '{s}'");
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public void Traverse_Should_FailWithParseError_When_InvalidInput()
    {
        // Arrange
        var data = new[] { "1", "abc", "3" };

        // Act
        var result = Result.Traverse(data, s =>
        {
            if (int.TryParse(s, out var n))
                return Result<int>.Success(n);
            return Error.Validation("PARSE_ERROR", $"Cannot parse '{s}'");
        });

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("PARSE_ERROR");
        result.Error.Message.Should().Contain("abc");
    }

    [Fact]
    public async Task Traverse_Should_AcceptTaskWrappedCollection_When_UsingOverload()
    {
        // Arrange
        var collectionTask = Task.FromResult<IEnumerable<int>>(new[] { 1, 2, 3 });

        // Act - passing Task directly to static method
        var result = await Result.Traverse(collectionTask, n => Result<int>.Success(n * 2));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new[] { 2, 4, 6 });
    }

    [Fact]
    public async Task Traverse_Should_ReturnFirstError_When_TaskWrappedCollectionAndSelectorFails()
    {
        // Arrange
        var collectionTask = Task.FromResult<IEnumerable<int>>(new[] { 1, 2, 3 });
        var error = Error.Validation("ERROR", "Error at 2");

        // Act
        var result = await Result.Traverse(collectionTask, n =>
            n == 2 ? Result<int>.Failure(error) : Result<int>.Success(n * 2));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task TraverseAsync_Should_AcceptTaskWrappedCollection_When_UsingOverload()
    {
        // Arrange
        var collectionTask = Task.FromResult<IEnumerable<int>>(new[] { 1, 2, 3 });

        // Act - passing Task directly with async selector
        var result = await Result.TraverseAsync(collectionTask, async n =>
        {
            await Task.Delay(1);
            return Result<int>.Success(n * 2);
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new[] { 2, 4, 6 });
    }

    [Fact]
    public async Task TraverseAsync_Should_WorkWithAsyncGetter()
    {
        // Arrange - simulating your exact scenario
        async Task<IEnumerable<int>> GetListAsync()
        {
            await Task.Delay(1);
            return new[] { 1, 2, 3 };
        }

        // Act - Result.TraverseAsync(Task<IEnumerable<T>>, Func<T, Task<Result<U>>>)
        var result = await Result.TraverseAsync(GetListAsync(), async id => await GetUserAsync(id));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
        result.Value.Select(u => u.Id).Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task Traverse_Should_WorkWithAsyncGetterAndSyncSelector()
    {
        // Arrange
        async Task<IEnumerable<int>> GetUserIdsAsync()
        {
            await Task.Delay(1);
            return new[] { 1, 2, 3 };
        }

        // Act - Result.Traverse(Task<IEnumerable<T>>, Func<T, Result<U>>)
        var result = await Result.Traverse(GetUserIdsAsync(), id => GetUser(id));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
    }

    [Fact]
    public async Task TraverseAsync_Should_WorkInComplexPipeline_WithTaskWrappedCollection()
    {
        // Arrange
        async Task<IEnumerable<string>> FetchDataAsync()
        {
            await Task.Delay(1);
            return new[] { "1", "2", "3" };
        }

        // Act - complex pipeline with Task-wrapped collection
        var result = await Result.TraverseAsync(
            FetchDataAsync(),
            async s =>
            {
                await Task.Delay(1);
                if (int.TryParse(s, out var n))
                    return Result<int>.Success(n);
                return Error.Validation("PARSE_ERROR", $"Cannot parse '{s}'");
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    // Helper methods for testing
    private static Result<User> GetUser(int id)
    {
        if (id == 999)
            return Error.NotFound("NOT_FOUND", "User not found");

        return new User { Id = id, Name = $"User{id}" };
    }

    private static async Task<Result<User>> GetUserAsync(int id)
    {
        await Task.Delay(1); // Simulate async work
        return GetUser(id);
    }

    private record User
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
