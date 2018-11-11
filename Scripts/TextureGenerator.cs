using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//method to create a texture out of a 1D color map

public static class TextureGenerator  {
    public static Texture2D GenerateColourMap(Color[] colourMap, int width, int height) {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point; // fix the blurriness
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }
    public static Texture2D GenerateNoiseMap(float[,] noiseMap) { //grayscale mode
        int width = noiseMap.GetLength(0); // first dismension
        int height = noiseMap.GetLength(1); // second dismension

        Color[] colorMap = new Color[width * height]; // size of multipied by height  
        for (int y = 0; y < height; y++) { // 2d array to 1d array
            for (int x = 0; x < width; x++){
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]); // y* width ->  index of row we are currently on, + x --> the column
            }
        }
        return GenerateColourMap(colorMap, width, height); // using the same method that used in Generate Color Map to generate the noise map. 
    }
}
