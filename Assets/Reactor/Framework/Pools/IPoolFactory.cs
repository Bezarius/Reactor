using Reactor.Factories;

namespace Reactor.Pools
{
    public interface IPoolFactory : IFactory<string, IPool>
    {
        
    }
}