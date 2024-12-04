/**
 * Holds all data for a chunk including its world position, its blocks, and likely its neighboring chunks. Also 
 * uses Perlin Noise to generate terrain. Will likely switch to Simplex or Gradient Noise eventually.
 */

using UnityEngine;
using Icaria.Engine.Procedural;
using System;
public class ChunkData
{
    private Block[] blocks = new Block[Constants.CHUNK_SIZE * Constants.CHUNK_SIZE * Constants.CHUNK_SIZE];
    private int worldX;
    private int worldY;
    private int worldZ;
    public ChunkData (int worldX, int worldY, int worldZ) {
        this.worldX = worldX;
        this.worldY = worldY;
        this.worldZ = worldZ;
        GenerateChunkData();
    }

    
    private void GenerateChunkData()
    {
        float frequency = 0.03f;  // Controls the size of features
        float amplitude = 20f;  // Controls the maximum height variation
        int baseHeight = 50;    // Base height level for the terrain

        for (int x = 0; x < Constants.CHUNK_SIZE; x++)
        {
            for (int z = 0; z < Constants.CHUNK_SIZE; z++)
            {
                float worldXCoord = (worldX + x) * frequency;
                float worldZCoord = (worldZ + z) * frequency;

                float noiseValue = GenerateHeight(worldXCoord, worldZCoord);

                int height = Mathf.Clamp(Mathf.RoundToInt(baseHeight + noiseValue * amplitude), 0, 100);

                for (int y = 0; y < Constants.CHUNK_SIZE; y++)
                {
                    if (worldY + y == height)
                    {
                        blocks[GetIndex(x, y, z)] = Block.Grass;
                    }
                    else if (worldY + y < height)
                    {
                        blocks[GetIndex(x, y, z)] = Block.Dirt;
                    }
                    else
                    {
                        blocks[GetIndex(x, y, z)] = Block.Air;
                    }
                }
            }
        }
    }

    private float GenerateHeight(float x, float z)
    {
        float noise = Mathf.PerlinNoise(x, z);
        noise += 0.5f * Mathf.PerlinNoise(x * 2, z * 2);  
        noise += 0.25f * Mathf.PerlinNoise(x * 4, z * 4);

        return noise; // Returns a value in the range [0, 1.75]
    }

    public int GetIndex(int x, int y, int z){
        return (y * Constants.CHUNK_SIZE * Constants.CHUNK_SIZE) + (z * Constants.CHUNK_SIZE) + x;
    }

    public Block GetBlockAtPosition(int x, int y, int z){
        return blocks[GetIndex(x,y,z)];
    }

    public int GetWorldX(){
        return worldX;
    }
    public int GetWorldY(){
        return worldY;
    }
    public int GetWorldZ(){
        return worldZ;
    }
}