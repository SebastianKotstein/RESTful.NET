using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKotstein.Net.Http.Threading
{
    public class ThreadTask<O>
    {
        private volatile bool _idle;
        private bool _shouldStop;

        public delegate void ExecuteTask(O task);

        private ExecuteTask task = null;
        private O param;

        public bool Idle
        {
            get
            {
                return _idle;
            }

            private set
            {
                _idle = value;
            }
        }

        public bool ShouldStop
        {
            get
            {
                return _shouldStop;
            }

            set
            {
                _shouldStop = value;
            }
        }

        internal void DoWork()
        {
            while (!ShouldStop)
            {
                if (Idle)
                {

                }
                else
                {
                    task(param);
                    task = null;
                    Idle = false;
                }
            }
        }

        public bool LoadJob(ExecuteTask et, O p)
        {
            if (Idle)
            {
                task = et;
                param = p;
                Idle = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}
