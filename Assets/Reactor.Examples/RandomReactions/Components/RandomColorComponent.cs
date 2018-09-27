using Reactor.Components;
using UniRx;
using UnityEngine;

namespace Assets.Reactor.Examples.RandomReactions.Components
{
    public class RandomColorComponent : EntityComponent<RandomColorComponent>
    {
        public ReactiveProperty<Color> Color { get; set; }
        public float Elapsed { get; set; }
        public float NextChangeIn { get; set; }



        public RandomColorComponent()
        {
            Color = new ReactiveProperty<Color>();
        }
    }
}