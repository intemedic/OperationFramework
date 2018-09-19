using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hillinworks.OperationFramework
{
    public interface IOperationEventHandler
    {
        void OnCancelled();
        void OnCompleted();
        void OnFailed(string message, Exception exception);
        void OnProgressChanged(double progress);
        void OnMessageReceived(LogLevel level, string message);
    }
}
