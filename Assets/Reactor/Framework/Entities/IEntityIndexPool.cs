namespace Reactor.Entities
{
    public interface IEntityIndexPool
    {
        int GetId();
        void Release(int id);
    }
}