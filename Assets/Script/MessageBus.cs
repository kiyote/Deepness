using System;
using System.Collections.Generic;

public delegate void EventHandler<TEventArgs>(object sender, TEventArgs eventArgs) where TEventArgs : EventArgs;

public class MessageBus
{
#if WEAK_MESSAGEBUS
    private readonly Dictionary<Type, List<WeakReference>> _eventHandlers;
    private readonly List<WeakReference> _purge;
#else
    private readonly Dictionary<Type, List<object>> _eventHandlers;
#endif

    private static object _lock = new object();
    private static MessageBus _instance;

    public MessageBus()
    {
#if WEAK_MESSAGEBUS
        _eventHandlers = new Dictionary<Type, List<WeakReference>>();
        _purge = new List<WeakReference>();
#else
        _eventHandlers = new Dictionary<Type, List<object>>();
#endif
    }

    public static MessageBus Get()
    {
        lock(_lock)
        {
            if (_instance == null)
            {
                _instance = new MessageBus();
            }

            return _instance;
        }
    }

#if WEAK_MESSAGEBUS
    private List<WeakReference> GetHandlers<T>()
    {
        List<WeakReference> handlers;
        Type eventType = typeof(T);
        if (_eventHandlers.ContainsKey(eventType) == false)
        {
            handlers = new List<WeakReference>();
            _eventHandlers.Add(eventType, handlers);
        }
        else
        {
            handlers = _eventHandlers[eventType];
        }

        return handlers;
    }
#else
    private List<object> GetHandlers<T>()
    {
        List<object> handlers;
        Type eventType = typeof(T);
        if (_eventHandlers.ContainsKey(eventType) == false)
        {
            handlers = new List<object>();
            _eventHandlers.Add(eventType, handlers);
        }
        else
        {
            handlers = _eventHandlers[eventType];
        }

        return handlers;
    }
#endif

    public void Subscribe<T>(EventHandler<T> eventHandler) where T : EventArgs
    {
#if WEAK_MESSAGEBUS
        GetHandlers<T>().Add(new WeakReference(eventHandler));
#else
        GetHandlers<T>().Add(eventHandler);
#endif
    }

    public void Unsubscribe<T>(EventHandler<T> eventHandler) where T : EventArgs
    {
#if WEAK_MESSAGEBUS
        List<WeakReference> handlers = GetHandlers<T>();
        foreach (WeakReference r in handlers)
        {
            if (ReferenceEquals(r.Target, eventHandler))
            {
                handlers.Remove(r);
                break;
            }
        }
#else
        List<object> handlers = GetHandlers<T>();
        foreach (object handler in handlers)
        {
            if (ReferenceEquals(handler, eventHandler))
            {
                handlers.Remove(handler);
                break;
            }
        }
#endif

    }

    public void Publish<T>(object sender, T eventArgs) where T : EventArgs
    {
#if WEAK_MESSAGEBUS
        List<WeakReference> handlers = GetHandlers<T>();
        foreach (WeakReference r in handlers)
        {
            if (r.IsAlive)
            {
                var eventHandler = r.Target as EventHandler<T>;
                eventHandler(sender, eventArgs);
            }
            else
            {
                _purge.Add(r);
            }
        }

        if (_purge.Count > 0)
        {
            foreach (WeakReference r in _purge)
            {
                handlers.Remove(r);
            }
            _purge.Clear();
        }
#else
        List<object> handlers = GetHandlers<T>();
        foreach (object handler in handlers)
        {
            var eventHandler = handler as EventHandler<T>;
            eventHandler(sender, eventArgs);
        }
#endif
    }
}
