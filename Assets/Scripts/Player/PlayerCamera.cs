using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField, Range(0.1f, 10f)] private float _sensibilityX = 2f;
    [SerializeField, Range(0.1f, 10f)] private float _sensibilityY = 2f;
    [SerializeField] private Transform _orientation;

    [Header("Crosshair")]
    [SerializeField] private RectTransform _crosshairImage;

    private float _xRotation;
    private float _yRotation;
    private ButtonBehaviour _currentButton;

    private void Awake()
    {
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
        if (_orientation != null)
            _orientation.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }

    private void HandleInteraction()
    {
        if (EventSystem.current == null) return;

        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        PointerEventData pointer = new PointerEventData(EventSystem.current) { position = screenCenter };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, results);

        // DEBUG: listar todos los resultados
        foreach (var r in results)
        {
            Debug.Log($"[UI RAYCAST] Hit: {r.gameObject.name} (Module: {r.module}, Depth: {r.depth})");
        }

        // Buscar el primer objeto que tenga ButtonBehaviour en sus padres
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
                if (_currentButton != null)
                {
                    Debug.Log($"[UI RAYCAST] Exit: {_currentButton.name}");
                    _currentButton.NotOnMouse();
                }

                _currentButton = foundButton;

                Debug.Log($"[UI RAYCAST] Enter: {_currentButton.name}");
                _currentButton.OnMouse();
            }

            if (InputManager.Instance.OnLeftClick)
            {
                Debug.Log($"[UI RAYCAST] Click on {_currentButton.name}");
                _currentButton.OnMouseClick();
            }
        }
        else
        {
            if (_currentButton != null)
            {
                Debug.Log($"[UI RAYCAST] Exit: {_currentButton.name}");
                _currentButton.NotOnMouse();
                _currentButton = null;
            }
        }
    }

    private void OnDisable()
    {
        if (_currentButton != null)
        {
            Debug.Log($"[UI RAYCAST] Exit (OnDisable): {_currentButton.name}");
            _currentButton.NotOnMouse();
        }
        _currentButton = null;
    }
}
