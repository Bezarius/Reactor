using System.Collections.Generic;
using Assets.Reactor.Examples.GroupFilters.Components;
using Reactor.Blueprints;
using Reactor.Components;

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

        public IEnumerable<IComponent> Build()
        {
            return new List<IComponent>
            {
                new HasScoreComponent
                {
                    Name = Name,
                    Score = { Value = Score}
                }
            };
        }
    }
}