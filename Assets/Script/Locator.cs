using System;
using System.Collections.Generic;

public static class Locator<T>
{
    private static readonly Dictionary<Type, List<T>> _locateElements = new();

    public static List<T> Get<U>() where U : T
    {
        Type type = typeof(U);

        if (!_locateElements.TryGetValue(type, out var list))
        {
            list = new List<T>();
            _locateElements[type] = list;
        }

        return list;
    }

    public static void Bind(T element)
    {
        if (element == null)
            return;

        Type type = element.GetType();

        if (!_locateElements.TryGetValue(type, out var list))
        {
            list = new List<T>();
            _locateElements[type] = list;
        }

        if (!list.Contains(element))
            list.Add(element);
    }

    public static void UnBind(T element)
    {
        if (element == null)
            return;

        Type type = element.GetType();

        if (_locateElements.TryGetValue(type, out var list))
            list.Remove(element);
    }
}