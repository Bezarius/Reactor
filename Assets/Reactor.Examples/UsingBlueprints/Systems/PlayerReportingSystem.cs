using Assets.Reactor.Examples.UsingBlueprints.Components;
using Assets.Reactor.Examples.UsingBlueprints.Groups;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using UnityEngine;

namespace Assets.Reactor.Examples.UsingBlueprints.Systems
{
    public class PlayerReportingSystem : ISetupSystem
    {
        public IGroup TargetGroup { get { return new PlayerGroup();} }

        public void Setup(IEntity entity)
        {
            var nameComponent = entity.GetComponent<HasName>();
            var healthComponent = entity.GetComponent<WithHealthComponent>();

            var message = string.Format("{0} created with {1}/{2}",
                nameComponent.Name,
                healthComponent.CurrentHealth, healthComponent.MaxHealth);

            Debug.Log(message);
        }
    }
}