using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Header("Game Event Listener")]
    [SerializeField] protected GameEvent _gameEvent;
    [SerializeField] protected UnityEvent _unityEvent;

    private void OnEnable()
    {
        _gameEvent.Register(this);
    }

    private void OnDisable()
    {
        _gameEvent.DeRegister(this);
    }

    public virtual void RaiseEvent()
    {
        _unityEvent.Invoke();
    }
}
