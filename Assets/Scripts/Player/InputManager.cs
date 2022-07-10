using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerInput _playerInput;
    Vector2 _playerMovementInput;
    float _mouseX;
    float _mouseY;
    bool _onLeftClick;

    public Vector2 PlayerMovementInput { get => _playerMovementInput;}
    public float MouseX { get => _mouseX;}
    public float MouseY { get => _mouseY;}
    public bool OnLeftClick { get => _onLeftClick; set => _onLeftClick = value; }
    private void Awake()
    {
        _playerInput = new PlayerInput();
        SetupInput();   
    }
    void SetupInput()
    {
        _playerInput.PlayerControls.MovePlayer.started += OnPlayerMovementInput;
        _playerInput.PlayerControls.MovePlayer.canceled += OnPlayerMovementInput;
        _playerInput.PlayerControls.MovePlayer.performed += OnPlayerMovementInput;

        _playerInput.PlayerControls.MoveCamera.started += OnCameraMovementInput;
        _playerInput.PlayerControls.MoveCamera.canceled += OnCameraMovementInput;
        _playerInput.PlayerControls.MoveCamera.performed += OnCameraMovementInput;

        _playerInput.PlayerControls.Click.started += OnLeftClickInput;
        _playerInput.PlayerControls.Click.canceled += OnLeftClickInput;
    }
    void OnPlayerMovementInput(InputAction.CallbackContext context)
    {
        _playerMovementInput = context.ReadValue<Vector2>();
    }
    void OnCameraMovementInput(InputAction.CallbackContext context)
    {
        Vector3 currentCameraMovementInput = context.ReadValue<Vector2>();
        _mouseX = currentCameraMovementInput.x;
        _mouseY = currentCameraMovementInput.y;
    }
    void OnLeftClickInput(InputAction.CallbackContext context)
    {
        _onLeftClick = context.ReadValueAsButton();
    }
    private void OnEnable()
    {
        _playerInput.PlayerControls.Enable();
    }
    private void OnDisable()
    {
        _playerInput.PlayerControls.Disable();
    }

}
