using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Movement")]
    public float _moveSpeed = 20f;
    public float _borderThickness = 20f; // pixels du bord de l'écran
    private Vector2 _moveInput;
    private Vector3 _targetPosition;

    [Header("Camera Information")]
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Transform _pivotCamera;

    [Header("Camera Angle")]
    public float tiltAngle = 45f;

    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;
    private float _currentZoom;

    [Header("Rotation Settings")]
    [SerializeField] private float _sensitivity;
    private float _rotationX;
    private float _rotationY;

    private Camera _cam;
    private bool _isRightMouseHeld = false;
    private bool _isLeftMouseHeld = false;

    private void Start()
    {
        _cam = Camera.main;
        _playerCamera = _cam.transform;
        _pivotCamera = transform;

        // Applique l'angle de la caméra
        transform.rotation = Quaternion.Euler(tiltAngle, 0, 0);

        _currentZoom = Vector3.Distance(_playerCamera.position, _pivotCamera.position);
        _targetPosition = _pivotCamera.position;

        Vector3 euler = _pivotCamera.localEulerAngles;
        _rotationX = euler.y;
        _rotationY = euler.x;
    }

    private void Update()
    {
        BorderCameraMovement();
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

    public void BorderCameraMovement()
    {
        Vector3 direction = Vector3.zero;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Directions locales basée sur la rotation du gameObject CameraPivot
        Vector3 forward = _pivotCamera.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = _pivotCamera.right;
        right.y = 0;
        right.Normalize();

        // Bordures avec directions locales
        if (mousePos.x <= _borderThickness)
            direction -= right; 
        else if (mousePos.x >= screenWidth - _borderThickness)
            direction += right;

        if (mousePos.y <= _borderThickness)
            direction -= forward;
        else if (mousePos.y >= screenHeight - _borderThickness)
            direction += forward; 

        if (direction != Vector3.zero)
            transform.position += direction * _moveSpeed * Time.deltaTime;
    }

    // placer la fonction du déplacement en ZQSD ici 
    private void InputCameraMovement()
    {
        if (_moveInput == Vector2.zero)
            return;

        // Directions locales basées sur la rotation du pivot
        Vector3 forward = _pivotCamera.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = _pivotCamera.right;
        right.y = 0;
        right.Normalize();

        // Construction du vecteur de mouvement
        Vector3 direction = forward * _moveInput.y + right * _moveInput.x;

        transform.position += direction * _moveSpeed * Time.deltaTime;
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

        _rotationX += lookInput.x * _sensitivity;
        //_rotationY = Mathf.Clamp(value, 0f, 100f);
        _pivotCamera.localEulerAngles = new Vector3(_rotationY, _rotationX, 0f);
    }

    #endregion
}
