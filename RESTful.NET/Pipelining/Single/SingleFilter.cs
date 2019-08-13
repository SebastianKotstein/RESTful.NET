using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Pipelining.Single
{
    /// <summary>
    /// Represents a single thread <see cref="SingleProcessor"/> filter implementation which consists of an input pipe, a execution unit implemented in <see cref="Execute(I)"/> and an output pipe.
    /// 
    /// </summary>
    /// <typeparam name="I">type of input task</typeparam>
    /// <typeparam name="O">type of output result</typeparam>
    public abstract class SingleFilter<I, O> : SingleProcessor
    {
        protected BlockingCollection<I> _inputPipe = new BlockingCollection<I>();
        protected BlockingCollection<O> _outputPipe = new BlockingCollection<O>();

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


        protected override void Execute()
        {
            /*
            I task;
            if(_inputPipe.TryTake(out task, Timeout))
            {
                
                O result = this.Execute(task);
                _outputPipe.TryAdd(result);
            }
            */
            O result = this.Execute(_inputPipe.Take());
            _outputPipe.TryAdd(result);
            
        }

        /// <summary>
        /// Executes a single task. The result will be added to the output pipe after execution. 
        /// </summary>
        /// <param name="task">task from input pipe</param>
        /// <returns>executed task (result) for output pipe</returns>
        protected abstract O Execute(I task);

    }
}
