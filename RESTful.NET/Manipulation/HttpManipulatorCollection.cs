using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Core;

namespace SKotstein.Net.Http.Manipulation
{
    public class HttpManipulatorCollection<T> : HttpManipulator<T>
    {
        private IList<HttpManipulator<T>> _manipulators;

        public HttpManipulatorCollection()
        {
            _manipulators = new List<HttpManipulator<T>>();
        }

        public override void Manipulate(T context)
        {
            foreach(HttpManipulator<T> mp in _manipulators)
            {
                mp.Manipulate(context);
            }
        }

        public IList<string> Names
        {
            get
            {
                IList<string> names = new List<string>();
                foreach(HttpManipulator<T> mp in _manipulators)
                {
                    names.Add(mp.Name);
                }
                return names;
            }
        }

        public int Size
        {
            get
            {
                return _manipulators.Count;
            }
        }
        
        public void Remove(int index)
        {
            _manipulators.RemoveAt(index);
        } 

        public void Remove(string name)
        {
            IList<HttpManipulator<T>> toBeRemoved = new List<HttpManipulator<T>>();
            for(int i = 0; i < _manipulators.Count; i++)
            {
                if (_manipulators[i].Name.CompareTo(name) == 0)
                {
                    toBeRemoved.Add(_manipulators[i]);
                }
            }
            foreach(HttpManipulator<T> mp in toBeRemoved)
            {
                _manipulators.Remove(mp);
            }
        }

        public void Add(HttpManipulator<T> mp)
        {
            _manipulators.Add(mp);
        }

        public void Inset(int index, HttpManipulator<T> mp)
        {
            _manipulators.Insert(index, mp);
        }
    }
}
