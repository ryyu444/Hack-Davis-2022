using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputInfo inputInfo;
    public ThirdPersonCameraController cameraController;
    public SphereCollider groundedCollider;
    public Rigidbody rb;
    public Animator animator;
    public LayerMask groundedMask;
    

    private AbstractPlayerState currentState;
    public string displ;

    private void Awake()
    {
        ChangeState(new PlayerGroundedState());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
        displ = currentState.GetType().Name;
    }

    //Utils
    public bool IsGrounded()
    {
        var hits = Physics.OverlapSphere(
            groundedCollider.bounds.center,
            groundedCollider.radius,
            groundedMask);
        if(hits.Length > 0)
        {
            return true;
        }
        return false;
    }

    //State stuff
    public void ChangeState(AbstractPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        newState.Enter(this);
    }
}
