namespace OnlineShopZitro.Infrastructure.Redis;

using StackExchange.Redis;

public class RedisConnectionFactory
{
    private static Lazy<ConnectionMultiplexer>? _lazyConnection;
    private static readonly object _lock = new object();

    public static void Initialize(string connectionString)
    {
        lock (_lock)
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect(connectionString));
        }
    }

    public static ConnectionMultiplexer Connection
    {
        get
        {
            if (_lazyConnection == null)
                throw new InvalidOperationException("Redis connection not initialized");
            return _lazyConnection.Value;
        }
    }

    //public static IDatabase GetDatabase() => Connection.GetDatabase();
    public static IDatabase GetDatabase(int db = 0) => Connection.GetDatabase(db);

}