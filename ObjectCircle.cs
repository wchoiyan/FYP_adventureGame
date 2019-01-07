using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ObjectCircle : MonoBehaviour {

    public GameObject terrainMap;

    Mesh mesh;
    Vector3[] vertices;
    public Vector3[] circleVertices;

    public List<Vector3> validVertices = new List<Vector3>();
    public List<Vector3> houseList;

    public int vertexCount = 48;
    public float lineWidth = 0.2f;
    public float radius;
    public int size = 10;

    private LineRenderer lineRenderer;

    public GameObject house_P;
    public int houseCount;



    // Use this for initialization
    void Start() {
        mesh = terrainMap.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(vertexCount + 1);
       // circleVertices = new Vector3[lineRenderer.positionCount];

    }

    // Update is called once per frame
    void Update() {
        CreateCircle();

        for (int i = 0; i < vertices.Length; i++) {

            if (vertices[i].y < 40 && vertices[i].x < 100 && vertices[i].x > -100) {

                if (!validVertices.Contains(vertices[i]))
                {
                    validVertices.Add(vertices[i]);
                }

            }
        }
        /*
        for (int i = 0; i < lineRenderer.positionCount; i++) { // for copying the vertex position of line renderer
            {
                circleVertices[i] = lineRenderer.GetPosition(i);
            }
        }*/
        for (int i = 0; i < validVertices.Count; i++) {
            float myX = validVertices[i].x;
            float myZ = validVertices[i].z;

            float myRadius = Mathf.Sqrt((myX * myX) + (myZ * myZ));
            Debug.Log(myRadius);
            if (myRadius < radius ) {
                if (!houseList.Contains(validVertices[i]))
                {
                    houseList.Add(validVertices[i]);
                }
                if (houseCount <= houseList.Count)
                {
                    SpawningHouse(house_P, validVertices[i], 0);
                }
            }
  
}
          
        

    }
  void CreateCircle()
    {
        float x, y, z;
        float angle = 20;

        for (int i = 0; i <( vertexCount+1) ; i++) {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x*size, 300, z*size));

            angle += (360f / vertexCount); 
        }
    }
    void SpawningHouse(GameObject g, Vector3 sp, float y) {

        Instantiate(g, new Vector3(sp.x * 20, sp.y + 100 + y, sp.z * 20), Quaternion.identity);

        houseCount++;

    }

}

