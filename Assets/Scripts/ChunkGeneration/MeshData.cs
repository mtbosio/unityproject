/** Responsible for holding chunk mesh information such as vertices, indices, uvs **/

using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
    public List<int> Triangles { get; private set; } = new List<int>();
    public List<Vector2> UVs { get; private set; } = new List<Vector2>();

    public void AddQuad(Vector3[] vertices, int[] triangleIndices, Vector2[] uvs)
    {
        int vertexIndex = Vertices.Count;

        Vertices.AddRange(vertices);
        UVs.AddRange(uvs);

        foreach (int index in triangleIndices)
        {
            Triangles.Add(vertexIndex + index);
        }
    }

    public void Clear()
    {
        Vertices.Clear();
        Triangles.Clear();
        UVs.Clear();
    }
}