
/**
 * Keeps track of vertices, indices, and uv coords for a chunk. Also generates chunk mesh
 * data from given chunk data.
 */

using UnityEngine;

public class ChunkMesh : MonoBehaviour
{
    private ChunkData chunkData;
    private World world;
    private MeshData meshData = new MeshData();
    private Mesh mesh;

    public void Initialize(ChunkData chunkData, World world)
    {
        this.chunkData = chunkData;
        this.world = world;
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        int chunkSize = Constants.CHUNK_SIZE;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    Block block = chunkData.GetBlockAtPosition(x, y, z);

                    if (block != Block.Air)
                    {
                        AddBlockMesh(x, y, z, block);
                    }
                }
            }
        }

        BuildMesh();
    }

    private void AddBlockMesh(int x, int y, int z, Block block)
    {
        Vector3 blockPosition = new Vector3(x, y, z);

        foreach (Vector3 normal in new[] { Vector3.up, Vector3.down, Vector3.right, Vector3.left, Vector3.forward, Vector3.back })
        {
            if (IsTransparent(x + (int)normal.x, y + (int)normal.y, z + (int)normal.z))
            {
                AddFace(blockPosition, normal, block);
            }
        }
    }

    private bool IsTransparent(int x, int y, int z)
    {
        if (x < 0 || x >= Constants.CHUNK_SIZE ||
            y < 0 || y >= Constants.CHUNK_SIZE ||
            z < 0 || z >= Constants.CHUNK_SIZE)
        {
            return GetNeighborBlock(x, y, z) == Block.Air;
        }

        return chunkData.GetBlockAtPosition(x, y, z) == Block.Air;
    }

    private Block GetNeighborBlock(int x, int y, int z)
    {
        Vector3Int neighborOffset = new Vector3Int(
            x < 0 ? -1 * Constants.CHUNK_SIZE : x >= Constants.CHUNK_SIZE ? Constants.CHUNK_SIZE : 0,
            y < 0 ? -1 * Constants.CHUNK_SIZE : y >= Constants.CHUNK_SIZE ? Constants.CHUNK_SIZE : 0,
            z < 0 ? -1 * Constants.CHUNK_SIZE : z >= Constants.CHUNK_SIZE ? Constants.CHUNK_SIZE : 0
        );

        if (neighborOffset == Vector3Int.zero)
            return Block.Air;

        Vector3Int neighborChunkPosition = new Vector3Int(
            chunkData.GetWorldX() + neighborOffset.x,
            chunkData.GetWorldY() + neighborOffset.y,
            chunkData.GetWorldZ() + neighborOffset.z
        );

        ChunkData neighborChunk = world.GetChunk(neighborChunkPosition);
        if (neighborChunk == null)
        {
            return Block.Air; // Assume air if neighbor chunk doesn't exist
        }

        Vector3Int neighborPosition = new Vector3Int(
            x < 0 ? Constants.CHUNK_SIZE - 1 : x % Constants.CHUNK_SIZE,
            y < 0 ? Constants.CHUNK_SIZE - 1 : y % Constants.CHUNK_SIZE,
            z < 0 ? Constants.CHUNK_SIZE - 1 : z % Constants.CHUNK_SIZE
        );

        return neighborChunk.GetBlockAtPosition(neighborPosition.x, neighborPosition.y, neighborPosition.z);
    }

    private void AddFace(Vector3 position, Vector3 normal, Block block)
    {
        Vector3[] vertices = FaceBuilder.GetFaceVertices(position, normal);
        int[] triangles = FaceBuilder.GetTriangleIndices(clockwise: normal == Vector3.up || normal == Vector3.right || normal == Vector3.forward);
        Vector2[] uvs = new Vector2[4];

        if (block == Block.Grass)
        {
            if (normal == Vector3.up)
            {
                uvs = TextureAtlas.GetUVs(0, 0, normal); // Grass top
            }
            else if (normal == Vector3.down)
            {
                uvs = TextureAtlas.GetUVs(2, 0, normal); // Dirt
            }
            else
            {
                uvs = TextureAtlas.GetUVs(1, 0, normal); // Grass side
            }
        }
        else if (block == Block.Dirt)
        {
            uvs = TextureAtlas.GetUVs(2, 0, normal); // Dirt
        }

        meshData.AddQuad(vertices, triangles, uvs);
    }

    private void BuildMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        mesh.Clear();
        mesh.vertices = meshData.Vertices.ToArray();
        mesh.triangles = meshData.Triangles.ToArray();
        mesh.uv = meshData.UVs.ToArray();
        mesh.RecalculateNormals();
    }
}
