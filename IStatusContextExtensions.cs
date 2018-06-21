using System;

namespace Hillinworks.OperationFramework
{
	public static class IStatusContextExtensions
	{
		public static void LogDebug(this IStatusContext context, string message)
		{
			context.Log(LogLevel.Debug, message);
		}

		public static void LogInfo(this IStatusContext context, string message)
		{
			context.Log(LogLevel.Info, message);
		}

		public static void LogWarning(this IStatusContext context, string message)
		{
			context.Log(LogLevel.Warning, message);
		}

		public static void LogError(this IStatusContext context, string message)
		{
			context.Log(LogLevel.Error, message);
		}

		public static void LogFatal(this IStatusContext context, string message)
		{
			context.Log(LogLevel.Fatal, message);
		}

		public static IOperationStatus AsOperationStatus(this IStatusContext context)
		{
			return context as IOperationStatus;
		}

		public static void StartChildOperation(
			this IStatusContext context,
			string name,
			Action<IStatusContext> operation,
			double progressShare = 0,
			bool shareCancellation = true)
		{
			using (var childContest = context.StartChildOperation(name, progressShare, shareCancellation))
			{
				operation(childContest);
			}
		}

		public static TResult StartChildOperation<TResult>(
			this IStatusContext context,
			string name,
			Func<IStatusContext, TResult> operation,
			double progressShare = 0,
			bool shareCancellation = true)
		{
			using (var childContest = context.StartChildOperation(name, progressShare, shareCancellation))
			{
				return operation(childContest);
			}
		}

		public static void StartChildOperation(
			this IStatusContext context,
			string name,
			IOperation operation,
			double progressShare = 0,
			bool shareCancellation = true)
		{
			context.StartChildOperation(name, operation.Execute, progressShare, shareCancellation);
		}

		public static TResult StartChildOperation<TResult>(
			this IStatusContext context,
			string name,
			IOperation<TResult> operation,
			double progressShare = 0,
			bool shareCancellation = true)
		{
			return context.StartChildOperation(name, operation.Execute, progressShare, shareCancellation);
		}

		public static void StartChildOperation<TOperation>(
			this IStatusContext context,
			string name,
			double progressShare = 0,
			bool shareCancellation = true)
			where TOperation : IOperation, new()
		{
			context.StartChildOperation(name, new TOperation(), progressShare, shareCancellation);
		}

		public static TResult StartChildOperation<TOperation, TResult>(
			this IStatusContext context,
			string name,
			double progressShare = 0,
			bool shareCancellation = true)
			where TOperation : IOperation<TResult>, new()
		{
			return context.StartChildOperation(name, new TOperation(), progressShare, shareCancellation);
		}
	}
}