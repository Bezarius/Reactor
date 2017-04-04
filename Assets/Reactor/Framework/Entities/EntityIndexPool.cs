using System.Collections.Generic;

namespace Reactor.Entities
{
    public class EntityIndexPool : IEntityIndexPool
    {
        private int _counter = -1;
        private readonly Queue<int> _ints;

        public EntityIndexPool()
        {
            _ints = new Queue<int>();
        }

        public int GetId()
        {
            if (_ints.Count > 0)
                return _ints.Dequeue();
            _counter++;
            return _counter;
        }

        public void Release(int id)
        {
            _ints.Enqueue(id);
        }
    }
}