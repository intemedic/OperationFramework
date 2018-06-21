namespace Hillinworks.OperationFramework
{
	public interface IOperation
	{
		void Execute(IStatusContext context);
	}

	public interface IOperation<out TResult>
	{
		TResult Execute(IStatusContext context);
	}
}