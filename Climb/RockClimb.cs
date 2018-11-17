using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockClimb : MonoBehaviour {
    //Vector3 startPosition;
    public Animator anim;
    Vector3 currentPosition;
    Vector3 targetPosition;

    public GameObject character;
    public Transform horizontalOrigin, verticalOrigin, zOrigin;


    public float rotationSpeed = 5f;
    public float climbingSpeed = 3f;

    public bool isClimbing = false;
    bool isDetected = false; // to anaylze the wall is detected or not
    bool isInPosition = false;


    Ray ray;
    public float rayDistance = 15f;
    public float offsetFromWall = 0.5f;// how far away from the wall

    float t;
    Quaternion RotationToWall;

    public ChildTrigger L_childTrigger;
    public ChildTrigger R_childTrigger;
    // Use this for initialization

    void Start() {

    
    }
    // Update is called once per frame
    void Update()
    {
     

        targetVerticalWall();
        
            if (Input.GetMouseButton(0))
            {
            initTargetTheWall();
        }
            if (Input.GetKey(KeyCode.W))
            {
                ClimbUp();

            }
            if (Input.GetKey(KeyCode.A))
            {
            targetHorizontalWall();

        }
    }
    

    void initTargetTheWall() {

        //ray.origin = new Vector3(character.transform.position.x, character.transform.position.y , character.transform.position.z);
        ray.origin = character.transform.position;

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, character.transform.TransformDirection(Vector3.forward), out hit, rayDistance))
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
        //   character.transform.position = Vector3.Lerp(currentPosition, targetPosition, 0.5f);
            isDetected = true;
            GetClimbPosition(hit); // get the position for climbing
            Debug.DrawRay(ray.origin, character.transform.TransformDirection(Vector3.back) * rayDistance , Color.yellow);
        }
        else
        {
            isDetected = false;

            Debug.DrawRay(ray.origin, character.transform.TransformDirection(Vector3.back)* rayDistance , Color.red);
        }
    }

    void targetHorizontalWall() {
        Vector3 h = horizontalOrigin.transform.position;
  
        RaycastHit h_raycastHit;
        if (Physics.Raycast(h, character.transform.TransformDirection(Vector3.right), out h_raycastHit, rayDistance * 2))  { // horizontal line
            Debug.Log(h_raycastHit.collider.name);
            GetClimbPosition(h_raycastHit);
        }
        Debug.DrawRay(h, character.transform.TransformDirection(Vector3.right)* rayDistance * 2, Color.green);

    }


    void targetVerticalWall() {
        Vector3 v = verticalOrigin.transform.position;
        RaycastHit v_raycastHit;
        Debug.DrawRay(v, character.transform.TransformDirection(Vector3.down) * rayDistance * 2, Color.red);

        if (Physics.Raycast(v, character.transform.TransformDirection(Vector3.down), out v_raycastHit, rayDistance * 2))  { // vertical line
            Debug.DrawRay(v, v_raycastHit.normal, Color.yellow);
            
        }
    }

    void GetClimbPosition( RaycastHit hit) {
        currentPosition = character.transform.position;
        anim.CrossFadeInFixedTime("climb_idle", 0.2f);
        targetPosition = hit.point + (hit.normal * offsetFromWall); // adjusting the distance between the normal surface of   

        Vector3 tp = Vector3.Lerp(currentPosition,targetPosition, 0.5f);
         character.transform.position = tp;

    }
    void detectWall() {


    }
    

    void ClimbUp()
    {
        
        if (L_childTrigger.isDetected == true || R_childTrigger.isDetected == true)
        {

            character.transform.position = new Vector3(character.transform.position.x,
                                                                                             character.transform.position.y + 1 ,
                                                                                            character.transform.position.z);
           anim.Play("climb_up");
        }
        

    }

}
