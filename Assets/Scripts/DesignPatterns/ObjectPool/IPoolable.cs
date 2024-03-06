namespace Testing.ObjectPool
{
    public interface IPoolable
    {
        public PoolableStatus GetStatus();
        public void SetStatus(PoolableStatus status);
    }

    public enum PoolableStatus
    {
        Inactive,
        Active
    }
}