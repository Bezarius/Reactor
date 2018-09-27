using System.Collections.Generic;
using Reactor.Blueprints;
using Reactor.Components;
using Reactor.Unity.Components;

public class SpawnerBlueprint : IBlueprint
{
    public IEnumerable<IComponent> Build()
    {
        return new IComponent[]
        {
            new SpawnerComponent(),
            new ViewComponent()
        };
    }
}
