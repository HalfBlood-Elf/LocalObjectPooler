namespace LocalObjectPooler
{
    public interface IPoolableItem: ISetupable, IReturnableToPool
    {
        
    }

    public interface IReturnableToPool
    {
        void OnReturnToPool();
    }

    public interface ISetupable
    {
        void Setup(PoolableItemSetupParameters setupParameters);
    }

    public class PoolableItemSetupParameters { }
}
