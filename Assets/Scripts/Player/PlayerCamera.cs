using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField, Range(0.1f, 10f)] private float _sensibilityX = 2f;
    [SerializeField, Range(0.1f, 10f)] private float _sensibilityY = 2f;
    [SerializeField] private LayerMask _uiLayerMask;
    [SerializeField] private float _interactionDistance = 3f;
    [SerializeField] private Transform _orientation;

    [Header("Crosshair")]
    [SerializeField] private RectTransform _crosshairImage;

    private float _xRotation;
    private float _yRotation;
    private Transform _currentSelection;
    private Camera _mainCamera;
    private ButtonBehaviour _currentButton;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleRotation();
        HandleInteraction();
    }

    private void HandleRotation()
    {
        Vector2 mouseInput = new Vector2(InputManager.Instance.MouseX, InputManager.Instance.MouseY);
        float deltaTime = Time.deltaTime;

        _xRotation -= mouseInput.y * _sensibilityY * deltaTime;
        _yRotation += mouseInput.x * _sensibilityX * deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        Quaternion cameraRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        transform.rotation = cameraRotation;
        _orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    private void HandleInteraction()
    {
        ClearPreviousSelection();

        Ray ray = _mainCamera.ScreenPointToRay(GetScreenCenter());
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance, _uiLayerMask))
        {
            ProcessHit(hit);
        }

        Debug.DrawRay(ray.origin, ray.direction * _interactionDistance, Color.yellow);
    }

    private void ClearPreviousSelection()
    {
        if (_currentSelection != null)
        {
            _currentButton?.NotOnMouse();
            _currentSelection = null;
            _currentButton = null;
        }
    }

    private void ProcessHit(RaycastHit hit)
    {
        _currentSelection = hit.transform;
        _currentButton = _currentSelection.GetComponent<ButtonBehaviour>();

        if (_currentButton != null)
        {
            if (InputManager.Instance.OnLeftClick)
            {
                _currentButton.OnMouseClick();
            }
            else
            {
                _currentButton.OnMouse();
            }
        }
    }

    private Vector3 GetScreenCenter()
    {
        // Versión optimizada del cálculo del centro de pantalla
        return new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
    }

    private void OnDisable()
    {
        ClearPreviousSelection();
    }
}