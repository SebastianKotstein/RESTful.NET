using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Pipelining
{
    /// <summary>
    /// Represents an abstract filter component (see pipes-and-filter architectural pattern) which executes specific tasks iteratively. The execution of those tasks must be implemented in <see cref="Execute"/>.
    /// The processor can be started and stopped by calling <see cref="Start"/> and <see cref="Stop"/>. Furthermore, initial and final steps before starting and after stopping this processor can be defined
    /// in <see cref="Init"/> and <see cref="Final"/>. Note that each processor has its own thread for execution.
    /// </summary>
    public abstract class Processor : IDisposable
    {

        private bool _active = false;
        private int _timeout = 200;

        /// <summary>
        /// Gets a bool value indicating whether this processor is running or not.
        /// Use <see cref="Start"/> and <see cref="Stop"/> to start/stop this processor.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return _active;
            }
        }

        /// <summary>
        /// Gets or sets a timeout value in milliseconds specifiying how long the processor is waiting for an incoming task by listening on input pipe.
        /// After this period has elasped the processor checks the active state (see <see cref="IsActive"/>) and decides whether to continue (waiting for an incoming task again) or not.
        /// </summary>
        public int Timeout
        {
            set
            {
                _timeout = value;
            }
            get
            {
                return _timeout;
            }
        }

        /// <summary>
        /// Performs initial steps for preparing the exection of incoming tasks. This method is automatically invoked by <see cref="Start"/>.
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// Performs final steps after the execution has been stopped by calling <see cref="Stop"/>.
        /// </summary>
        protected abstract void Final();

        /// <summary>
        /// Initializes the processor (see <see cref="Init"/>) and starts the repeating executions of tasks.
        /// </summary>
        public void Start()
        {
            Init();
            _active = true;
            ThreadPool.QueueUserWorkItem((o) =>
            {
                while (IsActive)
                {
                    this.Execute();
                }
            });
        }

        /// <summary>
        /// Stops the execution of further tasks and perfoms final steps specified in <see cref="Final"/>.
        /// </summary>
        public void Stop()
        {
            _active = false;
            Final();

        }

        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// Waits for an incoming task and executes it. The duration of waiting is limited by <see cref="Timeout"/>.
        /// </summary>
        protected abstract void Execute();
    }    
}

