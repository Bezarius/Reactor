﻿using Assets.Reactor.Examples.GroupFilters.Components;
using Reactor.Blueprints;
using Reactor.Entities;

namespace Assets.Reactor.Examples.GroupFilters.Blueprints
{
    public class PlayerBlueprint : IBlueprint
    {
        public string Name { get; private set; }
        public int Score { get; private set; }

        public PlayerBlueprint(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public void Apply(IEntity entity)
        {
            var scoreComponent = new HasScoreComponent { Name = Name };
            scoreComponent.Score.Value = Score;
            entity.AddComponent(scoreComponent);
        }
    }
}