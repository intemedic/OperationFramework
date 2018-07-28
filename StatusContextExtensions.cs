using System;
using System.Threading.Tasks;

namespace Hillinworks.OperationFramework
{
    public static class StatusContextExtensions
    {
        public static void LogDebug(this IOperationContext context, string message)
        {
            context.Log(LogLevel.Debug, message);
        }

        public static void LogInfo(this IOperationContext context, string message)
        {
            context.Log(LogLevel.Info, message);
        }

        public static void LogWarning(this IOperationContext context, string message)
        {
            context.Log(LogLevel.Warning, message);
        }

        public static void LogError(this IOperationContext context, string message)
        {
            context.Log(LogLevel.Error, message);
        }

        public static void LogFatal(this IOperationContext context, string message)
        {
            context.Log(LogLevel.Fatal, message);
        }

        public static IOperationStatus AsOperationStatus(this IOperationContext context)
        {
            return context as IOperationStatus;
        }

        public static void StartChildOperation(
            this IOperationContext context,
            string name,
            Action<IOperationContext> operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            using (var childContext = context.StartChildOperation(name, progressShare, shareCancellation))
            {
                operation(childContext);
            }
        }

        public static TResult StartChildOperation<TResult>(
            this IOperationContext context,
            string name,
            Func<IOperationContext, TResult> operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            using (var childContext = context.StartChildOperation(name, progressShare, shareCancellation))
            {
                return operation(childContext);
            }
        }

        public static async Task StartChildOperationAsync(
            this IOperationContext context,
            string name,
            Func<IOperationContext, Task> operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            using (var childContext = context.StartChildOperation(name, progressShare, shareCancellation))
            {
                await operation(childContext);
            }
        }

        public static async Task<TResult> StartChildOperationAsync<TResult>(
            this IOperationContext context,
            string name,
            Func<IOperationContext, Task<TResult>> operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            using (var childContext = context.StartChildOperation(name, progressShare, shareCancellation))
            {
                return await operation(childContext);
            }
        }

        public static void StartChildOperation(
            this IOperationContext context,
            string name,
            IOperation operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            context.StartChildOperation(name, operation.Execute, progressShare, shareCancellation);
        }

        public static TResult StartChildOperation<TResult>(
            this IOperationContext context,
            string name,
            IOperation<TResult> operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            return context.StartChildOperation(name, operation.Execute, progressShare, shareCancellation);
        }

        public static void StartChildOperation<TOperation>(
            this IOperationContext context,
            string name,
            double progressShare = 0,
            bool shareCancellation = true)
            where TOperation : IOperation, new()
        {
            context.StartChildOperation(name, new TOperation(), progressShare, shareCancellation);
        }

        public static TResult StartChildOperation<TOperation, TResult>(
            this IOperationContext context,
            string name,
            double progressShare = 0,
            bool shareCancellation = true)
            where TOperation : IOperation<TResult>, new()
        {
            return context.StartChildOperation(name, new TOperation(), progressShare, shareCancellation);
        }

        public static Task StartChildOperationAsync(
            this IOperationContext context,
            string name,
            IAsyncOperation operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            return context.StartChildOperationAsync(name, operation.ExecuteAsync, progressShare, shareCancellation);
        }

        public static Task<TResult> StartChildOperationAsync<TResult>(
            this IOperationContext context,
            string name,
            IAsyncOperation<TResult> operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            return context.StartChildOperationAsync(name, operation.ExecuteAsync, progressShare, shareCancellation);
        }

        public static void StartChildOperationAsync<TOperation>(
            this IOperationContext context,
            string name,
            double progressShare = 0,
            bool shareCancellation = true)
            where TOperation : IAsyncOperation, new()
        {
            context.StartChildOperationAsync(name, new TOperation(), progressShare, shareCancellation);
        }

        public static Task<TResult> StartChildOperationAsync<TOperation, TResult>(
            this IOperationContext context,
            string name,
            double progressShare = 0,
            bool shareCancellation = true)
            where TOperation : IAsyncOperation<TResult>, new()
        {
            return context.StartChildOperationAsync(name, new TOperation(), progressShare, shareCancellation);
        }
    }
}