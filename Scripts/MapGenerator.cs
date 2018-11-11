using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode { Noisemap, ColourMap, Mesh}; // determine the drawing mode and the expected output
    public DrawMode drawMode;

    public TerrainData terrainData;
     public NoiseData noiseData;
    public TextureData textureData;

    public Material terrainMaterial;

    const int mapChunkSize = 241; // the size of the mesh ( square size)
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves; 
    [Range(0,1)] //  a slider of persistance
    public float persistance;
    public float lancunarity;

    public int seed;
    public Vector2 offset; 

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainTypes[] regions;

 

    public float minHeight; // minimum height of terrain
    public float maxHeight; // maximum height of terrain


    void OnMapUpdated() {
        if (!Application.isPlaying) {
            GenerateMap();
        }
    }
    void OnValidate() {
        if (terrainData != null) {
        
            terrainData.OnValueUpdated += OnMapUpdated;
        }
        if (noiseData != null){
        
            noiseData.OnValueUpdated += OnMapUpdated;
        }
        if (textureData != null) {
            textureData.OnValueUpdated += OnMapUpdated;
        }
    }
    
    public void GenerateMap()
    { // (拿取)fetching the 2D noisemap from noise class

        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lancunarity, noiseData.offset);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y]; // height at this point = noisemap x by y
                for (int i = 0; i < regions.Length; i++)
                {// loop the paramter of  the all array of TerrainTypes[] regions
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour; // save the color of each pixel of noise map
                        break;
                    }
                }
            }
        }
        
        textureData.UpdateMeshHeights(terrainMaterial, minHeight, maxHeight);
  
        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (drawMode == DrawMode.Noisemap)
        {
            display.DrawTexture(TextureGenerator.GenerateNoiseMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.GenerateColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, levelOfDetail), TextureGenerator.GenerateColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
    }
}

[System.Serializable]
public struct TerrainTypes {
    public string name;
    public float height;
    public Color colour;
}
