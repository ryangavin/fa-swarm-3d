using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PubSub {

    private static Dictionary<Tuple<GameObject, Type>, List<Action<object>>> perObjectListeners = new Dictionary<Tuple<GameObject, Type>, List<Action<object>>>();
    private static Dictionary<Type, List<Action<object>>> globalListeners = new Dictionary<Type, List<Action<object>>>();

    public static void RegisterPerObjectListener<T>(GameObject target, Action<object> listener) where T : class {
        Tuple<GameObject, Type> perObjectKey = new Tuple<GameObject, Type>(target, typeof(T));
        if (!perObjectListeners.ContainsKey(perObjectKey)) {
            perObjectListeners[perObjectKey] = new List<Action<object>>();
        }

        perObjectListeners[perObjectKey].Add(listener);
    }

    public static void RegisterGlobalListener<T>(Action<object> listener) where T : class {
        if(!globalListeners.ContainsKey(typeof(T))) {
            globalListeners[typeof(T)] = new List<Action<object>>();
        }
        globalListeners[typeof(T)].Add(listener);
    }

    // TODO De-register

    public static void Publish<T>(GameObject target, T publishedEvent) {
        Tuple<GameObject, Type> perObjectKey = new Tuple<GameObject, Type>(target, typeof(T));
        if (perObjectListeners.ContainsKey(perObjectKey)) {
            List<Action<object>> perObjectListenerList = perObjectListeners[perObjectKey];
            foreach (Action<object> action in perObjectListenerList) {
                action(publishedEvent);
            }
        }

        if (globalListeners.ContainsKey(typeof(T))) {
            List<Action<object>> globalListenerList = globalListeners[typeof(T)];
            foreach (Action<object> action in globalListenerList) {
                action(publishedEvent);
            }
        }
    }

}
