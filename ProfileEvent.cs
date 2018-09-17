using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace Hillinworks.OperationFramework
{
    public class ProfileEvent : IDisposable
    {

        [DllImport("Kernel32.dll"), SuppressUnmanagedCodeSecurity]
        private static extern int GetCurrentProcessorNumber();
        public static string ProcessorCoreTag => $"core-{GetCurrentProcessorNumber()}";


        public DateTime StartTime { get; }
        public string[] Tags { get; }
        public DateTime? EndTime { get; internal set; }
        public TimeSpan Duration => (this.EndTime ?? DateTime.Now) - this.StartTime;

        public ProfileEvent(DateTime startTime, params string[] tags)
        {
            this.StartTime = startTime;
            this.Tags = tags;
        }

        public void Dispose()
        {
            if (this.EndTime != null)
            {
                return;
            }

            this.EndTime = DateTime.Now;
        }
    }
}