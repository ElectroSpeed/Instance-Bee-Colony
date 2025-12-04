using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Movement")]
    public float _moveSpeed = 20f; // vitesse de déplacement de la caméra
    public float _borderThickness = 20f; // pixels du bord de l'écran
    private Vector2 _moveInput; // input de déplacement
    private Vector3 _targetPosition;

    [Header("Camera Information")]
    [SerializeField] private Transform _playerCamera; //la caméra elle-même
    [SerializeField] private Transform _pivotCamera; //game object qui est parent de la caméra

    [Header("Camera Angle")]
    public float tiltAngle = 45f; // angle de la caméra sur l'axe X

    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom; 
    private float _currentZoom;

    [Header("Rotation Settings")]
    [SerializeField] private float _sensitivity; // sensibilité de la rotation de la caméra
    private float _rotationX; // rotation autour de l'axe Y
    private float _rotationY; // rotation autour de l'axe X

    private Camera _cam; // référence à la caméra principale
    private bool _isRightMouseHeld = false; // pour détecter si le clic droit est maintenu

    private void Start()
    {
        _cam = Camera.main;
        _playerCamera = _cam.transform;
        _pivotCamera = transform;

        // Applique l'angle de la caméra
        transform.rotation = Quaternion.Euler(tiltAngle, 0, 0);

        _currentZoom = Vector3.Distance(_playerCamera.position, _pivotCamera.position); // initialiser le zoom actuel
        _targetPosition = _pivotCamera.position; // initialiser la position cible

        Vector3 euler = _pivotCamera.localEulerAngles; // obtenir les angles de rotation initiaux
        _rotationX = euler.y;
        _rotationY = euler.x;
    }

    private void Update()
    {
        BorderCameraMovement();
        InputCameraMovement();
    }

    #region Click Detection

    public void OnRightClick(InputAction.CallbackContext context) // détecte si le clic droit est maintenu
    {
        if (context.started)
            _isRightMouseHeld = true;
        else if (context.canceled)
            _isRightMouseHeld = false;
    }

    #endregion

    #region Camera Movement

    public void OnMove(InputAction.CallbackContext context) // input de déplacement
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void BorderCameraMovement() // déplacer la caméra lorsque la souris est proche des bords de l'écran
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
    private void InputCameraMovement() // déplacer la caméra avec les touches
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

    public void OnZoom(InputAction.CallbackContext context) // input de zoom
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

    private void UpdateCameraPosition() // met à jour la position de la caméra en fonction du zoom actuel
    {   
        _playerCamera.localPosition = new Vector3(0, 0, -_currentZoom);
    }

    #endregion

    #region Camera Rotation

    public void OnRotate(InputAction.CallbackContext context) // input de rotation
    {
        if (!_isRightMouseHeld) return;

        Vector2 lookInput = context.ReadValue<Vector2>();

        _rotationX += lookInput.x * _sensitivity;
        //_rotationY = Mathf.Clamp(value, 0f, 100f);
        _pivotCamera.localEulerAngles = new Vector3(_rotationY, _rotationX, 0f);
    }

    #endregion
}
