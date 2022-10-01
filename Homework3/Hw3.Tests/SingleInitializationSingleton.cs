using System;
using System.Threading;

namespace Hw3.Tests;

public class SingleInitializationSingleton
{
    private static readonly object Locker = new();

    private static Lazy<SingleInitializationSingleton> _lazyHolder = SetupHolder();

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
        _lazyHolder = SetupHolder();
    }

    public static void Initialize(int delay)
    {
        if (_isInitialized) DoubleInitThrow();
        lock (Locker)
        {
            if (_isInitialized) DoubleInitThrow();
            _lazyHolder = SetupHolder(delay);
            _isInitialized = true;
        }
    }

    private static void DoubleInitThrow()=> 
        throw new InvalidOperationException("Double initialization occured");
    

    private static Lazy<SingleInitializationSingleton> SetupHolder(int delay = DefaultDelay) =>
        new(LazyHolderSingletonInitiator(delay));

    private static Func<SingleInitializationSingleton> LazyHolderSingletonInitiator(int delay) =>
        () => new SingleInitializationSingleton(delay);

    public static SingleInitializationSingleton Instance => _lazyHolder.Value;
}