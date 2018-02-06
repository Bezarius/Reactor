using System.Collections.Generic;
using Reactor.Components;

namespace Reactor.Blueprints
{
    public interface IBlueprint
    {
        IEnumerable<IComponent> Build();
    }
}