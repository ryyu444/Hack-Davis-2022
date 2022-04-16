using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapGen : MonoBehaviour
{
    public MeshFilter floorMesh;
    public int width;
    public int height;
    public float quadSize = 0.2f;

    [ContextMenu("MakePlane")]
    public void GenerateTesselatedPlane(Vector3 position)
    {
        var plane = new Mesh();

        int vSize = (width + 1) * (height + 1);
        Vector3[] verts = new Vector3[vSize];
        Vector2[] uvs = new Vector2[vSize];
        //Vertices
        for(int z= 0;z <= height;z++)
        {
            for(int x = 0;x <= width;x++)
            {
                var basePos = position + new Vector3(x * quadSize, 0, z * quadSize);
                //Displacement
                float freq = 0.05f;
                float ampl = 7;
                float rawNoise = Mathf.PerlinNoise(freq * basePos.x, freq * basePos.z) - 0.5f;
                basePos += ampl * Vector3.up * rawNoise;
                //
                verts[z * (width + 1) + x] = basePos;
                uvs[z * (width + 1) + x] = new Vector2(x / width, z / height);
            }
        }
        plane.vertices = verts;
        plane.uv = uvs;
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
        floorMesh.mesh = plane;
        UpdateCollider();
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
