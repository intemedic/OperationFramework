using System;

namespace Hillinworks.OperationFramework
{
	public interface IOperationStatus
	{
		string Name { get; }
		string FullName { get; }
		double Progress { get; }
		bool IsFailed { get; }
		bool IsCancelled { get; }

		event EventHandler ProgressChanged;
		event EventHandler Cancelled;
		event EventHandler Failed;
		event EventHandler Completed;
		event EventHandler<MessageReceivedEventArgs> MessageReceived;
	}
}