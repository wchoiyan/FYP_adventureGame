using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RockClimb : MonoBehaviour {
    //Vector3 startPosition;
    public Animator anim;
    Vector3 currentPosition;
    Vector3 targetPosition;

    public GameObject character;
    public Transform rayOrigin;

    public float movingSpeed = 5f;
    public float rotationSpeed = 5f;
    public float climbingSpeed = 3f;

    public bool isClimbing = false;
    bool isCountDown = false;
    bool isReadyForClimb = false;


    Ray ray;
    public float rayDistance = 15f;
    public float offsetFromWall = 0.5f;// how far away from the wall

    Quaternion RotationToWall;

    float t;
    public float waitForClimb = 1f;
    public ChildTrigger L_childTrigger;
    public ChildTrigger R_childTrigger;
    // Use this for initialization

    List<Vector3> detectionPoints = new List<Vector3>(); // store lots of point for the list

    void Start() {


    }
    // Update is called once per frame
    void Update()
    {
        float horizontal_value = Input.GetAxis("Horizontal") * Time.deltaTime * movingSpeed;
        float vertical_value = Input.GetAxis("Vertical") * Time.deltaTime * movingSpeed;

        character.transform.Translate(horizontal_value, vertical_value, 0);

        t += Time.deltaTime;

        if (isCountDown)
        {
            if (t > waitForClimb)
            {
                isReadyForClimb = true;
                t = 0;
            }
            else
            {
                isReadyForClimb = false;
            }
            isCountDown = false;
        }// Timer


        if (Input.GetMouseButton(0))
        {
            initTargetTheWall(); // first, detect the frontside of character
            isCountDown = true;

            if (isReadyForClimb &&isClimbing)
            {
                detectWall();
            }

        }
    }

    void initTargetTheWall() {

        //ray.origin = new Vector3(character.transform.position.x, character.transform.position.y , character.transform.position.z);
        ray.origin = character.transform.position;

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, character.transform.TransformDirection(Vector3.forward), out hit, rayDistance))
        {
            isClimbing = true;
            //   character.transform.position = Vector3.Lerp(currentPosition, targetPosition, 0.5f);
            GetClimbPosition(hit); // get the position for climbing
            Debug.DrawRay(ray.origin, character.transform.TransformDirection(Vector3.forward) * rayDistance, Color.yellow);
        }
        else
        {
            isClimbing = false;
            Debug.DrawRay(ray.origin, character.transform.TransformDirection(Vector3.forward) * rayDistance, Color.red);
        }
    }

    void detectWall()
    {
        RaycastHit l_raycastHit, r_raycastHit, v_raycastHit;
        Vector3 l = character.transform.TransformDirection(Vector3.left);
        Vector3 r = character.transform.TransformDirection(Vector3.right);
        Vector3 v = character.transform.TransformDirection(Vector3.up);

        Debug.DrawRay(rayOrigin.transform.position, l * rayDistance, Color.green);
        Debug.DrawRay(rayOrigin.transform.position, r * rayDistance, Color.green);
        Debug.DrawRay(rayOrigin.transform.position, v * rayDistance, Color.green);

        if (Physics.Raycast(rayOrigin.transform.position, l, out l_raycastHit, rayDistance))// left 
        {
            Debug.Log(l_raycastHit.collider.name);
            GetClimbPosition(l_raycastHit);
        }
        if (Physics.Raycast(rayOrigin.transform.position, r, out r_raycastHit, rayDistance))// right
        {
            Debug.Log(r_raycastHit.collider.name);
            GetClimbPosition(r_raycastHit);
        }
        if (Physics.Raycast(rayOrigin.transform.position, v, out v_raycastHit, rayDistance)) //  vertical
        {
            Debug.Log(v_raycastHit.collider.name);
        }

    }
    float RotateAngle( Vector3 wallNormalPosition) {

        float angle = Vector3.Angle(character.transform.position, wallNormalPosition);
        return angle;

    }
    
    void GetClimbPosition( RaycastHit hit) {

        currentPosition = character.transform.position;
        anim.CrossFadeInFixedTime("climb_idle", 0.2f);
        targetPosition = hit.point + (hit.normal * offsetFromWall); // adjusting the distance between the normal surface of   

        Vector3 tp = Vector3.Lerp(currentPosition,targetPosition, 0.5f);
         character.transform.position = tp;

        float angle = RotateAngle(- hit.normal); // return the angle for rotating to the wall
        Debug.Log(angle);

        character.transform.rotation = Quaternion.LookRotation(-hit.normal);
        

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
