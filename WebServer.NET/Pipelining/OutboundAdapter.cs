using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Pipelining
{
    /// <summary>
    /// Represents a endpoint of the pipes-and-filters architecture consisting of an input pipe and a repetive execution unit implemented in <see cref="ExecuteTask"/>.
    /// </summary>
    /// <typeparam name="I">type of input task</typeparam>
    public abstract class OutboundAdapter<I> : Processor
    {
        protected BlockingCollection<I> _inputPipe = new BlockingCollection<I>();

        /// <summary>
        /// Gets and sets the input pipe
        /// </summary>
        public BlockingCollection<I> InputPipe
        {
            get
            {
                return _inputPipe;
            }
            set
            {
                _inputPipe = value;
            }
        }


        protected override void Execute()
        {
            I task;
            if (_inputPipe.TryTake(out task, Timeout))
            {
               this.Execute(task);
            }
        }

        /// <summary>
        /// Executes a single task.
        /// </summary>
        /// <param name="task">task from input pipe</param>
        protected abstract void Execute(I task);
    }
}
