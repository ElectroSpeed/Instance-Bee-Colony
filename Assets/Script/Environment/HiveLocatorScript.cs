using UnityEngine;

public class HiveLocatorScript : MonoBehaviour
{
    private Camera playerCamera;

    [Header("Rotation")]
    public bool _yLock = true;

    [Header("Scale par distance")]
    public float _minDistance = 5f;
    public float _maxDistance = 50f;
    public float _minScale = 0.5f;
    public float _maxScale = 2f;

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 direction = transform.position - playerCamera.transform.position;
        if (_yLock)
        {
            direction.y = 0f;
        }
        transform.rotation = Quaternion.LookRotation(direction);
        
        float distance = Vector3.Distance(transform.position, playerCamera.transform.position);
        
        float t = Mathf.InverseLerp(_minDistance, _maxDistance, distance);
        
        float scale = Mathf.Lerp(_minScale, _maxScale, t);
        transform.localScale = Vector3.one * scale;
    }
}