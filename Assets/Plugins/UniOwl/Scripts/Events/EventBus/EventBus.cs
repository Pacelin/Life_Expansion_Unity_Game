using System;
using System.Collections.Generic;

namespace UniOwl.Events
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<IEventListener>> listeners = new();

        public void AddListener<T>(IEventListener<T> listener) where T : struct, IEvent
        {
            Type eventType = typeof(T);
            if (!listeners.ContainsKey(eventType))
                listeners.Add(eventType, new List<IEventListener>());
            
            listeners[eventType].Add(listener);
        }
        
        public void RemoveListener<T>(IEventListener<T> listener) where T : struct, IEvent
        {
            Type eventType = typeof(T);
            if (listeners.TryGetValue(eventType, out var list))
                list.Remove(listener);
        }

        public void RaiseEvent<T>(T evt) where T : struct, IEvent
        {
            Type eventType = typeof(T);

            if (!listeners.TryGetValue(eventType, out var list))
                return;

            foreach (var listener in list)
                (listener as IEventListener<T>)!.OnEvent(evt);
        }
    }
}
