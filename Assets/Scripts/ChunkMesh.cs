/**
 * Keeps track of vertices, indices, and uv coords for a chunk. Also generates chunk mesh
 * data from given chunk data.
 */

using System.Collections.Generic;
using UnityEngine;

public class ChunkMesh : MonoBehaviour
{
    private ChunkData chunkData;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    private Mesh mesh;

    public void Initialize(ChunkData chunkData)
    {
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

        // Add UVs for texture mapping. These are dummys for now
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(0, 1));
    }


    private bool IsTransparent(int x, int y, int z)
    {
        
        if (x < 0 || x >= Constants.CHUNK_SIZE || y < 0 || y >= Constants.CHUNK_SIZE || z < 0 || z >= Constants.CHUNK_SIZE)
            return false;

        return !chunkData.GetBlockAtPosition(x, y, z).IsSolid;
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
