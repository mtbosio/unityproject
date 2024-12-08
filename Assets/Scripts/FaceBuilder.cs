/** 
 * Responsible for 'building' a cube side face. */

using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public static class FaceBuilder
{
    public static Vector3[] GetFaceVertices(Vector3 position, Vector3 normal)
    {
        // Generate vertices for a face based on its normal
        if (normal == Vector3.up) return new Vector3[]
        {
            position + new Vector3(0, 1, 0), // Bottom-left
            position + new Vector3(0, 1, 1), // Top-left
            position + new Vector3(1, 1, 1), // Top-right
            position + new Vector3(1, 1, 0)  // Bottom-right
        };
        else if (normal == Vector3.down) return new Vector3[]
        {
            position + new Vector3(0, 1, 0), // Bottom-left
            position + new Vector3(0, 1, 1), // Top-left
            position + new Vector3(1, 1, 1), // Top-right
            position + new Vector3(1, 1, 0)  // Bottom-right
        };
        else if (normal == Vector3.down) return new Vector3[]
        {
            position + new Vector3(0, 0, 0), // Bottom-left
            position + new Vector3(1, 0, 0), // Bottom-right
            position + new Vector3(1, 0, 1), // Top-right
            position + new Vector3(0, 0, 1) // Top-left
    };
        else if (normal == Vector3.right) return new Vector3[]
        {
            position + new Vector3(1, 0, 0), // Bottom-left
            position + new Vector3(1, 1, 0), // Top-left
            position + new Vector3(1, 1, 1), // Top-right
            position + new Vector3(1, 0, 1) // Bottom-right
    };
        else if (normal == Vector3.left) return new Vector3[]
        {
            position + new Vector3(0, 0, 0), // Bottom-right
            position + new Vector3(0, 1, 0), // Top-right
            position + new Vector3(0, 1, 1), // Top-left
            position + new Vector3(0, 0, 1) // Bottom-left
        };
        else if (normal == Vector3.forward) return new Vector3[]
        {
            position + new Vector3(0, 0, 1), // Bottom-left
            position + new Vector3(1, 0, 1), // Bottom-right
            position + new Vector3(1, 1, 1), // Top-right
            position + new Vector3(0, 1, 1) // Top-left
    };
        else if (normal == Vector3.back) return new Vector3[]
        {
            position + new Vector3(0, 0, 0), // Bottom-right
            position + new Vector3(1, 0, 0), // Bottom-left
            position + new Vector3(1, 1, 0), // Top-left
            position + new Vector3(0, 1, 0) // Top-right
    };

        // Add other face definitions here...
        return new Vector3[0];
    }
  
    public static int[] GetTriangleIndices(bool clockwise)
    {
        if (clockwise) return new int[] { 0, 1, 2, 0, 2, 3 };
        return new int[] { 0, 3, 2, 0, 2, 1 };
    }


}
