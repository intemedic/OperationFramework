using System;

namespace Hillinworks.OperationFramework
{
    public abstract class OperationEventHandler : IOperationEventHandler
    {
        protected virtual void OnCancelled()
        {
        }

        protected virtual void OnCompleted()
        {
        }

        protected virtual void OnFailed(string message, Exception exception)
        {
        }

        protected virtual void OnProgressChanged(double progress)
        {
        }

        protected virtual void OnMessageReceived(LogLevel level, string message)
        {
        }

        void IOperationEventHandler.OnCancelled()
        {
            this.OnCancelled();
        }

        void IOperationEventHandler.OnCompleted()
        {
            this.OnCompleted();
        }

        void IOperationEventHandler.OnFailed(string message, Exception exception)
        {
            this.OnFailed(message, exception);
        }

        void IOperationEventHandler.OnProgressChanged(double progress)
        {
            this.OnProgressChanged(progress);
        }

        void IOperationEventHandler.OnMessageReceived(LogLevel level, string message)
        {
            this.OnMessageReceived(level, message);
        }
    }
}