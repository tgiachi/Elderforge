namespace Elderforge.Core.Pools;

public class ObjectPool<T> where T : class
{
    private readonly Queue<T> _pool;
    private readonly Func<T> _createFunc;
    private readonly Action<T> _actionOnGet;
    private readonly Action<T> _actionOnRelease;
    private readonly Action<T> _actionOnDestroy;
    private readonly int _maxSize;

    public ObjectPool(
        Func<T> createFunc,
        Action<T> actionOnGet = null,
        Action<T> actionOnRelease = null,
        Action<T> actionOnDestroy = null,
        int defaultCapacity = 100,
        int maxSize = 10000
    )
    {
        _pool = new Queue<T>(defaultCapacity);
        _createFunc = createFunc;
        _actionOnGet = actionOnGet;
        _actionOnRelease = actionOnRelease;
        _actionOnDestroy = actionOnDestroy;
        _maxSize = maxSize;

        // Pre-populate the pool
        for (int i = 0; i < defaultCapacity; i++)
        {
            _pool.Enqueue(createFunc());
        }
    }

    public T Get()
    {
        T item = _pool.Count > 0 ? _pool.Dequeue() : _createFunc();
        _actionOnGet?.Invoke(item);
        return item;
    }

    public void Release(T? item)
    {
        if (item == null)
        {
            return;
        }

        if (_pool.Count < _maxSize)
        {
            _actionOnRelease?.Invoke(item);
            _pool.Enqueue(item);
        }
        else
        {
            _actionOnDestroy?.Invoke(item);
        }
    }
}
