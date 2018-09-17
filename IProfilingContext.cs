namespace Hillinworks.OperationFramework
{
    public interface IProfilingContext
    {
        ProfileEvent StartProfileEvent(params string[] tags);
    }
}