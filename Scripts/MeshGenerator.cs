using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator  {

    public static MeshData GenerateTerrainMesh(float[,] heightmap , float heightMultiplier,AnimationCurve heightCurve , int levelOfDetail) {
        // Why change to MeshData? because game wont freeze up while generating chunks of our mesh
        // Unity only supports for return the mesh data instead of threading the new mesh
        int width = heightmap.GetLength(0);
        int height = heightmap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f; // no negative

        int meshSimplicationIncrement =(levelOfDetail==0)? 1:levelOfDetail*2; // to simplify the mesh by using LOD *2 ( factor 因數)
        int verticesPerLine = (width - 1) / meshSimplicationIncrement + 1; // the number of vertices after simplication

        MeshData meshData = new MeshData(verticesPerLine,verticesPerLine);
        int vertexIndex = 0; // find the current vertex

        for (int y = 0; y < height; y+= meshSimplicationIncrement) {
            for (int x = 0; x < width; x+=meshSimplicationIncrement )  {

                meshData.vertices[vertexIndex] = new Vector3( topLeftX + x,heightCurve.Evaluate( heightmap[x, y])*heightMultiplier, topLeftZ - y); // center to the screen
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                if ( x < width - 1 && y < height - 1){
                    meshData.AddTrianlge(vertexIndex , vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine );
                    meshData.AddTrianlge(vertexIndex + verticesPerLine +1, vertexIndex , vertexIndex +1);
                }
                vertexIndex++;

            }

        }
        return meshData;
    }
}

public class MeshData{ // for storing the mesh data
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs; // generate with a UV map to add textures to it

    int trianglesVertices; // current index and the total of index
    public MeshData(int meshWidth, int meshHeight) { // constructors
        vertices = new Vector3 [meshWidth*meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6]; // the length of triangle array
    }
    public void AddTrianlge(int a, int b, int c){  // add the total 3 triangle vertices 
        triangles[trianglesVertices] = a;
        triangles[trianglesVertices+1] = b;
        triangles[trianglesVertices + 2] = c;
        trianglesVertices += 3;
    }

    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals(); // lighting works out nicely
        return mesh;
    }
}