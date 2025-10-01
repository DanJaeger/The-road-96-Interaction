using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Controls the player camera: rotation + UI interaction.
/// Also follows a given "camera anchor" position on the player.
/// </summary>
[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField, Range(0.1f, 10f)] private float _sensitivityX = 2f;
    [SerializeField, Range(0.1f, 10f)] private float _sensitivityY = 2f;

    [Tooltip("Orientation used for player rotation (yaw only).")]
    [SerializeField] private Transform _orientation;

    [Tooltip("Transform to follow for position (e.g., a child of the player at (0,1.5,0)).")]
    [SerializeField] private Transform _cameraAnchor;

    [Header("Crosshair (Optional)")]
    [SerializeField] private RectTransform _crosshairImage;

    private float _xRotation;
    private float _yRotation;
    private ButtonBehaviour _currentButton;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        FollowAnchor();
        HandleRotation();
        HandleInteraction();
    }

    /// <summary>
    /// Keeps the camera at the position of the anchor (e.g. player's head).
    /// </summary>
    private void FollowAnchor()
    {
        if (_cameraAnchor != null)
            transform.position = _cameraAnchor.position;
    }

    /// <summary>
    /// Rotates the camera with mouse input.
    /// </summary>
    private void HandleRotation()
    {
        Vector2 mouseInput = new Vector2(InputManager.Instance.MouseX, InputManager.Instance.MouseY);
        float deltaTime = Time.deltaTime;

        _xRotation -= mouseInput.y * _sensitivityY * deltaTime;
        _yRotation += mouseInput.x * _sensitivityX * deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        Quaternion cameraRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        transform.rotation = cameraRotation;

        if (_orientation != null)
            _orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    /// <summary>
    /// Raycasts from the center of the screen into the UI and handles hover/click on buttons.
    /// </summary>
    private void HandleInteraction()
    {
        if (EventSystem.current == null) return;

        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        PointerEventData pointer = new PointerEventData(EventSystem.current) { position = screenCenter };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);

        ButtonBehaviour foundButton = null;
        foreach (var r in results)
        {
            var btn = r.gameObject.GetComponentInParent<ButtonBehaviour>();
            if (btn != null)
            {
                foundButton = btn;
                break;
            }
        }

        if (foundButton != null)
        {
            if (foundButton != _currentButton)
            {
                _currentButton?.NotOnMouse();
                _currentButton = foundButton;
                _currentButton.OnMouse();
            }

            if (InputManager.Instance.OnLeftClick)
            {
                _currentButton.OnMouseClick();
            }
        }
        else if (_currentButton != null)
        {
            _currentButton.NotOnMouse();
            _currentButton = null;
        }
    }

    private void OnDisable()
    {
        if (_currentButton != null)
            _currentButton.NotOnMouse();
        _currentButton = null;
    }
}
