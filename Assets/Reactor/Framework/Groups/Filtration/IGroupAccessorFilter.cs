using System;
using System.Collections.Generic;
using Reactor.Entities;
using Reactor.Groups;

namespace Assets.Reactor.Framework.Groups.Filtration
{
    public interface IGroupAccessorFilter
    {
        IGroupAccessor GroupAccessor { get; }
    }

    public interface IGroupAccessorFilter<T> : IGroupAccessorFilter
    {
        IEnumerable<T> Filter();
    }

    public interface IGroupAccessorFilter<TOutput, TInput> : IGroupAccessorFilter
    {
        IEnumerable<TOutput> Filter(TInput input);
    }
}