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
            int id;
            if (_ints.Count > 0)
                id = _ints.Dequeue();
            else
            {
                _counter++;
                id = _counter;
            }
            return id;
        }

        public void Release(int id)
        {
            _ints.Enqueue(id);
        }
    }
}