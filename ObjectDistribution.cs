using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectDistribution : MonoBehaviour {

    public GameObject terrainMap;
    public int distributeHouse = 16, distributeTown = 30;

    Mesh mesh;
    Vector3[] vertices;
    public List<Vector3> validVertices = new List<Vector3>();
  
    public GameObject house_P, village_P;

    public int houseCount = 0;
    public int villagesCount = 0;

    public  List<Vector3> houseList;
    public List<Vector3> villageList;

    private GameObject house_prefab;
    private GameObject village_prefab;

    // Use this for initialization
    void Start() {
        mesh = terrainMap.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;


    }

    // Update is called once per frame
    void Update()
    {
        //    Debug.DrawRay(vertices[0] * 20, Vector3.up * 300, Color.yellow); // the first point of mesh vertices
        for (int i = vertices.Length / 2 - 1000; i < Mathf.Round(vertices.Length / 1.25f) - 1; i++) // evaluate the vertices in the middle of mesh
        {
            if (vertices[i].y < 50 && vertices[i].x < 100 && vertices[i].x > -100)
            {
                if (!validVertices.Contains(vertices[i]))
                {
                    validVertices.Add(vertices[i]);
                }
            }

            // Debug.Log(validVertices.Count);
        }
        //---------------------------------------------------------------------------------------
        for (int i = 0; i < validVertices.Count; i++)
        {
            //  float dX = validVertices[i].x - validVertices[i + 1].x; // suppose the vertices is not the last one of each row
            //float dZ = validVertices[i].z - validVertices[i + 1].z;
            //Debug.DrawRay(validVertices[i] * 20, Vector3.up * 500, Color.yellow);
            if ((i +1)% distributeHouse == 0)  {
                if (!houseList.Contains(validVertices[i]))
                {
                    houseList.Add(validVertices[i]);
                }
                Debug.Log(houseList.Count);

                if (houseCount <= houseList.Count)
                {
                    SpawningHouse(house_P, validVertices[i], 0);
                  
                }

            }
            
            if ((i+1) % distributeTown == 0  &&(i+1)%distributeHouse!=0) {

                if (!villageList.Contains(validVertices[i]) )
                {
                    villageList.Add(validVertices[i]);
          
                }

                if (villagesCount <= villageList.Count)
                {
                    SpawningVillage(village_P, validVertices[i], 0);

                }

            }

        }

    }

    void SpawningHouse(GameObject g, Vector3 sp, float y)
    { 
        house_prefab =Instantiate(g, new Vector3(sp.x * 20, sp.y + 100 + y, sp.z * 20), Quaternion.identity);

        houseCount++;
    }
    void SpawningVillage(GameObject g, Vector3 sp, float y) {

        village_prefab = Instantiate(g, new Vector3(sp.x * 20, sp.y + 100 + y, sp.z * 20), Quaternion.identity);

        villagesCount++;
    }

}

       
    
    
   
    

       
  
