using System;
using UnityEngine;

public class BeeHover : MonoBehaviour
{
    [SerializeField] private GameObject _meshGameobject;
    [SerializeField] private ColliderDetector _groundCheckCollider;
    [SerializeField] bool _groundCheck;
    
    [SerializeField] float _fallingSpeed = 1;
    [SerializeField] float _climbingSpeed=1;
    
    [SerializeField] float _animSpeedX=1;
    [SerializeField] float _sinAmplifierX = 1;
    
    [SerializeField] float _animSpeedY=1;
    [SerializeField] float _sinAmplifierY = 1; 
    
    
    [SerializeField] float _animSpeedZ=1;
    [SerializeField] float _sinAmplifierZ = 1;

    //Debug
    [SerializeField] private float b = 1;

    private bool _climbing = false;
    private bool _lastState=false;
    private float _lastPos = 0;
    private float _dist = 2;
    
    private Vector3 _sinPos= Vector3.zero;
    private Vector3 _basePos;

    private void Start()
    {
        _basePos = transform.localPosition;
        Vector3 groundCheckColliderPos=new Vector3(1f, _sinAmplifierY, 1f);
        groundCheckColliderPos+=new Vector3(0,
            GetComponent<BoxCollider>().size.y*2+Mathf.Abs(GetComponent<BoxCollider>().center.y)
            ,0);
        
        _groundCheckCollider.GetComponent<CapsuleCollider>().height = groundCheckColliderPos.y+1;
        _groundCheckCollider.onTriggerEnterFunction+= x => { _groundCheck = true; };
        _groundCheckCollider.onTriggerExitFunction+= x => { _groundCheck = false; };
    }

    private void Update()
    {
        //Debug
        transform.parent.position += Vector3.back * (Time.deltaTime * b);
        
        _sinPos.x = Mathf.Sin(Time.time*_animSpeedX)*_sinAmplifierX;
        _sinPos.y= Mathf.Sin(Time.time*_animSpeedY)*_sinAmplifierY;
        _sinPos.z= Mathf.Sin(Time.time*_animSpeedZ)*_sinAmplifierZ;
        
        _meshGameobject.transform.localPosition = _basePos+_sinPos;
        //targetPos = transform.parent.position;
        

        if (_lastState)
        {
            transform.parent.position += Vector3.up * (_climbingSpeed * Time.deltaTime);
            if (transform.parent.position.y >= _lastPos)
            {
                _lastState = false;
            }
        }
        else
        {
            if (_climbing)
            {
                transform.parent.position += Vector3.up * (_climbingSpeed * Time.deltaTime);
            }
            else if (!_groundCheck)
            {
                transform.parent.position -= Vector3.up * (_fallingSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _climbing = true;
        _lastState = _climbing;
        
    }

    private void OnTriggerExit(Collider other)
    {
        _climbing = false;
        _lastState = true;
        _lastPos = transform.parent.position.y+_dist;

    }
    
    
}
