using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hillinworks.OperationFramework
{
    public interface IAsyncOperation
    {
        Task ExecuteAsync(IOperationContext statusContext);
    }

    public interface IAsyncOperation<TResult>
    {
        Task<TResult> ExecuteAsync(IOperationContext statusContext);
    }
}
