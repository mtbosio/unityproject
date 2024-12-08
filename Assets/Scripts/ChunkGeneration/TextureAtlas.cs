

/**
 * Responsible for defining UVs for Chunk's texture
 */

using UnityEngine;

public static class TextureAtlas
{
    public static Vector2[] GetUVs(int atlasX, int atlasY, Vector3 normal)
    {
        float horizontalTexSize = 1.0f / Constants.HORIZONTAL_TEXTURE_SIZE;
        float verticalTexSize = 1.0f / Constants.VERTICAL_TEXTURE_SIZE;
        float xMin = atlasX * horizontalTexSize;
        float xMax = xMin + horizontalTexSize;
        float yMin = atlasY * verticalTexSize;
        float yMax = yMin + verticalTexSize;
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