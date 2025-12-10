using System;
using UnityEngine;

public class BeeHover : MonoBehaviour
{
    [SerializeField] private GameObject meshGameobject;
    [SerializeField] private ColliderDetector groundCheckCollider;
    [SerializeField] bool groundCheck;
    
    [SerializeField] float fallingSpeed = 1;
    [SerializeField] float climbingSpeed=1;
    
    [SerializeField] float animSpeedX=1;
    [SerializeField] float sinAmplifierX = 1;
    
    [SerializeField] float animSpeedY=1;
    [SerializeField] float sinAmplifierY = 1; 
    
    
    [SerializeField] float animSpeedZ=1;
    [SerializeField] float sinAmplifierZ = 1;

    //Debug
    [SerializeField] private float b = 1;

    private bool climbing = false;
    private bool lastState=false;
    private float lastPos = 0;
    private float dist = 2;
    
    private Vector3 sinPos= Vector3.zero;
    private Vector3 basePos;

    private void Start()
    {
        basePos = transform.localPosition;
        Vector3 groundCheckColliderPos=new Vector3(1f, sinAmplifierY, 1f);
        groundCheckColliderPos+=new Vector3(0,
            GetComponent<BoxCollider>().size.y*2+Mathf.Abs(GetComponent<BoxCollider>().center.y)
            ,0);
        
        groundCheckCollider.GetComponent<CapsuleCollider>().height = groundCheckColliderPos.y+1;
        groundCheckCollider.onTriggerEnterFunction+= x => { groundCheck = true; };
        groundCheckCollider.onTriggerExitFunction+= x => { groundCheck = false; };
    }

    private void Update()
    {
        //Debug
        transform.parent.position += Vector3.back * (Time.deltaTime * b);
        
        sinPos.x = Mathf.Sin(Time.time*animSpeedX)*sinAmplifierX;
        sinPos.y= Mathf.Sin(Time.time*animSpeedY)*sinAmplifierY;
        sinPos.z= Mathf.Sin(Time.time*animSpeedZ)*sinAmplifierZ;
        
        meshGameobject.transform.localPosition = basePos+sinPos;
        //targetPos = transform.parent.position;
        

        if (lastState)
        {
            transform.parent.position += Vector3.up * (climbingSpeed * Time.deltaTime);
            if (transform.parent.position.y >= lastPos)
            {
                lastState = false;
            }
        }
        else
        {
            if (climbing)
            {
                transform.parent.position += Vector3.up * (climbingSpeed * Time.deltaTime);
            }
            else if (!groundCheck)
            {
                transform.parent.position -= Vector3.up * (fallingSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        climbing = true;
        lastState = climbing;
        
    }

    private void OnTriggerExit(Collider other)
    {
        climbing = false;
        lastState = true;
        lastPos = transform.parent.position.y+dist;

    }
    
    
}
