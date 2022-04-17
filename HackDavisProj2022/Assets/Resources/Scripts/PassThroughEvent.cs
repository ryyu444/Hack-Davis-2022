using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PassThroughEvent : MonoBehaviour
{
    public UnityEvent<object> OnCollision;

    public void PassThrough(object parameters)
    {
        OnCollision?.Invoke(parameters);
    }
}
