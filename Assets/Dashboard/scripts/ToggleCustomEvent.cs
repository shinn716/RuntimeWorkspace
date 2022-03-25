using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleCustomEvent : MonoBehaviour
{
    public UnityEvent isOnEvents = new UnityEvent();
    public UnityEvent isOffEvents = new UnityEvent();
    public void OnValueChange(bool isOn)
    {
        if (isOn)
            isOnEvents.Invoke();
        else
            isOffEvents.Invoke();
    }
}
