using Assets.Reactor.Examples.PluginExample.HelloWorldPlugin.components;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Systems;
using UnityEngine;

namespace Assets.Reactor.Examples.PluginExample.HelloWorldPlugin.systems
{
    public class OutputHelloWorldSystem : ISetupSystem
    {
        public IGroup TargetGroup { get { return new Group(typeof(SayHelloWorldComponent));} }

        public void Setup(IEntity entity)
        {
            Debug.Log(string.Format("Entity {0} Says Hello World", entity.Id));
        }
    }
}