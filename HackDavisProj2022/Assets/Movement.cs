using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector3 vec;
    public float maxJumps = 2;
    void Update() {
        vec = transform.localPosition;

        vec.x += Input.GetAxis("Horizontal") * Time.deltaTime * 5;
        vec.z += Input.GetAxis("Vertical") * Time.deltaTime * 5;
        vec.y += Input.GetAxis("Jump") * Time.deltaTime * 10;

        transform.localPosition = vec;
    }
}
