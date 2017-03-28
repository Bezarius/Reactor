using System;
using Reactor.Entities;

namespace Reactor.Groups
{
    public interface IHasPredicate
    {
        bool CanProcessEntity(IEntity entity);
    }
}