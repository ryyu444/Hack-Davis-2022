using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// funny proc gen
/// dont look at the mesh generation its some scuffed ass shit
/// this is what happens when you dont google code and try to do stupid shit on your own
/// </summary>
[ExecuteInEditMode]
public class MapGen : MonoBehaviour
{
    public MeshFilter floorMesh;
    public Transform parent;
    public int width;
    public int height;
    public float quadSize = 0.2f;
    public float colorAmpl;

    public NoiseOctaveScriptableObject noiseSettings;
    public ItemGenScriptableObject itemGenSettings;

    private float CalculateNoise(Vector2 pos)
    {
        float result = 0f;
        foreach(var octave in noiseSettings.noiseOctaves)
        {
            Vector2 coords = octave.frequency * pos + octave.offset;
            float raw = Mathf.PerlinNoise(coords.x, coords.y) - 0.5f;
            result += Mathf.Pow(raw * octave.amplitude,octave.exponent);
        }
        return result;
    }

    private List<GameObject> CheckForItemGen(Vector3 position)
    {
        var list = new List<GameObject>();
        float seed = Random.Range(0, 1000);
        foreach(var v in itemGenSettings.itemGenData)
        {
            if(seed >= v.range.x && seed <= v.range.y)
            {
                list.Add(Instantiate(v.item, position + v.offset, Quaternion.identity,parent));
            }
        }
        return list;
    }

    [ContextMenu("MakePlane")]
    public void GenerateTesselatedPlane()
    {
        GenerateTesselatedPlane(Vector3.zero);
    }

    public List<GameObject> GenerateTesselatedPlane(Vector3 position, bool genItems = false)
    {
        var generatedItems = new List<GameObject>();

        var plane = new Mesh();

        int vSize = (width + 1) * (height + 1);
        Vector3[] verts = new Vector3[vSize];
        Color[] clrs = new Color[vSize];
        Vector2[] uvs = new Vector2[vSize];
        //Vertices
        for(int z= 0;z <= height;z++)
        {
            for(int x = 0;x <= width;x++)
            {
                var basePos = position + new Vector3(x * quadSize, 0, z * quadSize);
                //Displacement

                basePos += Vector3.up * CalculateNoise(new Vector2(basePos.x, basePos.z));
                //colors
                float r = CalculateNoise(new Vector2(basePos.x, basePos.z) + new Vector2(100.5f,100.5f));
                float g = CalculateNoise(new Vector2(basePos.x, basePos.z) + new Vector2(10.5f,100.5f));
                float b = CalculateNoise(new Vector2(basePos.x, basePos.z) + new Vector2(100.5f,10.5f));
                clrs[z * (width + 1) + x] = new Color(colorAmpl * r/255, colorAmpl * g / 255, colorAmpl * b / 255);
                verts[z * (width + 1) + x] = basePos;
                uvs[z * (width + 1) + x] = new Vector2(x / width, z / height);
                generatedItems.AddRange(CheckForItemGen(basePos));
            }
        }
        plane.vertices = verts;
        plane.uv = uvs;
        plane.colors = clrs;
        //Tris
        int[] tris = new int[width * height * 6];
        int counter = 0;
        for(int i = 0;i < tris.Length;i+=6)
        {
            int bottomLeft = counter + counter / width;
            tris[i] = bottomLeft;
            tris[i+1] = bottomLeft + width + 1;
            tris[i+2] = bottomLeft + 1;

            tris[i+3] = bottomLeft + width + 1;
            tris[i+4] = bottomLeft + width + 2;
            tris[i+5] = bottomLeft + 1;
            counter++;
        }
        plane.triangles = tris;

        plane.RecalculateNormals();
        plane.RecalculateTangents();
        DestroyImmediate(floorMesh.sharedMesh);
        floorMesh.sharedMesh = plane;
        UpdateCollider();
        return generatedItems;
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        var plane = new Mesh();
        floorMesh.mesh = plane;
        UpdateCollider();
    }

    private void UpdateCollider()
    {
        GetComponent<MeshCollider>().sharedMesh = floorMesh.sharedMesh;
    }

    private void OnDrawGizmosSelected()
    {
        /*var verts = floorMesh.sharedMesh.vertices;
        Gizmos.color = Color.red;
        foreach(var vert in verts)
        {
            Gizmos.DrawSphere(vert, 0.02f);
        }*/
    }
}
