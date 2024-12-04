/**
 * Responsible for generating and removing chunks from the world as the player
 * moves throughout it.  
 */

using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    [SerializeField]
    private int worldWidth = 4;
    [SerializeField]
    private int worldHeight = 5;

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldWidth; z++)
            {
                for (int y = 2; y < 2 + worldHeight; y++)
                {
               
                    CreateChunk(x, y, z);
                }
            }
        }
    }

    void CreateChunk(int chunkX, int chunkY, int chunkZ)
    {
        GameObject chunkObject = Instantiate(chunkPrefab, new Vector3(chunkX * Constants.CHUNK_SIZE, chunkY * Constants.CHUNK_SIZE, chunkZ * Constants.CHUNK_SIZE), Quaternion.identity);

        ChunkData chunkData = new ChunkData(chunkX * Constants.CHUNK_SIZE, chunkY * Constants.CHUNK_SIZE, chunkZ * Constants.CHUNK_SIZE);
        ChunkMesh chunkMesh = chunkObject.GetComponent<ChunkMesh>();

        chunkMesh.Initialize(chunkData);
    }
}
