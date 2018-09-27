using System.Collections.Generic;
using Assets.Reactor.Examples.UsingBlueprints.Components;
using Reactor.Blueprints;
using Reactor.Components;

namespace Assets.Reactor.Examples.UsingBlueprints.Blueprints
{
    public class PlayerBlueprint : IBlueprint
    {
        public float DefaultHealth { get; set; }

        public string Name { get; set; }

        public PlayerBlueprint(string name, float defaultHealth = 100.0f)
        {
            DefaultHealth = defaultHealth;
            Name = name;
        }

        public IEnumerable<IComponent> Build()
        {
            return new List<IComponent>
            {
                new HasName
                {
                    Name = Name
                },
                new WithHealthComponent
                {
                    CurrentHealth = DefaultHealth,
                    MaxHealth = DefaultHealth
                }
            };
        }
    }
}