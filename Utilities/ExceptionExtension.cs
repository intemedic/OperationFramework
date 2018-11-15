using System.Text;

// ReSharper disable once CheckNamespace
namespace System
{
    internal static class ExceptionExtension
    {
        public static string FormatMessage(this Exception exception)
        {
            var builder = new StringBuilder();
            FormatMessageRecursive(builder, exception, 0);
            return builder.ToString();
        }

        private static void FormatMessageRecursive(StringBuilder builder, Exception exception, int indentLevel)
        {
            var indent = new string(' ', indentLevel * 2);
            builder.Append(indent).Append($"[{exception.GetType().Name}] {exception.Message}");

            if (exception is AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    FormatMessageRecursive(builder, innerException, indentLevel + 1);
                }
            }
            else if (exception.InnerException != null)
            {
                FormatMessageRecursive(builder, exception.InnerException, indentLevel + 1);
            }
        }
    }
}