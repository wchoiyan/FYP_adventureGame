using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// get the texture of noisemap or colormap and apply to the material 

public class MapDisplay : MonoBehaviour {

    public Renderer textureRenderer; 
    public MeshFilter meshFilter;   // store the data of the mesh that we have created
    public MeshRenderer meshRenderer; // look at the meshfilter and render the mesh with mateials

    public void DrawTexture(Texture2D texture) {
        textureRenderer.sharedMaterial.mainTexture = texture; // apply the texture to material during edit time
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);  
    }
    public void DrawMesh(MeshData meshData, Texture2D texture) {
        meshFilter.sharedMesh = meshData.CreateMesh();
       
        meshRenderer.sharedMaterial.mainTexture = texture;

    }
}