using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MapManager : MonoBehaviour
{
    public int chunkWidth;
    public float vertSize;
    private float chunkSize;

    public int renderDistance;//from center chunk, distance to edge
    private int renderedSize;

    public GameObject tracked;
    public GameObject mapChunkPrefab;

    private GameObject[,] managedChunks;
    private Vector2Int currentStandingChunk = Vector2Int.zero;

    public ItemGenScriptableObject itemSpawn;
    public NoiseOctaveScriptableObject genData;

    //item managing
    public Dictionary<Vector2Int, List<GameObject>> itemLoad = new Dictionary<Vector2Int, List<GameObject>>();
    private float maxEntries = 500f;

    private void Awake()
    {
        chunkSize = vertSize * chunkWidth;
        renderedSize = 2 * renderDistance + 1;
        managedChunks = new GameObject[renderedSize,renderedSize];
        RegenerateChunks(new Vector2Int(0,0));
    }

    private void Update()
    {
        Vector3 trackedPos = tracked.transform.position;
        Vector2Int newStandingChunk = new Vector2Int((int)(trackedPos.x / chunkSize) - (trackedPos.x < 0 ? 1 : 0), (int)(trackedPos.z / chunkSize) - (trackedPos.z < 0 ? 1 : 0));
        if(newStandingChunk != currentStandingChunk)
        {
            var diff = newStandingChunk - currentStandingChunk;
            currentStandingChunk = newStandingChunk;
            RegenerateChunks(-diff);
        }

        if (Time.frameCount % 10 == 0 && createQueue.Count > 0)
        {
            while(true)
            {
                var toRender = createQueue.Dequeue();
                SetActiveItemSet(toRender.Item3, true);
                if (toRender.Item2 != null)
                {
                    bool itemSetExists = itemLoad.ContainsKey(toRender.Item3);
                    if (itemLoad.Count > maxEntries || itemSetExists)
                    {
                        toRender.Item2.GenerateTesselatedPlane(toRender.Item1, false);
                    }
                    else
                    {
                        itemLoad.Add(toRender.Item3,toRender.Item2.GenerateTesselatedPlane(toRender.Item1, true));
                    }
                    break;
                }
            }
        }
    }

    private void SetActiveItemSet(Vector2Int key,bool set)
    {
        if (!itemLoad.ContainsKey(key))
            return;
        foreach(var item in itemLoad[key])
        {
            item.SetActive(set);
        }
    }

    private void ShiftColumn(int from,int to)
    {
        for(int i = 0;i < renderedSize;i++)
        {
            managedChunks[to, i] = managedChunks[from, i];
            managedChunks[from, i] = null;
        }
    }

    private void ShiftRow(int from, int to)
    {
        for (int i = 0; i < renderedSize; i++)
        {
            managedChunks[i,to] = managedChunks[i,from];
            managedChunks[i, from] = null;
        }
    }

    private Vector2Int ManagedChunksToGrid(Vector2Int center, int x, int z)
    {
        return center + new Vector2Int(x - renderDistance, z - renderDistance);
    }

    private void RegenerateChunks(Vector2Int offsetDirection)
    {
        Vector2Int previousPos = currentStandingChunk + offsetDirection;
        if(offsetDirection.x == 1)
        {
            for (int i = 0; i < renderedSize; i++)
            {
                Destroy(managedChunks[renderedSize - 1, i]);
                SetActiveItemSet(ManagedChunksToGrid(previousPos,renderedSize - 1,i), false);
            }
            for (int i = renderedSize - 2; i >= 0; i--)
            {
                ShiftColumn(i, i + 1);
            }
        }
        else if(offsetDirection.x == -1)
        {
            for (int i = 0; i < renderedSize; i++)
            {
                Destroy(managedChunks[0, i]);
                SetActiveItemSet(ManagedChunksToGrid(previousPos,0, i), false);
            }
            for (int i = 1; i < renderedSize; i++)
            {
                ShiftColumn(i, i - 1);
            }
        }

        if (offsetDirection.y == 1)
        {
            for (int i = 0; i < renderedSize; i++)
            {
                Destroy(managedChunks[i,renderedSize - 1]);
                SetActiveItemSet(ManagedChunksToGrid(previousPos, i, renderedSize - 1), false);
            }
            for (int i = renderedSize - 2; i >= 0; i--)
            {
                ShiftRow(i, i + 1);
            }
        }
        else if (offsetDirection.y == -1)
        {
            for (int i = 0; i < renderedSize; i++)
            {
                Destroy(managedChunks[i, 0]);
                SetActiveItemSet(ManagedChunksToGrid(previousPos, i, 0), false);
            }
            for (int i = 1; i < renderedSize; i++)
            {
                ShiftRow(i, i - 1);
            }
        }
        //Regenerate missing chunks
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                if(managedChunks[x + renderDistance, z + renderDistance] == null)
                {
                    var newChunk = CreateChunk(currentStandingChunk + new Vector2Int(x, z));
                    managedChunks[x + renderDistance, z + renderDistance] = newChunk;
                }
            }
        }
        //Combine meshes in the center to avoid lines

    }

    private Queue<(Vector3 pos,MapGen,Vector2Int)> createQueue = new Queue<(Vector3 pos, MapGen,Vector2Int)>();

    private GameObject CreateChunk(Vector2Int gridPos)
    {
        var newChunk = Instantiate(mapChunkPrefab,transform);
        var mapgen = newChunk.GetComponent<MapGen>();
        mapgen.width = chunkWidth + 1;
        mapgen.height = chunkWidth + 1;
        mapgen.quadSize = vertSize;
        mapgen.itemGenSettings = itemSpawn;
        mapgen.noiseSettings = genData;
        Vector3 position = new Vector3(gridPos.x * chunkSize, 0, gridPos.y * chunkSize);
        createQueue.Enqueue((position, mapgen,gridPos));
        return newChunk;
    }

}
