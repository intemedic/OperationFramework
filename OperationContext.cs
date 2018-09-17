using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Hillinworks.OperationFramework
{
    [DebuggerDisplay("Operation <{" + nameof(FullName) + "}>")]
    public class OperationContext : IOperationContext, IOperationStatus
    {
        public OperationContext(string name)
        {
            this.Name = name;

            this.CancellationTokenSource = new CancellationTokenSource();
            this.CancellationToken.Register(this.OnCancelled);

            this.Start();
        }

        private OperationContext(string name, OperationContext parent, double progressShare, bool shareCancellation)
        {
            this.Name = name;
            this.Parent = parent;
            this.Parent.ChildrenList.Add(this);

            if (progressShare < 0 || progressShare > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(progressShare));
            }

            this.ProgressShare = progressShare;

            this.CancellationTokenSource = shareCancellation
                ? this.Parent.CancellationTokenSource
                : new CancellationTokenSource();

            this.CancellationToken.Register(this.OnCancelled);
            this.Start();
        }

        private TimeSpan? DeterminedElapsedTime { get; set; }
        private OperationContext Parent { get; }

        private List<OperationContext> ChildrenList { get; }
            = new List<OperationContext>();

        public IReadOnlyList<OperationContext> Children => this.ChildrenList;

        private double ProgressShare { get; }

        private CancellationTokenSource CancellationTokenSource { get; }


        internal List<ProfileEvent> InternalEvents { get; } = new List<ProfileEvent>();

        public IEnumerable<ProfileEvent> ExclusiveEvents
            => this.InternalEvents;

        public DateTime StartTime { get; private set; }
        public TimeSpan Duration => this.DeterminedElapsedTime ?? DateTime.Now - this.StartTime;


        public CancellationToken CancellationToken => this.CancellationTokenSource.Token;

        public string Name { get; }

        public string FullName => this.Parent == null ? this.Name : $"{this.Parent.FullName}::{this.Name}";

        public string FoldedFullName
        {
            get
            {
                if (this.Parent == null)
                {
                    return this.Name;
                }

                var root = this.Parent;

                while (root.Parent != null)
                {
                    root = root.Parent;
                }

                if (root == this.Parent)
                {
                    return this.FullName;
                }

                return $"{root.Name}...{this.Name}";
            }
        }

        public IOperationContext StartChildOperation(string name, double progressShare, bool shareCancellation)
        {
            this.CheckAvailability();

            Trace.WriteLine(
                $"<{this.Name}> Starting child operation <{name}> at {this.Progress * 100}%, Progress Share = {progressShare}, Share Cancellation = {shareCancellation}");
            return new OperationContext(name, this, progressShare, shareCancellation);
        }

        public ProfileEvent StartProfileEvent(params string[] tags)
        {
            var @event = new ProfileEvent(DateTime.Now, tags);
            lock (this.InternalEvents)
            {
                this.InternalEvents.Add(@event);
            }

            return @event;
        }

        public void Commit()
        {
            this.CheckAvailability();

            this.DeterminedElapsedTime = DateTime.Now - this.StartTime;
            Trace.WriteLine($"<{this.Name}> completed at {DateTime.Now}, {this.Duration} elapsed");

            this.ReportProgress(1.0);

            this.Completed?.Invoke(this, EventArgs.Empty);
            this.Parent?.OnChildComplete(this);
        }

        public void Cancel()
        {
            this.CheckAvailability();

            this.DeterminedElapsedTime = DateTime.Now - this.StartTime;
            Trace.WriteLine($"<{this.Name}> cancelled at {DateTime.Now}, {this.Duration} elapsed");

            if (this.Parent == null || this.CancellationTokenSource != this.Parent.CancellationTokenSource)
            {
                Trace.WriteLine($"<{this.Name}> Cancellation has been requested");
            }

            this.CancellationTokenSource.Cancel(true);
        }

        private void LogInternal(LogLevel level, string message)
        {
            this.Parent?.LogInternal(level, message);

#if !DEBUG
            if (level == LogLevel.Debug)
            {
                return;
            }
#endif
            this.MessageReceived?.Invoke(this, new MessageReceivedEventArgs(level, message));
        }


        public void Log(LogLevel level, string message)
        {
            Trace.WriteLine($"<{this.FullName}> {level}: {message}");
            this.LogInternal(level, message);
        }

        public void OnFatalError(string message, Exception innerException)
        {
            this.Log(LogLevel.Fatal, message);
            this.IsFailed = true;
            this.Failed?.Invoke(this, EventArgs.Empty);
            throw new OperationFatalErrorException(this, message, innerException);
        }

        public void ReportProgress(double progress)
        {
            this.CheckAvailability();

            //Debug.Assert(progress >= 0 && progress < 1.01);

            progress = Math.Min(Math.Max(progress, 0), 1);

            if (this.Parent != null)
            {
                var delta = progress - this.Progress;
                this.Parent.ReportProgress(this.Parent.Progress + delta * this.ProgressShare);
            }

            this.Progress = progress;
            this.ProgressChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public double Progress { get; private set; }
        public bool IsFailed { get; private set; }

        public event EventHandler ProgressChanged;
        public event EventHandler Failed;
        public event EventHandler Completed;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler Cancelled;

        public bool IsCancelled => this.CancellationTokenSource.IsCancellationRequested;

        public void Start()
        {
            this.CheckAvailability();

            if (this.Progress > 0)
            {
                throw new InvalidOperationException("this operation has already started");
            }

            this.StartTime = DateTime.Now;
            Trace.WriteLine($"<{this.Name}> started at {DateTime.Now}");
        }

        private void CheckAvailability()
        {
            if (this.IsFailed || this.IsCancelled)
            {
                throw new InvalidOperationException("this operation is already failed or cancelled");
            }
        }

        private void OnCancelled()
        {
            this.Cancelled?.Invoke(this, EventArgs.Empty);
        }

        private void OnChildComplete(OperationContext child)
        {
            Trace.WriteLine($"<{this.Name}> Child operation <{child.Name}> is complete");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Commit();
            }
        }
    }
}