using UnityEngine;
using UnityEngine.InputSystem;
public class CameraMovement : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] private float _moveSpeed;
    private Vector2 _moveInput;

    [Header("Camera Information")]
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Transform _pivotCamera;
    [SerializeField] private Transform _centerMapPosition;
    [SerializeField] private int _mapLimitX = 50;
    [SerializeField] private int _mapLimitY = 50;

    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom; 
    private float _currentZoom;
    
    [Header("Rotation Settings")]
    [SerializeField] private float _sensitivityRotation;
    private float _rotationX;
    private float _rotationY;
    private Camera _camera;
    private bool _isRightMouseHeld = false;

    private void Start()
    {
        _camera = Camera.main;
        _playerCamera = _camera.transform;
        _pivotCamera = transform;
        

        _currentZoom = Vector3.Distance(_playerCamera.position, _pivotCamera.position);

        Vector3 euler = _pivotCamera.localEulerAngles;
        _rotationX = euler.y;
        _rotationY = euler.x;
    }

    private void Update()
    {
        InputCameraMovement();
    }

    #region Click Detection

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
            _isRightMouseHeld = true;
        else if (context.canceled)
            _isRightMouseHeld = false;
    }

    #endregion

    #region Camera Movement

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void InputCameraMovement()
    {
        if (_moveInput == Vector2.zero)
            return;

        Vector3 forward = _pivotCamera.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = _pivotCamera.right;
        right.y = 0;
        right.Normalize();

        Vector3 direction = forward * _moveInput.y + right * _moveInput.x;
        Vector3 newPosition = transform.position + direction * _moveSpeed * Time.deltaTime;
        
        Vector3 center = _centerMapPosition.position;
        
        newPosition.x = Mathf.Clamp(
            newPosition.x,
            center.x - _mapLimitX,
            center.x + _mapLimitX
        );

        newPosition.z = Mathf.Clamp(
            newPosition.z,
            center.z - _mapLimitY,
            center.z + _mapLimitY
        );

        transform.position = newPosition;
    }

    #endregion

    #region Camera Zoom

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            Vector2 scroll = context.ReadValue<Vector2>();
            float input = scroll.y;

            _currentZoom -= input * _zoomSpeed * Time.deltaTime;
            _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

            UpdateCameraPosition();
        }
    }

    private void UpdateCameraPosition()
    {   
        _playerCamera.localPosition = new Vector3(0, 0, -_currentZoom);
    }

    #endregion

    #region Camera Rotation

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (!_isRightMouseHeld) return;

        Vector2 lookInput = context.ReadValue<Vector2>();

        _rotationX += lookInput.x * _sensitivityRotation;
        _pivotCamera.localEulerAngles = new Vector3(_rotationY, _rotationX, 0f);
    }

    #endregion
}
