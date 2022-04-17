using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input Info Asset")]
public class InputInfo : ScriptableObject
{
    public Vector2 mouseDelta;
    public Vector2 movementVector;

    //Events
    public System.Action jumpPressedEvent;
    public System.Action LMBPressedEvent;
    public System.Action RMBPressedEvent;

    private void OnEnable()
    {
        jumpPressedEvent = null;
        LMBPressedEvent = null;
        RMBPressedEvent = null;
    }
}