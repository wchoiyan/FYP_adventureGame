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


    bool isCountDown = false;
    bool isReadyForClimb = false;

    public bool isInitialClimb = false; // already climb the first step


    Ray ray;
    public float rayDistance = 15f;
    public float offsetFromWall = 0.5f;// how far away from the wall

    Quaternion RotationToWall;

    float t;
    public float waitForClimb = 1f;
    public ChildTrigger L_childTrigger;
    public ChildTrigger R_childTrigger;
    // Use this for initialization

  public Dictionary<Vector3,RaycastHit> detectionPoints = new Dictionary<Vector3, RaycastHit>(); // store lots of point for the list
    Vector3[] checkpoints;
    RaycastHit[] checkHits;

    Vector3 theClosestPoint;
    RaycastHit theClosestHit;

    void Start() {


    }
    // Update is called once per frame
    void FixedUpdate()
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

        if (Input.GetMouseButton(0)) {

            if (isInitialClimb)
            {
                detectWall();
                GetContinuousClimbPosition();
                 Debug.Log(theClosestPoint);
            }
            else {

                initTargetTheWall(); // first, detect the frontside of character

            }

        }

    }

    void initTargetTheWall() {

        //ray.origin = new Vector3(character.transform.position.x, character.transform.position.y , character.transform.position.z);
        ray.origin = character.transform.position;

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, character.transform.TransformDirection(Vector3.forward), out hit, rayDistance))
        {

            // character.transform.position = Vector3.Lerp(currentPosition, targetPosition, 0.5f);
            GetInitClimbPosition(hit); // get the position for climbing
            Debug.DrawRay(ray.origin, character.transform.TransformDirection(Vector3.forward) * rayDistance, Color.yellow);
        }
        else
        {
            isInitialClimb = false;
            Debug.DrawRay(ray.origin, character.transform.TransformDirection(Vector3.forward) * rayDistance, Color.red);
        }
    }
    void detectWall() // detect the hit point (left, right , vertical 
    {
        
        detectionPoints = new Dictionary<Vector3, RaycastHit>(); // reset the list
        RaycastHit f_raycastHit,l_raycastHit, r_raycastHit, u_raycastHit;

        Vector3 f = character.transform.TransformDirection(Vector3.forward);
        Vector3 l = character.transform.TransformDirection(Vector3.left);
        Vector3 r = character.transform.TransformDirection(Vector3.right);
        Vector3 u = character.transform.TransformDirection(Vector3.up);

        Debug.DrawRay(rayOrigin.transform.position, f * rayDistance, Color.green);
        Debug.DrawRay(rayOrigin.transform.position, l * rayDistance, Color.green);
        Debug.DrawRay(rayOrigin.transform.position, r * rayDistance, Color.green);
        Debug.DrawRay(rayOrigin.transform.position, u * rayDistance, Color.green);

        if (Physics.Raycast(rayOrigin.transform.position, f, out f_raycastHit, rayDistance))// left 
        {
            //Debug.Log(l_raycastHit.collider.name);
            detectionPoints.Add(f_raycastHit.point, f_raycastHit);
        }
        else if (Physics.Raycast(rayOrigin.transform.position, l, out l_raycastHit, rayDistance))// left 
        {
            //Debug.Log(l_raycastHit.collider.name);
            detectionPoints.Add(l_raycastHit.point, l_raycastHit);
        }
        else if (Physics.Raycast(rayOrigin.transform.position, r, out r_raycastHit, rayDistance))// right
        {
            detectionPoints.Add(r_raycastHit.point, r_raycastHit);
        }
       else if (Physics.Raycast(rayOrigin.transform.position, u, out u_raycastHit, rayDistance)) // up
        {
            detectionPoints.Add(u_raycastHit.point, u_raycastHit);
        }
        storePoint();
    }

    void storePoint() {
        if (detectionPoints.Keys.Count < 1)
            return;
         checkpoints = new Vector3[detectionPoints.Keys.Count]; 
        checkHits = new RaycastHit[detectionPoints.Values.Count];
        detectionPoints.Keys.CopyTo(checkpoints, 0);// convert dictionary to array (Vector3)
        detectionPoints.Values.CopyTo(checkHits, 0); // convert dictionary to array ( raycasthit)
        theClosestPoint = checkpoints[0];
        theClosestHit = checkHits[0];
        // Debug.Log(checkpoints.Length);
      if(checkpoints.Length>1) // when detect 2 points , then compare
       FindTheClosetPoint();
    }

   void FindTheClosetPoint( ) {
        Vector3 c = character.transform.position;
        for (int i = 0; i < checkpoints.Length; i++)
        {
            Vector3 cpt = checkpoints[0] - c; // suppose the first vector is the nearest distance between the character
            Vector3 pts = checkpoints[i] - c;  //  another vector
            float closestValue = cpt.magnitude; 
            float anotherValue = pts.magnitude;
            if (closestValue > anotherValue) // when the closest vector is larger than another point (detectionPoint to character)
            {
                theClosestPoint = checkpoints[i]; // then the closest is another vector
                theClosestHit = checkHits[i];   
            }
            Debug.Log("closest"+closestValue);
            Debug.Log("another" + anotherValue);

        }

    }
    float RotateAngle( Vector3 wallNormalPosition) {

        float angle = Vector3.Angle(character.transform.position, wallNormalPosition);
        return angle;

    }
    void GetInitClimbPosition(RaycastHit initHit) {

        anim.CrossFadeInFixedTime("climb_idle", 0.2f);
        targetPosition = initHit.point + initHit.normal * offsetFromWall;
        Vector3 tp = Vector3.Lerp(character.transform.position, targetPosition, 1f);
        character.transform.position = tp;
        float angle = RotateAngle(-initHit.normal); // return the angle for rotating to the wall
        character.transform.rotation = Quaternion.LookRotation(-initHit.normal);
        isInitialClimb = true;


    }
    
    void GetContinuousClimbPosition() {

            anim.CrossFadeInFixedTime("climb_idle", 0.2f);
            Vector3 t;
            t = theClosestHit.point+ theClosestHit.normal * offsetFromWall;

            Vector3 tp= Vector3.Lerp(character.transform.position, t, 0.5f);
            character.transform.position = t;
            float angle = RotateAngle(-theClosestHit.normal); // return the angle for rotating to the wall
            character.transform.rotation = Quaternion.LookRotation(-theClosestHit.normal);
       // Debug.Log(angle);

    }

  
    void ClimbUp()
    {
        
        if (L_childTrigger.isDetected == true || R_childTrigger.isDetected == true)
        {

            character.transform.position = new Vector3(character.transform.position.x,
                                                                                             character.transform.position.y + 1 ,
                                                                                            character.transform.position.z);
            anim.CrossFadeInFixedTime("climb_up", 0.2f);
        }
        

    }

}
