﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap, ColorMap, MeshMap
    }
    public DrawMode drawMode;
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistence;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions; 

    public void GenerateMap()
    {
        float[,] noiseMap = noise.GenerateNoiseMap(mapHeight, mapWidth, seed, noiseScale, octaves, persistence, lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int z = 0; z < mapHeight; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currectHeight = noiseMap[x, z];
                foreach (TerrainType region in regions)
                {
                    if (currectHeight <= region.height)
                    {
                        colorMap[z * mapWidth + x] = region.color;
                        break;
                    }
                }
            }
        }


        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if(drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colorMap, mapWidth, mapHeight));
        }
        else if(drawMode == DrawMode.MeshMap)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.TextureFromColourMap(colorMap, mapWidth, mapHeight));
        }
    }

    void OnValidate()
    {
        if(mapWidth < 1)
        {
            mapWidth = 1;
        } 
        if(mapHeight < 1)
        {
            mapHeight = 1;
        }
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
        if(noiseScale < 0)
        {
            noiseScale = 0;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string label;
    public float height;
    public Color color;
}
