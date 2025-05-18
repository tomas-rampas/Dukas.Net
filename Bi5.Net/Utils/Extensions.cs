using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Bi5.Net.Utils
{

public static class Extensions
{
    /// <summary>
    /// Credit to Alexandru Puiu
    /// https://gist.github.com/scattered-code/b834bbc355a9ee710e3147321d6f985a#file-asyncparallelforeach-cs 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="body"></param>
    /// <param name="maxDegreeOfParallelism"></param>
    /// <param name="scheduler"></param>
    /// <typeparam name="T"></typeparam>
    public static async Task AsyncParallelForEach<T>(this IAsyncEnumerable<T> source, Func<T, Task> body,
        int maxDegreeOfParallelism = 4, TaskScheduler scheduler = default(TaskScheduler))
    {
        var options = new ExecutionDataflowBlockOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };
        if (scheduler != null)
        {
            options.TaskScheduler = scheduler;
        }

        var block = new ActionBlock<T>(body, options);

        await foreach (var item in source)
        {
            block.Post(item);
        }

        block.Complete();

        await block.Completion;
    }
}}
