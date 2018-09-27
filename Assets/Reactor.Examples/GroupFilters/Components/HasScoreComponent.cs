using System;
using Reactor.Components;
using UniRx;

namespace Assets.Reactor.Examples.GroupFilters.Components
{
    public class HasScoreComponent : EntityComponent<HasScoreComponent>, IDisposable
    {
        public string Name { get; set; }
        public ReactiveProperty<int> Score { get; set; }

        public HasScoreComponent()
        {
            Score = new IntReactiveProperty(0);
        }

        public void Dispose()
        {
            Score.Dispose();
        }
    }
}