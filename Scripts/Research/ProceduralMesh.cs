using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralMesh : MonoBehaviour {

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;

    }

	// Use this for initialization
	void Update () {
        MakeMeshData();
        CreateMesh();
	}

    // Update is called once per frame
    void MakeMeshData() {
        // create an array of vertices 
        vertices = new Vector3[] { new Vector3(0,YValue.ins.yValue,0), new Vector3(0,0,1),
                                                      new Vector3(1,0,0), new Vector3 ( 1,0,1)};

        // create an array of triangles
        triangles = new int[] { 0, 1, 2 ,2,1,3 };
        
    }
    void CreateMesh() {
        mesh.Clear(); // clear out the history
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
}
}
