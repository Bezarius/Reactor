using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Groups;

namespace Assets.Reactor.Framework.Groups
{
    public interface IGroupAccessorQuery
    {
        IEnumerable<IEntity> Execute(IGroupAccessor groupAccessor);
    }
}