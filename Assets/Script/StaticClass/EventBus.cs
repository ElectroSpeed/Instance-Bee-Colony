using System;
using System.Collections.Generic;


public enum EventType //Mettre dans un autre fichier .cs
{
    NULL,
}

public static class EventBus
{
    private static readonly Dictionary<EventType, Delegate> eventTable = new();
    private static void AddDelegate(EventType eventType, Delegate listener)
    {
        if (eventTable.TryGetValue(eventType, out var existing))
        {
            if (existing != null && existing.GetType() != listener.GetType())
                throw new Exception($"Event type '{eventType}' utilisé avec plusieurs signatures différentes.");

            eventTable[eventType] = Delegate.Combine(existing, listener);
        }
        else
        {
            eventTable[eventType] = listener;
        }
    }

    private static void RemoveDelegate(EventType eventType, Delegate listener)
    {
        if (!eventTable.TryGetValue(eventType, out var existing) || existing == null)
        {
            return;
        }

        if (existing.GetType() != listener.GetType())
        {
            throw new Exception($"Tentative de desabonner une signature différente pour '{eventType}'.");
        }

        var newDel = Delegate.Remove(existing, listener);
        if (newDel == null)
            eventTable.Remove(eventType);
        else
            eventTable[eventType] = newDel;
    }


    /// <summary>
    /// Abonne une méthode à un événement identifié par un nom.
    /// Utilisé pour des événements possédant un seul paramètre.
    /// </summary>
    /// <typeparam name="T">Type du paramètre transmis lors de la publication de l'événement.</typeparam>
    /// <param name="eventType">Nom de l'événement.</param>
    /// <param name="listener">Méthode à exécuter lorsque l'événement est déclenché.</param>
    public static void Subscribe<T>(EventType eventType, Action<T> listener) =>
        AddDelegate(eventType, listener);


    /// <summary>
    /// Abonne une méthode à un événement possédant deux paramètres.
    /// </summary>
    /// <typeparam name="T1">Type du premier paramètre.</typeparam>
    /// <typeparam name="T2">Type du second paramètre.</typeparam>
    /// <param name="eventType">Nom de l'événement.</param>
    /// <param name="listener">Méthode à exécuter lorsque l'événement est déclenché.</param>
    public static void Subscribe<T1, T2>(EventType eventType, Action<T1, T2> listener) =>
        AddDelegate(eventType, listener);


    /// <summary>
    /// Abonne une méthode à un événement possédant trois paramètres.
    /// </summary>
    /// <typeparam name="T1">Type du premier paramètre.</typeparam>
    /// <typeparam name="T2">Type du second paramètre.</typeparam>
    /// <typeparam name="T3">Type du troisième paramètre.</typeparam>
    /// <param name="eventType">Nom de l'événement.</param>
    /// <param name="listener">Méthode à exécuter lorsque l'événement est déclenché.</param>
    public static void Subscribe<T1, T2, T3>(EventType eventType, Action<T1, T2, T3> listener) =>
        AddDelegate(eventType, listener);



    // ---------------------------------------------------------
    // Unsubscribe
    // ---------------------------------------------------------

    /// <summary>
    /// Désabonne une méthode d’un événement possédant un seul paramètre.
    /// Si la méthode n'était pas abonnée, l’appel est ignoré.
    /// </summary>
    /// <typeparam name="T">Type du paramètre de l'événement.</typeparam>
    /// <param name="eventType">Nom de l'événement.</param>
    /// <param name="listener">Méthode précédemment enregistrée à retirer.</param>
    public static void Unsubscribe<T>(EventType eventType, Action<T> listener) =>
        RemoveDelegate(eventType, listener);


    /// <summary>
    /// Désabonne une méthode d’un événement possédant deux paramètres.
    /// </summary>
    /// <typeparam name="T1">Type du premier paramètre.</typeparam>
    /// <typeparam name="T2">Type du second paramètre.</typeparam>
    /// <param name="eventType">Nom de l'événement.</param>
    /// <param name="listener">Méthode précédemment enregistrée à retirer.</param>
    public static void Unsubscribe<T1, T2>(EventType eventType, Action<T1, T2> listener) =>
        RemoveDelegate(eventType, listener);


    /// <summary>
    /// Désabonne une méthode d’un événement possédant trois paramètres.
    /// </summary>
    /// <typeparam name="T1">Type du premier paramètre.</typeparam>
    /// <typeparam name="T2">Type du second paramètre.</typeparam>
    /// <typeparam name="T3">Type du troisième paramètre.</typeparam>
    /// <param name="eventType">Nom de l'événement.</param>
    /// <param name="listener">Méthode précédemment enregistrée à retirer.</param>
    public static void Unsubscribe<T1, T2, T3>(EventType eventType, Action<T1, T2, T3> listener) =>
        RemoveDelegate(eventType, listener);



    // ---------------------------------------------------------
    // Publish
    // ---------------------------------------------------------

    /// <summary>
    /// Déclenche un événement possédant un seul paramètre.
    /// Exécute toutes les méthodes abonnées avec la valeur fournie.
    /// </summary>
    /// <typeparam name="T">Type du paramètre transmis.</typeparam>
    /// <param name="eventType">Nom de l'événement.</param>
    /// <param name="arg1">Valeur envoyée aux listeners.</param>
    public static void Publish<T>(EventType eventType, T arg1)
    {
        if (eventTable.TryGetValue(eventType, out var del))
            (del as Action<T>)?.Invoke(arg1);
    }


    /// <summary>
    /// Déclenche un événement possédant deux paramètres.
    /// </summary>
    /// <typeparam name="T1">Type du premier paramètre.</typeparam>
    /// <typeparam name="T2">Type du second paramètre.</typeparam>
    /// <param name="eventType">Nom de l'événement.</param>
    /// <param name="arg1">Premier argument envoyé.</param>
    /// <param name="arg2">Second argument envoyé.</param>
    public static void Publish<T1, T2>(EventType eventType, T1 arg1, T2 arg2)
    {
        if (eventTable.TryGetValue(eventType, out var del))
            (del as Action<T1, T2>)?.Invoke(arg1, arg2);
    }


    /// <summary>
    /// Déclenche un événement possédant trois paramètres.
    /// </summary>
    /// <typeparam name="T1">Type du premier paramètre.</typeparam>
    /// <typeparam name="T2">Type du second paramètre.</typeparam>
    /// <typeparam name="T3">Type du troisième paramètre.</typeparam>
    /// <param name="eventType">Nom de l'événement.</param>
    /// <param name="arg1">Premier argument envoyé.</param>
    /// <param name="arg2">Second argument envoyé.</param>
    /// <param name="arg3">Troisième argument envoyé.</param>
    public static void Publish<T1, T2, T3>(EventType eventType, T1 arg1, T2 arg2, T3 arg3)
    {
        if (eventTable.TryGetValue(eventType, out var del))
            (del as Action<T1, T2, T3>)?.Invoke(arg1, arg2, arg3);
    }





    public static void Clear()
    {
        eventTable.Clear();
    }
    public static bool HasSubscribers(EventType eventType)
    {
        return eventTable.TryGetValue(eventType, out var del) && del != null;
    }
}
