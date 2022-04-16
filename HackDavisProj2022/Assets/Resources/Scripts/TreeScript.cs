using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Tree script LUL
/// leaf me alone
/// </summary>
public class TreeScript : MonoBehaviour
{
    public GameObject mainTreeSegment;
    public GameObject stump;
    public GameObject[] cutSegments;

    private int chopState = 0;

    private void Awake()
    {
        mainTreeSegment.GetComponent<Rigidbody>().isKinematic = true;
        ChangeCutState(0);
    }

    private bool ChangeCutState(int val)
    {
        for(int i = 0;i < cutSegments.Length; i++)
        {
            cutSegments[i].SetActive(i == val);
        }
        return val == cutSegments.Length - 1;
    }

    [ContextMenu("Cut")]
    private void Chop()
    {
        chopState++;
        if(ChangeCutState(chopState))
        {
            //Release da treeeeee
            mainTreeSegment.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
