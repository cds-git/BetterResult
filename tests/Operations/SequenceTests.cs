namespace BetterResult.Tests.Operations;

public class SequenceTests
{
    [Fact]
    public void Sequence_Should_ReturnSuccessWithAllValues_When_AllResultsAreSuccess()
    {
        // Arrange
        var results = new[]
        {
            Result<int>.Success(1),
            Result<int>.Success(2),
            Result<int>.Success(3)
        };

        // Act
        var sequenced = results.Sequence();

        // Assert
        sequenced.IsSuccess.Should().BeTrue();
        sequenced.Value.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public void Sequence_Should_ReturnFirstError_When_OneResultIsFailure()
    {
        // Arrange
        var error1 = Error.Validation("ERROR1", "First error");
        var error2 = Error.Validation("ERROR2", "Second error");
        var results = new[]
        {
            Result<int>.Success(1),
            Result<int>.Failure(error1),
            Result<int>.Success(3),
            Result<int>.Failure(error2)
        };

        // Act
        var sequenced = results.Sequence();

        // Assert
        sequenced.IsFailure.Should().BeTrue();
        sequenced.Error.Should().Be(error1);
    }

    [Fact]
    public void Sequence_Should_ReturnError_When_AllResultsAreFailure()
    {
        // Arrange
        var error1 = Error.Validation("ERROR1", "First error");
        var error2 = Error.Validation("ERROR2", "Second error");
        var results = new[]
        {
            Result<int>.Failure(error1),
            Result<int>.Failure(error2)
        };

        // Act
        var sequenced = results.Sequence();

        // Assert
        sequenced.IsFailure.Should().BeTrue();
        sequenced.Error.Should().Be(error1);
    }

    [Fact]
    public void Sequence_Should_ReturnEmptyList_When_CollectionIsEmpty()
    {
        // Arrange
        var results = Array.Empty<Result<int>>();

        // Act
        var sequenced = results.Sequence();

        // Assert
        sequenced.IsSuccess.Should().BeTrue();
        sequenced.Value.Should().BeEmpty();
    }

    [Fact]
    public void Sequence_Should_ReturnSingleValue_When_CollectionHasSingleSuccess()
    {
        // Arrange
        var results = new[] { Result<int>.Success(42) };

        // Act
        var sequenced = results.Sequence();

        // Assert
        sequenced.IsSuccess.Should().BeTrue();
        sequenced.Value.Should().BeEquivalentTo(new[] { 42 });
    }

    [Fact]
    public void Sequence_Should_ShortCircuit_When_FirstResultIsFailure()
    {
        // Arrange
        var error = Error.Validation("ERROR", "Error");
        var results = new[]
        {
            Result<int>.Failure(error),
            Result<int>.Success(1),
            Result<int>.Success(2)
        };

        // Act
        var sequenced = results.Sequence();

        // Assert
        sequenced.IsFailure.Should().BeTrue();
        sequenced.Error.Should().Be(error);
    }

    [Fact]
    public void Sequence_Should_WorkWithDifferentTypes()
    {
        // Arrange
        var results = new[]
        {
            Result<string>.Success("Hello"),
            Result<string>.Success("World")
        };

        // Act
        var sequenced = results.Sequence();

        // Assert
        sequenced.IsSuccess.Should().BeTrue();
        sequenced.Value.Should().BeEquivalentTo(new[] { "Hello", "World" });
    }

    [Fact]
    public async Task SequenceAsync_Should_ReturnSuccessWithAllValues_When_AllResultsAreSuccess()
    {
        // Arrange
        var results = new[]
        {
            Task.FromResult(Result<int>.Success(1)),
            Task.FromResult(Result<int>.Success(2)),
            Task.FromResult(Result<int>.Success(3))
        };

        // Act
        var sequenced = await results.SequenceAsync();

        // Assert
        sequenced.IsSuccess.Should().BeTrue();
        sequenced.Value.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task SequenceAsync_Should_ReturnFirstError_When_OneResultIsFailure()
    {
        // Arrange
        var error1 = Error.Validation("ERROR1", "First error");
        var error2 = Error.Validation("ERROR2", "Second error");
        var results = new[]
        {
            Task.FromResult(Result<int>.Success(1)),
            Task.FromResult(Result<int>.Failure(error1)),
            Task.FromResult(Result<int>.Success(3)),
            Task.FromResult(Result<int>.Failure(error2))
        };

        // Act
        var sequenced = await results.SequenceAsync();

        // Assert
        sequenced.IsFailure.Should().BeTrue();
        sequenced.Error.Should().Be(error1);
    }

    [Fact]
    public async Task SequenceAsync_Should_ReturnEmptyList_When_CollectionIsEmpty()
    {
        // Arrange
        var results = Array.Empty<Task<Result<int>>>();

        // Act
        var sequenced = await results.SequenceAsync();

        // Assert
        sequenced.IsSuccess.Should().BeTrue();
        sequenced.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task SequenceAsync_Should_ShortCircuit_When_FirstResultIsFailure()
    {
        // Arrange
        var error = Error.Validation("ERROR", "Error");
        var executionCount = 0;
        var results = new[]
        {
            Task.FromResult(Result<int>.Failure(error)),
            Task.Run(() => { executionCount++; return Result<int>.Success(1); }),
            Task.Run(() => { executionCount++; return Result<int>.Success(2); })
        };

        // Act
        var sequenced = await results.SequenceAsync();

        // Assert
        sequenced.IsFailure.Should().BeTrue();
        sequenced.Error.Should().Be(error);
        // Note: Tasks may start execution, but sequencing stops processing results after first failure
    }

    [Fact]
    public void Sequence_Should_WorkInPipeline()
    {
        // Arrange
        var userIds = new[] { 1, 2, 3 };
        
        // Act
        var result = userIds
            .Select(id => GetUser(id))
            .ToList()
            .Sequence();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
        result.Value.Select(u => u.Id).Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public void Sequence_Should_FailInPipeline_When_OneUserNotFound()
    {
        // Arrange
        var userIds = new[] { 1, 999, 3 }; // 999 doesn't exist
        
        // Act
        var result = userIds
            .Select(id => GetUser(id))
            .ToList()
            .Sequence();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("NOT_FOUND");
    }

    [Fact]
    public void Sequence_Should_ReturnReadOnlyList()
    {
        // Arrange
        var results = new[]
        {
            Result<int>.Success(1),
            Result<int>.Success(2)
        };

        // Act
        var sequenced = results.Sequence();

        // Assert
        sequenced.IsSuccess.Should().BeTrue();
        sequenced.Value.Should().BeAssignableTo<IReadOnlyList<int>>();
    }

    // Helper methods for testing
    private static Result<User> GetUser(int id)
    {
        if (id == 999)
            return Error.NotFound("NOT_FOUND", "User not found");
        
        return new User { Id = id, Name = $"User{id}" };
    }

    [Fact]
    public async Task TaskSequence_Should_AwaitAndSequence_When_TaskWrapsResults()
    {
        // Arrange
        var resultsTask = Task.FromResult<IEnumerable<Result<int>>>(new[]
        {
            Result<int>.Success(1),
            Result<int>.Success(2),
            Result<int>.Success(3)
        });

        // Act
        var sequenced = await resultsTask.Sequence();

        // Assert
        sequenced.IsSuccess.Should().BeTrue();
        sequenced.Value.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task TaskSequence_Should_ReturnFirstError_When_TaskWrapsResultsWithFailure()
    {
        // Arrange
        var error = Error.Validation("ERROR", "Error");
        var resultsTask = Task.FromResult<IEnumerable<Result<int>>>(new[]
        {
            Result<int>.Success(1),
            Result<int>.Failure(error),
            Result<int>.Success(3)
        });

        // Act
        var sequenced = await resultsTask.Sequence();

        // Assert
        sequenced.IsFailure.Should().BeTrue();
        sequenced.Error.Should().Be(error);
    }

    [Fact]
    public async Task TaskSequenceAsync_Should_AwaitOuterTaskAndSequenceInnerTasks()
    {
        // Arrange
        var tasksTask = Task.FromResult<IEnumerable<Task<Result<int>>>>(new[]
        {
            Task.FromResult(Result<int>.Success(1)),
            Task.FromResult(Result<int>.Success(2)),
            Task.FromResult(Result<int>.Success(3))
        });

        // Act
        var sequenced = await tasksTask.SequenceAsync();

        // Assert
        sequenced.IsSuccess.Should().BeTrue();
        sequenced.Value.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public async Task TaskExtensions_Should_EnableFluentChaining()
    {
        // Arrange - simulating a pipeline
        Task<IEnumerable<Result<int>>> GetResultsAsync() =>
            Task.FromResult<IEnumerable<Result<int>>>(new[]
            {
                Result<int>.Success(1),
                Result<int>.Success(2),
                Result<int>.Success(3)
            });

        // Act - fluent chaining without intermediate await
        var result = await GetResultsAsync().Sequence();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
    }

    private record User
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
