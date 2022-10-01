using System;
using System.Threading;

namespace Hw3.Tests;

public class SingleInitializationSingleton
{
    private static readonly object Locker = new();

    private static Lazy<SingleInitializationSingleton> _lazyHolder = new(() => new SingleInitializationSingleton());

    private static volatile bool _isInitialized;

    public const int DefaultDelay = 3_000;

    public int Delay { get; }

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        Thread.Sleep(delay);
    }

    internal static void Reset()
    {
        _isInitialized = false;
        _lazyHolder = new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton());
    }

    public static void Initialize(int delay)
    {
        if (_isInitialized) throw new InvalidOperationException();
        lock (Locker)
        {
            if (_isInitialized) throw new InvalidOperationException();
            _lazyHolder = new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton(delay));
            _isInitialized = true;
        }
    }

    public static SingleInitializationSingleton Instance => _lazyHolder.Value;
}