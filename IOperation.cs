namespace Hillinworks.OperationFramework
{
	public interface IOperation
	{
		void Execute(IOperationContext statusContext);
	}

	public interface IOperation<out TResult>
	{
		TResult Execute(IOperationContext context);
	}
}