using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contest
{
    public class deque<T>
    {
        public List<T> _deque;
        public int Count;

        public deque(int _count)
        {
            _deque = new List<T>();
            Count = _count;
        }
        public deque(IEnumerable<T> deq)
        {
            _deque = new List<T>(deq);
            Count = deq.Count();
        }

        public int _count { get { return Count; } }
        public bool IsEmpty { get { return Count > 0; } }
        public void pushback(T element) //add left
        {
            _deque.Add(element);
            Count++;
        }
        public void pushfront(T element) //add right
        {
            _deque.Insert(0, element);
            Count++;
        }
        public T popback()
        {
            T element = _deque[Count - 1];
            _deque.RemoveAt(Count - 1);
            Count--;
            return element;
        }

        public T popfront()
        {
            T element = _deque[0];
            _deque.RemoveAt(0);
            Count--;
            return element;
        }

        public T _front
        { //see right
            get { return _deque[0]; }
        }
        public T _back
        { //see left
            get { return _deque[Count - 1]; }
        }

        public T this[int index]
        {
            get
            {
                return _deque[index];
            }
            set
            {
                _deque[index] = value;
            }
        }
    }
}
