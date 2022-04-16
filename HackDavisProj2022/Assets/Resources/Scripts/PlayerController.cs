using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputInfo inputInfo;
    public ThirdPersonCameraController cameraController;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(cameraController.transform.rotation.NoXAxis() * inputInfo.movementVector.normalized.ToXZPlane() * Time.deltaTime * 10);
    }
}
