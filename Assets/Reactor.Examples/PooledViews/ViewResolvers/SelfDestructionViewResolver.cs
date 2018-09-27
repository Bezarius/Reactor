using System;
using Assets.Game.SceneCollections;
using Assets.Reactor.Examples.PooledViews.Blueprints;
using Reactor.Blueprints;
using Reactor.Entities;
using Reactor.Groups;
using Reactor.Pools;
using Reactor.Unity.Components;
using Reactor.Unity.Systems;
using UnityEngine;

namespace Assets.Reactor.Examples.PooledViews.ViewResolvers
{
    public enum DestructableTypes
    {
        PooledPrefab
    }

    public sealed class SelfDestructionViewResolver : PooledViewResolverSystem
    {
        public override IGroup TargetGroup { get; protected set; }

        private readonly Transform _parentTrasform = GameObject.Find("Destructables").transform;

        public SelfDestructionViewResolver(PrefabLoader<DestructableTypes> prefabLoader) : base(prefabLoader)
        {
            TargetGroup = new GroupBuilder()
                .WithComponent<SelfDestructComponent>()
                .WithComponent<ViewComponent>()
                .Build();
        }

        protected override void Resolve(IEntity entity)
        {
            var component = entity.GetComponent<SelfDestructComponent>();
            var typeId = (int)component.DestructableTypes;
            var view = AllocateView(entity, typeId);
            view.name = string.Format("destructable-{0}", entity.Id);
        }
    }

    public interface IDestructableFactory
    {
        IEntity Create(DestructableTypes type, Vector3 pos, Quaternion rotation, Vector3 scale);
    }

    public class DestructableFactory : IDestructableFactory
    {
        private readonly IPool _pool;

        private readonly Func<IBlueprint>[] _blueprints;

        public DestructableFactory(IPoolManager poolManager)
        {
            _pool = poolManager.GetPool("destructablePool");

            // todo: create enum to blueprint editor
            var typeLen = Enum.GetValues(typeof(DestructableTypes)).Length;
            _blueprints = new Func<IBlueprint>[typeLen];
            _blueprints[(int)DestructableTypes.PooledPrefab] = () => new SelfDestructBlueprint();

        }

        public IEntity Create(DestructableTypes type, Vector3 pos, Quaternion rotation, Vector3 scale)
        {
            var vfx = _pool.CreateEntity(_blueprints[(int)type]());
            var view = vfx.GetComponent<ViewComponent>();
            var tr = view.GameObject.transform;
            tr.position = pos;
            tr.rotation = rotation;
            tr.localScale = scale;
            return vfx;
        }
    }
}