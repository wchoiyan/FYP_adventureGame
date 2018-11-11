using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TextureData : UpdatableData {

    public Color[] baseColors;
    [Range(0, 1)]
    public float[] baseStartHeights;  // determine the starting height for each color  
    [Range(0, 1)]
    public float[] baseBlends; // blending between different layers


    public void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
    {    
        material.SetInt("baseColorCount", baseColors.Length);
        material.SetColorArray("baseColors", baseColors);
        material.SetFloatArray("baseStartHeights", baseStartHeights);
        material.SetFloatArray("baseBlends", baseBlends);

        material.SetFloat("minHeight", minHeight); // set values of standard shader in materials
        material.SetFloat("maxHeight", maxHeight);

    }
}
