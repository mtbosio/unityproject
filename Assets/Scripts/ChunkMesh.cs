
/**
 * Keeps track of vertices, indices, and uv coords for a chunk. Also generates chunk mesh
 * data from given chunk data.
 */

using System.Collections.Generic;
using UnityEngine;

public class ChunkMesh : MonoBehaviour
{
    private ChunkData chunkData;
    private World world;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    private Mesh mesh;

    public void Initialize(ChunkData chunkData, World world)
    {
        this.world = world;
        this.chunkData = chunkData;
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

        if (IsTransparent(x, y + 1, z)) AddFace(blockPosition, Vector3.up); // Top
        if (IsTransparent(x, y - 1, z)) AddFace(blockPosition, Vector3.down); // Bottom
        if (IsTransparent(x + 1, y, z)) AddFace(blockPosition, Vector3.right); // Right
        if (IsTransparent(x - 1, y, z)) AddFace(blockPosition, Vector3.left); // Left
        if (IsTransparent(x, y, z + 1)) AddFace(blockPosition, Vector3.forward); // Front
        if (IsTransparent(x, y, z - 1)) AddFace(blockPosition, Vector3.back); // Back
    }

    private void AddFace(Vector3 position, Vector3 normal)
    {
        int vertexIndex = vertices.Count;

        Vector3[] faceVertices = new Vector3[4];

        if (normal == Vector3.up) // Top face
        {
            faceVertices[0] = position + new Vector3(0, 1, 0); // Bottom-left
            faceVertices[1] = position + new Vector3(0, 1, 1); // Top-left
            faceVertices[2] = position + new Vector3(1, 1, 1); // Top-right
            faceVertices[3] = position + new Vector3(1, 1, 0); // Bottom-right
        }
        else if (normal == Vector3.down) // Bottom face
        {
            faceVertices[0] = position + new Vector3(0, 0, 0); // Bottom-left
            faceVertices[3] = position + new Vector3(1, 0, 0); // Bottom-right
            faceVertices[2] = position + new Vector3(1, 0, 1); // Top-right
            faceVertices[1] = position + new Vector3(0, 0, 1); // Top-left
        }
        else if (normal == Vector3.right) // Right face
        {
            faceVertices[0] = position + new Vector3(1, 0, 0); // Bottom-left
            faceVertices[1] = position + new Vector3(1, 1, 0); // Top-left
            faceVertices[2] = position + new Vector3(1, 1, 1); // Top-right
            faceVertices[3] = position + new Vector3(1, 0, 1); // Bottom-right
        }
        else if (normal == Vector3.left) // Left face
        {
            faceVertices[0] = position + new Vector3(0, 0, 0); // Bottom-right
            faceVertices[1] = position + new Vector3(0, 1, 0); // Top-right
            faceVertices[2] = position + new Vector3(0, 1, 1); // Top-left
            faceVertices[3] = position + new Vector3(0, 0, 1); // Bottom-left
        }
        else if (normal == Vector3.forward) // Front face
        {
            faceVertices[0] = position + new Vector3(0, 0, 1); // Bottom-left
            faceVertices[1] = position + new Vector3(1, 0, 1); // Bottom-right
            faceVertices[2] = position + new Vector3(1, 1, 1); // Top-right
            faceVertices[3] = position + new Vector3(0, 1, 1); // Top-left
        }
        else if (normal == Vector3.back) // Back face
        {
            faceVertices[0] = position + new Vector3(0, 0, 0); // Bottom-right
            faceVertices[1] = position + new Vector3(1, 0, 0); // Bottom-left
            faceVertices[2] = position + new Vector3(1, 1, 0); // Top-left
            faceVertices[3] = position + new Vector3(0, 1, 0); // Top-right
        }

        vertices.AddRange(faceVertices);

        // Add triangles 
        if (normal == Vector3.up || normal == Vector3.right || normal == Vector3.forward)
        {
            // Clockwise
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
        }
        else
        {
            // Counter-clockwise
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 3);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 1);
        }

        // Texture Mapping
        Block block = chunkData.GetBlockAtPosition((int)position.x, (int)position.y, (int)position.z);
        Vector2[] faceUVs = GetUVsForBlockFace(block, normal);
        uvs.AddRange(faceUVs);
    }

    private Vector2[] GetUVsForBlockFace(Block block, Vector3 normal)
    {
        Vector2[] uvs = new Vector2[4];

        // Define UV coordinates based on the block type and face normal
        if (block == Block.Grass)
        {
            if (normal == Vector3.up)
            {
                uvs = GetUVsFromAtlas(0, 0, normal); // Grass top
            }
            else if (normal == Vector3.down)
            {
                uvs = GetUVsFromAtlas(2, 0, normal); // Dirt
            }
            else
            {
                uvs = GetUVsFromAtlas(1, 0, normal); // Grass side
            }
        }
        else if (block == Block.Dirt)
        {
            uvs = GetUVsFromAtlas(2, 0, normal); // Dirt
        }


        return uvs;
    }

    private Vector2[] GetUVsFromAtlas(int atlasX, int atlasY, Vector3 normal)
    {
        float texSize = 1.0f / Constants.TEXTURE_SIZE;
        float xMin = atlasX * texSize;
        float xMax = xMin + texSize;
        float yMin = 0;
        float yMax = 1;

        if (normal == Vector3.left || normal == Vector3.right)
        {
            return new Vector2[]
            {
            new Vector2(xMin, yMin), // Bottom-left
            new Vector2(xMin, yMax), // Top-left
            new Vector2(xMax, yMax), // Top-right
            new Vector2(xMax, yMin)  // Bottom-right
            };
        }
        else if (normal == Vector3.forward || normal == Vector3.back)
        {
            return new Vector2[]
            {
            new Vector2(xMin, yMin), // Bottom-left
            new Vector2(xMax, yMin), // Bottom-right
            new Vector2(xMax, yMax), // Top-right
            new Vector2(xMin, yMax)  // Top-left
            };
        }
        else if (normal == Vector3.up || normal == Vector3.down)
        {
            return new Vector2[]
            {
            new Vector2(xMin, yMin), // Bottom-left
            new Vector2(xMax, yMin), // Bottom-right
            new Vector2(xMax, yMax), // Top-right
            new Vector2(xMin, yMax)  // Top-left
            };
        }

        return new Vector2[]
        {
        new Vector2(xMin, yMin), // Bottom-left
        new Vector2(xMax, yMin), // Bottom-right
        new Vector2(xMax, yMax), // Top-right
        new Vector2(xMin, yMax)  // Top-left
        };
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

    private void BuildMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }
}