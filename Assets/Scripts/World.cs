/**
 * Responsible for keeping a reference to all generated chunks and chunk meshes
 */

using System.Collections.Generic;
using UnityEngine;

public class World
{
    private Dictionary<Vector3Int, ChunkData> chunkList;
    public World()
    {
        chunkList = new Dictionary<Vector3Int, ChunkData>();
    }
    public void AddChunk(ChunkData chunkData, int x, int y, int z)
    {
        chunkList.Add(new Vector3Int(x,y,z) , chunkData); 
    }

    public ChunkData GetChunk(Vector3Int position)
    {
        if(chunkList.TryGetValue(position, out var chunkData))
        {
            return chunkData;
        } else
        {
            return null;
        }
    }
}