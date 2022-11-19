using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SystemTextTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset _story;
    [SerializeField] private UnityEvent _endEvent;
    

    public void TriggerStory()
    {
        SystemTextManager.Instance.StartStory(_story, _endEvent);
    }
}
