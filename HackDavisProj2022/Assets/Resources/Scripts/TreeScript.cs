using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Tree script LUL
/// leaf me alone
/// ignore this monolith of hot garbage shitcode
/// it works so w/e like lmaooooooo
/// </summary>
public class TreeScript : MonoBehaviour
{
    public GameObject mainTreeSegment;
    private Vector3 mainTreeInitPos;
    private Quaternion mainTreeInitRot;
    public GameObject stump;
    public GameObject[] cutSegments;

    public MeshRenderer[] toRegrow;

    public AudioSource source;
    public AudioClip chopSFX;
    public AudioClip woodSnapSFX;
    public AudioClip regrowSFX;

    private int chopState = 0;

    public bool startChopped;

    public bool Chopped { get => chopped; }
    [SerializeField]
    private bool chopped = false;

    private void Awake()
    {
        mainTreeInitPos = mainTreeSegment.transform.position;
        mainTreeInitRot = mainTreeSegment.transform.rotation;
        mainTreeSegment.GetComponent<Rigidbody>().isKinematic = true;
        ChangeCutState(0,Vector3.zero);
        chopped = startChopped;
        if(startChopped)
        {
            foreach (var v in toRegrow)
                v.gameObject.SetActive(false);
        }
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
        chopState = val;
        return val == cutSegments.Length - 1;
    }

    public void TrunkCollision(object source)
    {
        Chop(((GameObject)source).transform.position - stump.transform.position);
    }

    private void Chop(Vector3 dir)
    {
        if (chopped)
            return;
        chopState++;
        if (ChangeCutState(chopState,dir))
        {
            //Release da treeeeee
            source.PlayOneShot(chopSFX);
            StartCoroutine(Corout_SnapSFX());
            mainTreeSegment.GetComponent<Rigidbody>().isKinematic = false;
            GetComponentInChildren<ParticleSystem>().Play();
            chopped = true;
        }
    }

    private IEnumerator Corout_SnapSFX()
    {
        yield return new WaitForSeconds(0.35f);
        source.PlayOneShot(woodSnapSFX);
    }

    public void StartRegrowth()
    {
        foreach (var v in toRegrow)
            v.gameObject.SetActive(true);
        StartCoroutine(Corout_Regrow());
    }

    private IEnumerator Corout_Regrow()
    {
        source.PlayOneShot(regrowSFX);
        mainTreeSegment.GetComponent<Rigidbody>().isKinematic = true;
        mainTreeSegment.transform.position = mainTreeInitPos;
        mainTreeSegment.transform.rotation = mainTreeInitRot;
        var propertyBlocks = new MaterialPropertyBlock[toRegrow.Length];
        for(int i = 0;i < toRegrow.Length;i++)
        {
            propertyBlocks[i] = new MaterialPropertyBlock();
            toRegrow[i].GetPropertyBlock(propertyBlocks[i]);
        }

        float duration = 20f;
        float timer = 0f;

        float lower = transform.position.y - 2f;
        float upper = transform.position.y + 17f;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            float h = Mathf.Lerp(lower,upper,Mathf.Sqrt(timer / duration));
            foreach(var block in propertyBlocks)
            {
                block.SetFloat("_EdgeHeight", h);
            }
            for (int i = 0; i < toRegrow.Length; i++)
            {
                toRegrow[i].SetPropertyBlock(propertyBlocks[i]);
            }
            yield return new WaitForEndOfFrame();
        }
        chopped = false;
        ChangeCutState(0,Vector3.zero);
    }
}
