using System.Collections.Generic;
using Reactor.Entities;

namespace Reactor.Pools
{
    public interface IPoolQuery
    {
        IEnumerable<IEntity> Execute(IEnumerable<IEntity> entityList);
    }
}