using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[width, height];

        System.Random rnd = new System.Random(seed);
        Vector2[] offsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++)
        {
            float offsetX = rnd.Next(-10000, 10000) + offset.x;
            float offsetY = rnd.Next(-10000, 10000) + offset.y;
            offsets[i] = new Vector2(offsetX, offsetY);
        }
        float max = float.MinValue;
        float min = float.MaxValue;

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {

                float amplutide = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - width/2f) / scale * frequency + offsets[i].x;
                    float sampleY = (y - height/2f) / scale * frequency + offsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += perlinValue * amplutide;
                    amplutide *= persistence;
                    frequency *= lacunarity;
                }

                if(noiseHeight > max)
                {
                    max = noiseHeight;
                }
                else if(noiseHeight < min)
                {
                    min = noiseHeight;
                }

                noiseMap[x,y] = noiseHeight;
            }
        }

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                noiseMap[x,y] = Mathf.InverseLerp(min, max, noiseMap[x,y]);
            }
            
        }

        return noiseMap;
    }
}
