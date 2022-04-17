using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputInfo inputInfo;
    public ThirdPersonCameraController cameraController;
    public SphereCollider groundedCollider;
    public BoxCollider actionCollider;
    public Rigidbody rb;
    public GameObject modelContainer;

    public ParticleSystem wandParticles;

    public Animator spiritController;
    public Animator lumberJackController;
    public bool isLumberjack = true;

    [HideInInspector]
    public TreeScript growthTarget;
    public Animator animator
    {
        get
        {
            if (isLumberjack)
                return lumberJackController;
            return spiritController;
        }
    }
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

    [ContextMenu("Switch Characters")]
    public void ToggleCharacter()
    {
        SetCharacter(!isLumberjack);
    }

    public void SetCharacter(bool toLumberjack)
    {
        isLumberjack = toLumberjack;
        lumberJackController.gameObject.SetActive(toLumberjack);
        spiritController.gameObject.SetActive(!toLumberjack);
    }
}
