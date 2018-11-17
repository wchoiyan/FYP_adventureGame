using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace climbing
{

    public class FreeClimb : MonoBehaviour
    {
        public Animator anim;
        public bool isClimbing;
        bool isLerping;
        bool inPosition;

        float t;
        Vector3 startPosition;
        Vector3 targetPosition;
        Quaternion startRot;
        Quaternion targetRot;
        public float positionOffset;
        public float offsetFromWall = 0.3f; // how far away from the wall
        public float speed = 0.2f;

        Transform helper;
        float delta;
  

        void Start() {
            Init();
        }
     public  void Init()
        {
            helper = new GameObject().transform;
            helper.name = "climb_helper";
            CheckForClimb();
        }

        void Update() {

            delta = Time.deltaTime;
            Tick(delta);
        }
        public void CheckForClimb() {

            Vector3 origin = transform.position;
            origin.y += 1.4f;
            Vector3 dir = transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, 10)) { // the raycast hit the collider( normal face)
                InitForClimb(hit);
              
            }

        }
        void InitForClimb(RaycastHit hit) {
            isClimbing = true;
            helper.transform.rotation = Quaternion.LookRotation(-hit.normal);
            startPosition = transform.position;
            targetPosition = hit.point + (hit.normal * offsetFromWall);
            t = 0;
            inPosition = false;
            anim.CrossFade("climb_up",2);
        
        }
        public void Tick(float delta) {
            if (!inPosition) { // not the right place to climb
                GetInPosition();
                return;
            }
        }
        void GetInPosition()
        {
            t += delta;
            if (t > 1)
            {
                t = 1;
                inPosition = true;

                //enable the ik 
            }
            Vector3 tp = Vector3.Lerp(startPosition, targetPosition, t);
            transform.position = tp;
        }
        Vector3 PosWithOffset(Vector3 origin, Vector3 target) {
            Vector3 direction = origin - target;
            direction.Normalize();
            Vector3 offset = direction * offsetFromWall;
            return target + offset;
        }

    }
}
