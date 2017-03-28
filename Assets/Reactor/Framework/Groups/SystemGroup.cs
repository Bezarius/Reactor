using System;
using System.Collections.Generic;

namespace Reactor.Groups
{
    public class SystemGroupKey : IGroup
    {
        public IEnumerable<Type> TargettedComponents { get; private set; }

        private readonly int _hash = 0;

        public SystemGroupKey(IEnumerable<Type> targettedComponents)
        {
            TargettedComponents = targettedComponents;

            unchecked
            {
                foreach (var component in TargettedComponents)
                {
                    var cHash = component.GetHashCode();
                    _hash = (_hash*397) ^ cHash;
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj != null && this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _hash;
        }
    }
}