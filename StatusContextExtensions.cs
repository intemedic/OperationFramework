using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Hillinworks.OperationFramework
{
    public static class StatusContextExtensions
    {
        public static void LogDebug(this IOperationContext context, string message)
        {
            context.Log(LogLevel.Debug, message);
        }

        public static void LogDebug(
            this IOperationContext context,
            string actionDescription,
            string firstParameter,
            object firstValue,
            params object[] restParameters)
        {
            if (restParameters.Length % 2 != 0)
            {
                throw new ArgumentException("parameters must be in pairs", nameof(restParameters));
            }

            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine(actionDescription);
            messageBuilder.AppendLine($"\t{firstParameter}={firstValue}");

            for (var i = 0; i < restParameters.Length; i += 2)
            {
                messageBuilder.AppendLine($"\t{restParameters[i]}={restParameters[i + 1]}");
            }

            context.LogDebug(messageBuilder.ToString());
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

        [DebuggerStepThrough]
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

        [DebuggerStepThrough]
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

        [DebuggerStepThrough]
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

        [DebuggerStepThrough]
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

        [DebuggerStepThrough]
        public static void StartChildOperation(
            this IOperationContext context,
            string name,
            IOperation operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            context.StartChildOperation(name, operation.Execute, progressShare, shareCancellation);
        }

        [DebuggerStepThrough]
        public static TResult StartChildOperation<TResult>(
            this IOperationContext context,
            string name,
            IOperation<TResult> operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            return context.StartChildOperation(name, operation.Execute, progressShare, shareCancellation);
        }

        [DebuggerStepThrough]
        public static void StartChildOperation<TOperation>(
            this IOperationContext context,
            string name,
            double progressShare = 0,
            bool shareCancellation = true)
            where TOperation : IOperation, new()
        {
            context.StartChildOperation(name, new TOperation(), progressShare, shareCancellation);
        }

        [DebuggerStepThrough]
        public static TResult StartChildOperation<TOperation, TResult>(
            this IOperationContext context,
            string name,
            double progressShare = 0,
            bool shareCancellation = true)
            where TOperation : IOperation<TResult>, new()
        {
            return context.StartChildOperation(name, new TOperation(), progressShare, shareCancellation);
        }

        [DebuggerStepThrough]
        public static Task StartChildOperationAsync(
            this IOperationContext context,
            string name,
            IAsyncOperation operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            return context.StartChildOperationAsync(name, operation.ExecuteAsync, progressShare, shareCancellation);
        }

        [DebuggerStepThrough]
        public static Task<TResult> StartChildOperationAsync<TResult>(
            this IOperationContext context,
            string name,
            IAsyncOperation<TResult> operation,
            double progressShare = 0,
            bool shareCancellation = true)
        {
            return context.StartChildOperationAsync(name, operation.ExecuteAsync, progressShare, shareCancellation);
        }

        [DebuggerStepThrough]
        public static void StartChildOperationAsync<TOperation>(
            this IOperationContext context,
            string name,
            double progressShare = 0,
            bool shareCancellation = true)
            where TOperation : IAsyncOperation, new()
        {
            context.StartChildOperationAsync(name, new TOperation(), progressShare, shareCancellation);
        }

        [DebuggerStepThrough]
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