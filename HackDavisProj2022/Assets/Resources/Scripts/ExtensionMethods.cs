using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{ 
    public static float AngleLerp(this float source, float target, float t)
    {
        return Mathf.Lerp(source, source + Mathf.DeltaAngle(source, target), t);
    }

    public static Vector3 ToXZPlane(this Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }

    public static Quaternion NoXAxis(this Quaternion rot)
    {
        rot.eulerAngles = new Vector3(0,rot.eulerAngles.y, rot.eulerAngles.z);
        return rot;
    }

    public static void MoveWithRotation(this Rigidbody target, Quaternion rotation, Vector2 movementVector, float speed)
    {
        Vector3 moveVec = (
            rotation.NoXAxis() *
            movementVector.normalized.ToXZPlane() *
            speed);

        target.velocity = new Vector3(moveVec.x, target.velocity.y, moveVec.z);
    }
}
