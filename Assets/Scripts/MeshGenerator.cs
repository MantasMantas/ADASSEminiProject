using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightScale, AnimationCurve heightCurve, int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int simplificationIncrement = (levelOfDetail == 0) ? 1: levelOfDetail * 2;
        int verticesPerLine = (width-1) / simplificationIncrement + 1;
        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int index = 0;

         for(int y = 0; y < height; y+= simplificationIncrement)
        {
            for(int x = 0; x < width; x+= simplificationIncrement)
            {

                meshData.verticles[index] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x,y]) * heightScale, topLeftZ - y);
                meshData.uvs[index] = new Vector2(x / (float)width, y / (float)height);

                if(x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(index, index + verticesPerLine + 1, index + verticesPerLine);
                    meshData.AddTriangle(index + verticesPerLine + 1, index, index + 1);
                }

                index++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Vector3[] verticles;
    public int[] triangles;
    public Vector2[] uvs;

    int index;

    public MeshData(int width, int height)
    {
        verticles = new Vector3[width * height];
        uvs = new Vector2[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[index] = a;
        triangles[index + 1] = b;
        triangles[index + 2] = c;

        index += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticles;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;

    }
}
