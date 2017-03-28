using System;
using System.Collections.Generic;

namespace Reactor.Groups
{
    public interface IGroup
    {
        IEnumerable<Type> TargettedComponents { get; }
    }
}