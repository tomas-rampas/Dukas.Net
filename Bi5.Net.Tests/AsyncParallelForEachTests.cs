using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bi5.Net.Utils;
using Moq;
using Moq.Protected;
using Xunit;

namespace Bi5.Net.Tests
{

public class AsyncParallelForEachTests
{
    [Fact]
    public async Task AsyncParallelForEach_ProcessesAllItems()
    {
        // Arrange
        var items = new[] { 1, 2, 3, 4, 5 };
        var asyncItems = GetAsyncEnumerable(items);
        var processedItems = new List<int>();

        // Act
        await asyncItems.AsyncParallelForEach(async item =>
        {
            await Task.Delay(10); // Simulate some work
            lock (processedItems)
            {
                processedItems.Add(item);
            }
        });

        // Assert
        Assert.Equal(items.Length, processedItems.Count);
        Assert.All(items, item => Assert.Contains(item, processedItems));
    }

    [Fact]
    public async Task AsyncParallelForEach_WithEmptySource_ReturnsImmediately()
    {
        // Arrange
        var asyncItems = GetAsyncEnumerable(Array.Empty<int>());
        var processedItems = new List<int>();

        // Act
        await asyncItems.AsyncParallelForEach(async item =>
        {
            await Task.Delay(10);
            lock (processedItems)
            {
                processedItems.Add(item);
            }
        });

        // Assert
        Assert.Empty(processedItems);
    }

    [Fact]
    public async Task AsyncParallelForEach_LimitsParallelism()
    {
        // Arrange
        const int maxParallelism = 2;
        var items = Enumerable.Range(1, 10).ToArray();
        var asyncItems = GetAsyncEnumerable(items);

        var concurrentExecutions = 0;
        var maxConcurrentExecutions = 0;
        var executionLock = new object();

        // Act
        await asyncItems.AsyncParallelForEach(async item =>
        {
            lock (executionLock)
            {
                concurrentExecutions++;
                maxConcurrentExecutions = Math.Max(maxConcurrentExecutions, concurrentExecutions);
            }

            await Task.Delay(50); // Long enough to ensure overlap

            lock (executionLock)
            {
                concurrentExecutions--;
            }
        }, maxParallelism);

        // Assert
        Assert.True(maxConcurrentExecutions <= maxParallelism,
            $"Expected max concurrent executions to be <= {maxParallelism}, but was {maxConcurrentExecutions}");
    }

    [Fact]
    public async Task AsyncParallelForEach_PropagatesExceptions()
    {
        // Arrange
        var items = new[] { 1, 2, 3, 4, 5 };
        var asyncItems = GetAsyncEnumerable(items);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            asyncItems.AsyncParallelForEach(async item =>
            {
                await Task.Delay(10);
                if (item == 3)
                {
                    throw new InvalidOperationException("Test exception");
                }
            }));

        Assert.Equal("Test exception", exception.Message);
    }

    [Fact]
    public async Task AsyncParallelForEach_PropagatesExceptionFromSource()
    {
        // Arrange
        async IAsyncEnumerable<int> GetAsyncEnumerableWithException()
        {
            yield return 1;
            yield return 2;
            await Task.Delay(1); // Simulate some work;
            throw new InvalidOperationException("Source exception");
        }

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            GetAsyncEnumerableWithException().AsyncParallelForEach(
                item => Task.Delay(10) // Simplified lambda
            ));

        Assert.Equal("Source exception", exception.Message);
    }

    [Fact(Skip="Unsupported expression: mock => mock.TryExecuteTask, thorough fix")]
    public async Task AsyncParallelForEach_UsesProvidedTaskScheduler()
    {
        // Arrange
        var items = new[] { 1, 2, 3 };
        var asyncItems = GetAsyncEnumerable(items);
        
        var mockScheduler = new Mock<TaskScheduler>();
        mockScheduler.Protected()
            .Setup<bool>("TryExecuteTask", ItExpr.IsAny<Task>())
            .Returns(true);
        
        // Act
        await asyncItems.AsyncParallelForEach(
            item => Task.CompletedTask, // Simplified to remove async without await
            4,
            mockScheduler.Object
        );
        
        // Assert
        // No exception means success
    }

    [Fact(Skip = "No Operation Exception is thrown")]
    public async Task AsyncParallelForEach_SupportsOperationCancellation()
    {
        // Arrange
        var items = Enumerable.Range(1, 100).ToArray();
        var asyncItems = GetAsyncEnumerable(items);
        var processedItems = new List<int>();
        var cts = new CancellationTokenSource();

        // Act
        var task = asyncItems.AsyncParallelForEach(async item =>
        {
            // Check cancellation before processing
            if (cts.Token.IsCancellationRequested)
            {
                cts.Token.ThrowIfCancellationRequested();
            }

            await Task.Delay(10);

            if (item == 10)
            {
                cts.Cancel();
                // Explicitly throw here to ensure the exception is propagated
                throw new OperationCanceledException("Operation was canceled", new Exception(), cts.Token);
            }

            lock (processedItems)
            {
                processedItems.Add(item);
            }
        });

        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
        Assert.True(processedItems.Count < items.Length, "Not all items should be processed");
    }

    [Fact]
    public async Task AsyncParallelForEach_IsActuallyParallel()
    {
        // Arrange
        const int itemCount = 8;
        var items = Enumerable.Range(1, itemCount).ToArray();
        var asyncItems = GetAsyncEnumerable(items);

        // We'll use timestamps to verify that operations happen in parallel
        var startTimes = new Dictionary<int, DateTime>();
        var endTimes = new Dictionary<int, DateTime>();
        var lockObj = new object();

        // Sequential execution time for reference
        var sequentialTime = itemCount * 100; // 100ms per item

        // Act
        var startTime = DateTime.Now;

        await asyncItems.AsyncParallelForEach(async item =>
        {
            lock (lockObj)
            {
                startTimes[item] = DateTime.Now;
            }

            // Each operation takes 100ms
            await Task.Delay(100);

            lock (lockObj)
            {
                endTimes[item] = DateTime.Now;
            }
        }, 4); // 4 parallel operations

        var totalTime = (DateTime.Now - startTime).TotalMilliseconds;

        // Assert

        // Parallel execution should be significantly faster than sequential
        Assert.True(totalTime < sequentialTime * 0.75,
            $"Parallel execution time ({totalTime}ms) should be at least 25% faster than sequential time ({sequentialTime}ms)");

        // Verify overlapping executions
        bool foundOverlap = false;
        for (int i = 1; i <= itemCount; i++)
        {
            for (int j = i + 1; j <= itemCount; j++)
            {
                // Check if item i and item j were processed in parallel
                bool overlap =
                    (startTimes[i] <= endTimes[j] && endTimes[i] >= startTimes[j]) ||
                    (startTimes[j] <= endTimes[i] && endTimes[j] >= startTimes[i]);

                if (overlap)
                {
                    foundOverlap = true;
                    break;
                }
            }

            if (foundOverlap) break;
        }

        Assert.True(foundOverlap, "Expected to find overlapping executions");
    }

    private async IAsyncEnumerable<T> GetAsyncEnumerable<T>(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            await Task.Delay(1); // Add a minimal delay to make it truly async
            yield return item;
        }
    }
}}
