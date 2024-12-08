/**
 * Responsible for generating and removing chunks from the world as the player
 * moves throughout it.  
 */

using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public GameObject chunkPrefab;
    [SerializeField]
    private int worldWidth = 10;
    [SerializeField]
    private int worldHeight = 3;

    private World world;

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        world = new World();
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldWidth; z++)
            {
                for (int y = 2; y < 2 + worldHeight; y++)
                {
                    CreateChunkData(x, y, z);
                }
            }
        }
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldWidth; z++)
            {
                for (int y = 2; y < 2 + worldHeight; y++)
                {
                    CreateChunkMesh(x, y, z);
                }
            }
        }
    }

    void CreateChunkData(int chunkX, int chunkY, int chunkZ)
    {
        chunkX *= Constants.CHUNK_SIZE;
        chunkY *= Constants.CHUNK_SIZE;
        chunkZ *= Constants.CHUNK_SIZE;

        ChunkData chunkData = new ChunkData(chunkX, chunkY, chunkZ);

        world.AddChunk(chunkData, chunkX, chunkY, chunkZ);

        
    }
    void CreateChunkMesh(int chunkX, int chunkY, int chunkZ)
    {
        chunkX *= Constants.CHUNK_SIZE;
        chunkY *= Constants.CHUNK_SIZE;
        chunkZ *= Constants.CHUNK_SIZE;

        GameObject chunkObject = Instantiate(chunkPrefab, new Vector3(chunkX, chunkY, chunkZ), Quaternion.identity);

        ChunkData chunkData = world.GetChunk(new Vector3Int(chunkX, chunkY, chunkZ));  

        ChunkMesh chunkMesh = chunkObject.GetComponent<ChunkMesh>();

        chunkMesh.Initialize(chunkData, world);
    }
}
