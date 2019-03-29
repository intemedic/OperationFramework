using System.Threading.Tasks;

namespace Hillinworks.OperationFramework
{
	public interface IOperation
	{
		Task ExecuteAsync(IOperationContext statusContext);
	}

	public interface IOperation<TResult>
	{
		Task<TResult> ExecuteAsync(IOperationContext context);
	}
}