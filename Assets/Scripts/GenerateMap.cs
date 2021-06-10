using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{

   public enum DrawMode{Noise, Color, Mesh};
   public DrawMode drawMode; 

   const int chunkSize = 241;
   [Range(0,6)]
   public int levelOfDetail;
   public float scale;
   public float SizeScale;
   public AnimationCurve meshCurve;

   public int octaves = 1;
   [Range(0,1)]
   public float persistence;
   public float lacunarity;

   public int seed;
   public Vector2 offset;

   public bool autoUpdate;

   public TerrainType[] regions;

   public void generateMap()
   {
       float[,] noiseMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, seed, scale, octaves, persistence, lacunarity, offset);

        Color[] colorMap = new Color[chunkSize * chunkSize];
        for(int y = 0; y < chunkSize; y++)
        {
            for(int x = 0; x < chunkSize; x++)
            {
                float currentHeight = noiseMap[x,y];
                for(int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].heightLevel)
                    {
                        colorMap[y * chunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

       Display display = FindObjectOfType<Display>();
       if(drawMode == DrawMode.Noise)
       {
           display.DrawTexture(TextureGenerator.textureFromHeightMap(noiseMap));
       }
       else if(drawMode == DrawMode.Color)
       {
           display.DrawTexture(TextureGenerator.textureFromColorMap(colorMap, chunkSize, chunkSize));
       }
       else if(drawMode == DrawMode.Mesh)
       {
           display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, SizeScale, meshCurve, levelOfDetail), TextureGenerator.textureFromColorMap(colorMap, chunkSize, chunkSize));
       }
   }

   void OnValidate()
   {
       if(scale <= 0)
       {
           scale = 0.0001f;
       }
       if(octaves < 0)
       {
           octaves = 0;
       }
   }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float heightLevel;
    public Color color;
}