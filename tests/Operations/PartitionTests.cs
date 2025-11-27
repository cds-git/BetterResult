namespace BetterResult.Tests.Operations;

public class PartitionTests
{
    [Fact]
    public void Partition_Should_SeparateSuccessesFromFailures()
    {
        // Arrange
        var results = new[]
        {
            Result<int>.Success(1),
            Result<int>.Failure(Error.Validation("E1", "Error 1")),
            Result<int>.Success(2),
            Result<int>.Failure(Error.Validation("E2", "Error 2")),
            Result<int>.Success(3)
        };

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        successes.Should().BeEquivalentTo(new[] { 1, 2, 3 });
        failures.Should().HaveCount(2);
        failures.Select(e => e.Code).Should().BeEquivalentTo(new[] { "E1", "E2" });
    }

    [Fact]
    public void Partition_Should_ReturnAllSuccesses_When_NoFailures()
    {
        // Arrange
        var results = new[]
        {
            Result<int>.Success(1),
            Result<int>.Success(2),
            Result<int>.Success(3)
        };

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        successes.Should().BeEquivalentTo(new[] { 1, 2, 3 });
        failures.Should().BeEmpty();
    }

    [Fact]
    public void Partition_Should_ReturnAllFailures_When_NoSuccesses()
    {
        // Arrange
        var error1 = Error.Validation("E1", "Error 1");
        var error2 = Error.Validation("E2", "Error 2");
        var results = new[]
        {
            Result<int>.Failure(error1),
            Result<int>.Failure(error2)
        };

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        successes.Should().BeEmpty();
        failures.Should().BeEquivalentTo(new[] { error1, error2 });
    }

    [Fact]
    public void Partition_Should_ReturnEmptyLists_When_CollectionIsEmpty()
    {
        // Arrange
        var results = Array.Empty<Result<int>>();

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        successes.Should().BeEmpty();
        failures.Should().BeEmpty();
    }

    [Fact]
    public void Partition_Should_ProcessAllItems_WithoutShortCircuiting()
    {
        // Arrange
        var executionCount = 0;
        var results = new[]
        {
            CreateResultWithSideEffect(1, true),
            CreateResultWithSideEffect(2, false),
            CreateResultWithSideEffect(3, true),
            CreateResultWithSideEffect(4, false),
            CreateResultWithSideEffect(5, true)
        };

        // Act
        var (successes, failures) = results.Partition();

        // Assert - all items should be processed
        executionCount.Should().Be(5);
        successes.Should().BeEquivalentTo(new[] { 1, 3, 5 });
        failures.Should().HaveCount(2);

        Result<int> CreateResultWithSideEffect(int value, bool shouldSucceed)
        {
            executionCount++;
            return shouldSucceed
                ? Result<int>.Success(value)
                : Error.Validation($"E{value}", $"Error {value}");
        }
    }

    [Fact]
    public void Partition_Should_WorkWithDifferentTypes()
    {
        // Arrange
        var results = new[]
        {
            Result<User>.Success(new User { Id = 1, Name = "Alice" }),
            Result<User>.Failure(Error.NotFound("NOT_FOUND", "User not found")),
            Result<User>.Success(new User { Id = 2, Name = "Bob" })
        };

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        successes.Should().HaveCount(2);
        successes.Select(u => u.Name).Should().BeEquivalentTo(new[] { "Alice", "Bob" });
        failures.Should().HaveCount(1);
        failures.First().Code.Should().Be("NOT_FOUND");
    }

    [Fact]
    public void Partition_Should_ReturnReadOnlyLists()
    {
        // Arrange
        var results = new[]
        {
            Result<int>.Success(1),
            Result<int>.Failure(Error.Validation("E1", "Error 1"))
        };

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        successes.Should().BeAssignableTo<IReadOnlyList<int>>();
        failures.Should().BeAssignableTo<IReadOnlyList<Error>>();
    }

    [Fact]
    public void Partition_Should_WorkInBulkImportScenario()
    {
        // Arrange - simulate CSV import
        var csvRows = new[]
        {
            "1,Alice,25",
            "2,Bob,invalid", // Invalid age
            "3,Charlie,30",
            "invalid,Dave,28", // Invalid ID
            "5,Eve,35"
        };

        var importResults = csvRows.Select(ParseAndValidateUser);

        // Act
        var (imported, failed) = importResults.Partition();

        // Assert
        imported.Should().HaveCount(3);
        imported.Select(u => u.Name).Should().BeEquivalentTo(new[] { "Alice", "Charlie", "Eve" });
        failed.Should().HaveCount(2);
        failed.Should().AllSatisfy(e => e.Type.Should().Be(ErrorType.Validation));
    }

    [Fact]
    public void Partition_Should_WorkWithLINQ()
    {
        // Arrange
        var userIds = new[] { 1, 2, 999, 4, 888, 6 }; // 999 and 888 don't exist

        // Act
        var (foundUsers, notFoundErrors) = userIds
            .Select(id => GetUser(id))
            .Partition();

        // Assert
        foundUsers.Should().HaveCount(4);
        foundUsers.Select(u => u.Id).Should().BeEquivalentTo(new[] { 1, 2, 4, 6 });
        notFoundErrors.Should().HaveCount(2);
        notFoundErrors.Should().AllSatisfy(e => e.Code.Should().Be("NOT_FOUND"));
    }

