using System;
using System.Collections.Generic;

namespace Reactor.Groups
{
    public class Group : IGroup
    {
        public IEnumerable<Type> TargettedComponents { get; private set; }
        
        public Group(params Type[] targettedComponents)
        {
            TargettedComponents = targettedComponents;
        }
    }
}