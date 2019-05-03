using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus {

    // Subscriptions to events with a specific "source"
    private static Dictionary<Tuple<GameObject, Type>, List<Action<object>>> sourceListeners = new Dictionary<Tuple<GameObject, Type>, List<Action<object>>>();

    // Subscriptions to events with a specific "target"
    private static Dictionary<Tuple<GameObject, Type>, List<Action<object>>> targetListeners = new Dictionary<Tuple<GameObject, Type>, List<Action<object>>>();

    // Subscripions to all instances of an event
    private static Dictionary<Type, List<Action<object>>> globalListeners = new Dictionary<Type, List<Action<object>>>();


    // Register listener - takes a mode to switch between subscription types
    public static void Register<T>(Action<object> listener, GameObject source = null, GameObject target = null, bool global=false) where T : Event {
        if (source != null) {
            Tuple<GameObject, Type> key = new Tuple<GameObject, Type>(source, typeof(T));
            if (sourceListeners.ContainsKey(key) == false) {
                sourceListeners.Add(key, new List<Action<object>>());
            }
            sourceListeners[key].Add(listener);
        }

        if (target != null) {
            Tuple<GameObject, Type> key = new Tuple<GameObject, Type>(target, typeof(T));
            if (targetListeners.ContainsKey(key) == false) {
                targetListeners.Add(key, new List<Action<object>>());
            }
            targetListeners[key].Add(listener);
        }

        if (global) {
            Type key = typeof(T);
            if (globalListeners.ContainsKey(key) == false) {
                globalListeners.Add(key, new List<Action<object>>());
            }
            globalListeners[key].Add(listener);
        }
    }

    // Deregisters all global listeners and any source/target listeners if source and targets are provided
    public static void DeRegister<T>(Action<object> listener, GameObject source = null, GameObject target = null) where T : Event {

        if (source != null) {
            Tuple<GameObject, Type> key = new Tuple<GameObject, Type>(source, typeof(T));
            if (sourceListeners.ContainsKey(key)) {
                sourceListeners[key].Remove(listener);
            }
        }

        if (target != null) {
            Tuple<GameObject, Type> key = new Tuple<GameObject, Type>(target, typeof(T));
            if (targetListeners.ContainsKey(key)) {
                targetListeners[key].Remove(listener);
            }
        }

        if (globalListeners.ContainsKey(typeof(T))) {
            globalListeners[typeof(T)].Remove(listener);
        }

    }

    // Publish an event
    public static void Publish<T>(T publishedEvent) where T : Event {
        if (publishedEvent.source != null) {
            Tuple<GameObject, Type> key = new Tuple<GameObject, Type>(publishedEvent.source, typeof(T));
            if (sourceListeners.ContainsKey(key)) {
                foreach (Action<object> action in new List<Action<object>>(sourceListeners[key])) {
                    action(publishedEvent);
                }
            }
        }

        if (publishedEvent.target != null) {
            Tuple<GameObject, Type> key = new Tuple<GameObject, Type>(publishedEvent.target, typeof(T));
            if (targetListeners.ContainsKey(key)) {
                foreach (Action<object> action in new List<Action<object>>(targetListeners[key])) {
                    action(publishedEvent);
                }
            }
        }

        Type globalKey = typeof(T);
        if (globalListeners.ContainsKey(globalKey)) {
            foreach (Action<object> action in new List<Action<object>>(globalListeners[globalKey])) {
                action(publishedEvent);
            }
        }
    }
}
