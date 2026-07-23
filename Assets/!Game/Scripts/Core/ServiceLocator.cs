using System;
using System.Collections.Generic;
using UnityEngine;

namespace Delphin.Core
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnLoad()
        {
            services.Clear();
        }

        public static void Register<TService>(TService instance) where TService : class
        {
            var type = typeof(TService);
            if (services.ContainsKey(type))
                throw new InvalidOperationException($"Service of type {type.Name} is already registered.");

            services[type] = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public static void Unregister<TService>() where TService : class
        {
            services.Remove(typeof(TService));
        }

        public static TService Get<TService>() where TService : class
        {
            if (services.TryGetValue(typeof(TService), out var service))
                return (TService)service;

            throw new InvalidOperationException($"Service of type {typeof(TService).Name} is not registered.");
        }

        public static bool TryGet<TService>(out TService service) where TService : class
        {
            if (services.TryGetValue(typeof(TService), out var raw))
            {
                service = (TService)raw;
                return true;
            }

            service = null;
            return false;
        }

        public static bool IsRegistered<TService>() where TService : class
        {
            return services.ContainsKey(typeof(TService));
        }
    }
}
