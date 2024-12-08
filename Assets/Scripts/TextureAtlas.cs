

/**
 * Responsible for defining UVs for Chunk's texture
 */

using UnityEngine;

public static class TextureAtlas
{
    public static Vector2[] GetUVs(int atlasX, int atlasY, Vector3 normal)
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
}