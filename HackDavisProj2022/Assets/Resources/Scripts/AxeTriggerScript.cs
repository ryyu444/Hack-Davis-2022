using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeTriggerScript : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TreeTrunk"))
        {
            other.GetComponent<PassThroughEvent>().PassThrough(gameObject);
        }
    }
}
