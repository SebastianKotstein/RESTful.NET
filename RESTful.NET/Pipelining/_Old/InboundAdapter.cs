using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Pipelining
{
    /// <summary>
    /// Represents a starting point of the pipes-and-filters architecture consisting of a repetive execution unit implemented in <see cref="Execute"/> and an output pipe.
    /// </summary>
    /// <typeparam name="O">type of output result</typeparam>
    public abstract class InboundAdapter<O> : Processor
    {
        protected BlockingCollection<O> _outputPipe;

        /// <summary>
        /// Gets and sets the output pipe
        /// </summary>
        public BlockingCollection<O> OutputPipe
        {
            get
            {
                return _outputPipe;
            }
            set
            {
                _outputPipe = value;
            }
        }

        /// <summary>
        /// Forwards a task to the output pipe
        /// </summary>
        /// <param name="task">task to be forwared</param>
        protected void Forward( O task)
        {
            _outputPipe.TryAdd(task);
        }



    }
}
