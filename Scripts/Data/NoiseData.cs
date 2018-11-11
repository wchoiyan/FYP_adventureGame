using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()] // create a data file by right-click
public class NoiseData : UpdatableData{

    public float noiseScale;

    public int octaves;
    [Range(0, 1)] //  a slider of persistance
    public float persistance;
    public float lancunarity;

    public int seed;
    public Vector2 offset;

    protected override void OnValidate() 
    { // clamp the width and height of map at least 1  --> handy validate the map

        if (lancunarity < 1) {
            lancunarity = 1;
        }
        if (octaves < 0) {
            octaves = 0;
        }
        base.OnValidate();
    }
}