    [Fact]
    public void Partition_Should_PreserveOrder()
    {
        // Arrange
        var results = new[]
        {
            Result<int>.Success(1),
            Result<int>.Success(2),
            Result<int>.Failure(Error.Validation("E1", "Error 1")),
            Result<int>.Success(3),
            Result<int>.Failure(Error.Validation("E2", "Error 2")),
            Result<int>.Success(4)
        };

        // Act
        var (successes, failures) = results.Partition();

        // Assert - order should be preserved
        successes.Should().ContainInOrder(1, 2, 3, 4);
        failures.Select(e => e.Code).Should().ContainInOrder("E1", "E2");
    }

    [Fact]
    public void Partition_Should_WorkInBatchProcessingScenario()
    {
        // Arrange - simulate batch processing with mixed results
        var userIds = Enumerable.Range(1, 100).ToArray();
        
        // Simulate: every 10th user fails
        var processResults = userIds.Select(id => 
            id % 10 == 0 
                ? Result<int>.Failure(Error.Validation("PROCESSING_ERROR", $"Failed to process user {id}"))
                : Result<int>.Success(id * 2));

        // Act
        var (successes, failures) = processResults.Partition();

        // Assert
        successes.Should().HaveCount(90); // 100 - 10 failures
        failures.Should().HaveCount(10);
    }

    [Fact]
    public void Partition_Should_AllowSeparateProcessingOfSuccessesAndFailures()
    {
        // Arrange
        var results = new[]
        {
            Result<int>.Success(1),
            Result<int>.Failure(Error.Validation("E1", "Error 1")),
            Result<int>.Success(2),
            Result<int>.Failure(Error.Validation("E2", "Error 2"))
        };

        // Act
        var (successes, failures) = results.Partition();
        
        var processedSuccesses = successes.Select(x => x * 2).ToList();
        var errorMessages = failures.Select(e => e.Message).ToList();

        // Assert
        processedSuccesses.Should().BeEquivalentTo(new[] { 2, 4 });
        errorMessages.Should().BeEquivalentTo(new[] { "Error 1", "Error 2" });
    }

    [Fact]
    public void Partition_Should_HandleLargeCollections()
    {
        // Arrange - large collection
        var results = Enumerable.Range(1, 10000)
            .Select(i => i % 3 == 0 
                ? Result<int>.Failure(Error.Validation("E", "Error"))
                : Result<int>.Success(i));

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        successes.Should().HaveCount(6667); // ~2/3 succeed
        failures.Should().HaveCount(3333);  // ~1/3 fail
    }

    // Helper methods for testing
    private static Result<User> ParseAndValidateUser(string csvRow)
    {
        var parts = csvRow.Split(',');
        if (parts.Length != 3)
            return Error.Validation("INVALID_FORMAT", "Invalid CSV format");

        if (!int.TryParse(parts[0], out var id))
            return Error.Validation("INVALID_ID", "Invalid user ID");

        var name = parts[1];

        if (!int.TryParse(parts[2], out var age))
            return Error.Validation("INVALID_AGE", "Invalid age");

        return new User { Id = id, Name = name, Age = age };
    }

    private static Result<User> GetUser(int id)
    {
        if (id == 999 || id == 888)
            return Error.NotFound("NOT_FOUND", "User not found");

        return new User { Id = id, Name = $"User{id}" };
    }

    [Fact]
    public async Task TaskPartition_Should_AwaitAndPartition_When_TaskWrapsResults()
    {
        // Arrange
        var resultsTask = Task.FromResult<IEnumerable<Result<int>>>(new[]
        {
            Result<int>.Success(1),
            Result<int>.Failure(Error.Validation("E1", "Error 1")),
            Result<int>.Success(2),
            Result<int>.Failure(Error.Validation("E2", "Error 2")),
            Result<int>.Success(3)
        });

        // Act
        var (successes, failures) = await resultsTask.Partition();

        // Assert
        successes.Should().BeEquivalentTo(new[] { 1, 2, 3 });
        failures.Should().HaveCount(2);
        failures.Select(e => e.Code).Should().BeEquivalentTo(new[] { "E1", "E2" });
    }

    [Fact]
    public async Task TaskPartition_Should_EnableFluentChaining()
    {
        // Arrange - simulating a pipeline
        Task<IEnumerable<Result<User>>> GetResultsAsync() =>
            Task.FromResult<IEnumerable<Result<User>>>(new[]
            {
                Result<User>.Success(new User { Id = 1, Name = "Alice" }),
                Result<User>.Failure(Error.NotFound("NOT_FOUND", "User not found")),
                Result<User>.Success(new User { Id = 2, Name = "Bob" })
            });

        // Act - fluent chaining without intermediate await
        var (successes, failures) = await GetResultsAsync().Partition();

        // Assert
        successes.Should().HaveCount(2);
        successes.Select(u => u.Name).Should().BeEquivalentTo(new[] { "Alice", "Bob" });
        failures.Should().HaveCount(1);
        failures.First().Code.Should().Be("NOT_FOUND");
    }

    private record User
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int Age { get; init; }
    }
}
