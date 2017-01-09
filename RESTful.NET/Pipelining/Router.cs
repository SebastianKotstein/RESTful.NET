using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Pipelining
{
    /// <summary>
    /// Represents a special processor (filter) which consists of an input pipe, an execution and routing unit implemented in <see cref="Route(I)"/> and multiple named output pipes. 
    /// In comparison to a <see cref="SimpleProcessor{I, O}"/>, the result of the executed task can be forwarded to a specific or even multiple outpute pipes depeding on the context of the task.
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    public abstract class Router<I,O> : Processor
    {
        protected BlockingCollection<I> _inputPipe = new BlockingCollection<I>();
        protected IDictionary<string, BlockingCollection<O>> _outputPipes = new Dictionary<string, BlockingCollection<O>>();

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
        /// Adds a named output pipe.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pipe"></param>
        public void AddOutputPipe(string name, BlockingCollection<O> pipe)
        {
            _outputPipes.Add(name, pipe);
        }

        /// <summary>
        /// Removes a named output pipe
        /// </summary>
        /// <param name="name"></param>
        public void RemoveOutputPipe(string name)
        {
            _outputPipes.Remove(name);
        }

        /// <summary>
        /// Gets a named output pipe
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BlockingCollection<O> GetOutputPipe(string name)
        {
            return _outputPipes[name];
        }
        protected override void Execute()
        {

            I task;
            if (_inputPipe.TryTake(out task, Timeout))
            {
                Route(task);
            }
        }

        /// <summary>
        /// Forwards a task to the named output pipe
        /// </summary>
        /// <param name="name">name of the output pipe</param>
        /// <param name="task">task to be forwared</param>
        protected void Forward(string name, O task)
        {
            _outputPipes[name].TryAdd(task);
        }
        /// <summary>
        /// Contains the route logic of this router.
        /// Use the <see cref="Forward(string, O)"/> method for forwarding the result object to the corresponding output pipe.
        /// </summary>
        /// <param name="task">Incoming task</param>
        protected abstract void Route(I task);

    }
}
