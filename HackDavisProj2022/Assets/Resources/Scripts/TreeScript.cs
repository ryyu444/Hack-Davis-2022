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
        ChangeCutState(0,Vector3.zero);
    }

    private bool ChangeCutState(int val,Vector3 dir)
    {
        if (val >= cutSegments.Length)
            return false;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.z, dir.x);
        for(int i = 0;i < cutSegments.Length; i++)
        {
            cutSegments[i].SetActive(i == val);
            cutSegments[i].transform.Rotate(new Vector3(0, -angle + 90f, 0),Space.World);
        }
        return val == cutSegments.Length - 1;
    }

    public void TrunkCollision(object source)
    {
        Chop(((GameObject)source).transform.position - stump.transform.position);
    }

    private void Chop(Vector3 dir)
    {
        chopState++;
        if(ChangeCutState(chopState,dir))
        {
            //Release da treeeeee
            mainTreeSegment.GetComponent<Rigidbody>().isKinematic = false;
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
