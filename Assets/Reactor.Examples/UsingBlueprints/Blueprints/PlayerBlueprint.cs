﻿using Assets.Reactor.Examples.UsingBlueprints.Components;
using Reactor.Blueprints;
using Reactor.Entities;

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

        public void Apply(IEntity entity)
        {
            entity.AddComponent(new HasName { Name = Name });
            entity.AddComponent(new WithHealthComponent { CurrentHealth = DefaultHealth, MaxHealth = DefaultHealth});
        }
    }
}