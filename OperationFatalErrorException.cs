using System;

namespace Hillinworks.OperationFramework
{
	public class OperationFatalErrorException : Exception
	{
		public OperationFatalErrorException(IOperationStatus status, string message, Exception innerException)
			: base(message, innerException)
		{
			this.Status = status;
		}

		public IOperationStatus Status { get; }
	}
}