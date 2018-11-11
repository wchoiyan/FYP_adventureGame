using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for generating noise map 

public static class Noise  {

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight,int seed,float noiseScale,int octaves, float persistance, float lacunarity, Vector2 offset)
        // persistance = amplitude(sin) 
        // lacunarity = frequency
        // octave = layer of the curve (persistance and lacunarity)
        // seed for pseudo random map generation
        
    {   
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed); // generate the noise map randomly depending on the seed ( pseudo random)

        Vector2[] octaveOffsets = new Vector2[octaves]; // the offset of x and Y 

        for (int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000,100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (noiseScale <= 0)
        {
            noiseScale = 0.0001f;
        }
        float maxNoiseHeight = -1*float.MaxValue; // normalize the noise height
        float minNoiseHeight =  -1*float.MinValue; // flip

        float halfWidth = mapWidth / 2; // get the middle  for zooming in  the center
        float halfHeight = mapHeight / 2;

        for (int y = 0; y  < mapHeight; y++) {  // loop through the noise map
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) // octave = layer of the curve (persistance and lacunarity) it needs more than two octaves.
                {
                    float sampleX = (x - halfWidth)/ noiseScale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight)/ noiseScale * frequency + octaveOffsets[i].y;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY)*2-1; // when perlin noise decreases to negative, noise height will be decreased and nicer too

                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxNoiseHeight)  { // extend the height
                    maxNoiseHeight = noiseHeight;
                }   else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
         }
        for(int y = 0; y < mapHeight; y++){
            for (int x = 0; x < mapWidth; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]); //return 0 and 1
            }
        }
        return noiseMap;
    }
	
}
