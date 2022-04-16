using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputInfo inputInfo;

    // Update is called once per frame
    void Update()
    {
        inputInfo.mouseDelta = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));

        inputInfo.movementVector = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            inputInfo.jumpPressedEvent?.Invoke();
        }
        if(Input.GetMouseButton(0))
        {
            inputInfo.LMBPressedEvent?.Invoke();
        }
        if (Input.GetMouseButton(1))
        {
            inputInfo.RMBPressedEvent?.Invoke();
        }
    }
}
