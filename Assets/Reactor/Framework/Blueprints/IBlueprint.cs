using Reactor.Entities;

namespace Reactor.Blueprints
{
    public interface IBlueprint
    {
        void Apply(IEntity entity);
    }
}