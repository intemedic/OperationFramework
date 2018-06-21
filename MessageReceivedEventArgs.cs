using System;

namespace Hillinworks.OperationFramework
{
	public sealed class MessageReceivedEventArgs : EventArgs
	{
		public MessageReceivedEventArgs(LogLevel logLevel, string message)
		{
			this.LogLevel = logLevel;
			this.Message = message;
		}

		public LogLevel LogLevel { get; }
		public string Message { get; }
	}
}