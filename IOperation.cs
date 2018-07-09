namespace Hillinworks.OperationFramework
{
	public interface IOperation
	{
		void Execute(IStatusContext statusContext);
	}

	public interface IOperation<out TResult>
	{
		TResult Execute(IStatusContext context);
	}
}