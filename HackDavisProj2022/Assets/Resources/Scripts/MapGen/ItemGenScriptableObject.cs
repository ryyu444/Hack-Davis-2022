using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemGenScriptableObject")]
public class ItemGenScriptableObject : ScriptableObject
{
    public List<Item> itemGenData;
}

[System.Serializable]
public class Item
{
    public GameObject item;
    public Vector2 range;
    public Vector3 offset;
}
