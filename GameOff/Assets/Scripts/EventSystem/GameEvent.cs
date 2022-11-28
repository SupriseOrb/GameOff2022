using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SupriseOrb/Game Event", fileName = "New Game Event")]
public class GameEvent : ScriptableObject
{
    private HashSet<GameEventListener> _listeners = new HashSet<GameEventListener>();

    public void Invoke()
    {
        foreach(GameEventListener eventListener in _listeners)
        {
            eventListener.RaiseEvent();
        }
    }

    public void Register(GameEventListener gameEventListener)
    {
        _listeners.Add(gameEventListener);
    }

    public void DeRegister(GameEventListener gameEventListener)
    {
        _listeners.Remove(gameEventListener);
    }
}
