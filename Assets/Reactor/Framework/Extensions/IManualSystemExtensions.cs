using System;
using Reactor.Systems;
using UniRx;

namespace Reactor.Extensions
{
    public static class IManualSystemExtensions
    {
        public static IObservable<long> WaitForScene(this IManualSystem manualSystem)
        {
            return Observable.EveryUpdate().First();
        }  
    }
}