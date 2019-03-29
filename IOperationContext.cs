using System;
using System.Threading;

namespace Hillinworks.OperationFramework
{
	public interface IOperationContext : IDisposable, IProfilingContext, IProgress<double>
    {
		CancellationToken CancellationToken { get; }
		string Name { get; }
		string FullName { get; }
		DateTime StartTime { get; }
		TimeSpan Duration { get; }

		void Log(LogLevel level, string message);

		/// <summary>
		///     Throw an OperationFatalErrorException to signal termination of the operation
		/// </summary>
		/// <param name="message">The message describing the reason of termination</param>
		/// <param name="innerException">The inner exception causing the termination, if any</param>
		void OnFatalError(string message, Exception innerException = null);

		/// <summary>
		///     Signal completion of the operation
		/// </summary>
		void Commit();

		/// <summary>
		///     Cancel current operation
		/// </summary>
		void Cancel();

		/// <summary>
		///     Report the progress of current operation
		/// </summary>
		/// <param name="progress">A value from 0 to 1</param>
		void ReportProgress(double progress);

		/// <summary>
		///     Start a child operation
		/// </summary>
		/// <param name="name">The name of the child operation</param>
		/// <param name="progressShare">The factor with which the child operation's progress will affect current operation's</param>
		/// <param name="shareCancellation">Whether cancelling the child operation also cancels the current operation</param>
		/// <returns>A child status context for the child operation</returns>
		IOperationContext StartChildOperation(string name, double progressShare = 0, bool shareCancellation = true);

        event EventHandler ProgressChanged;
        double Progress { get; }
    }
}