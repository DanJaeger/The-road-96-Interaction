using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput _playerInput;

    private static InputManager _instance;
    public static InputManager Instance { get => _instance; }

    public Vector2 PlayerMovementInput { get; private set; }
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public bool OnLeftClick { get; private set; }
    private void Awake()
    {

        if (_instance != null)
        {
            Debug.LogWarning("Found more than one Input Manager in the scene");
        }
        _instance = this;

        SetupInput();   
    }
    void SetupInput()
    {
        _playerInput = new PlayerInput();

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
        PlayerMovementInput = context.ReadValue<Vector2>();
    }
    void OnCameraMovementInput(InputAction.CallbackContext context)
    {
        Vector3 currentCameraMovementInput = context.ReadValue<Vector2>();
        MouseX = currentCameraMovementInput.x;
        MouseY = currentCameraMovementInput.y;
    }
    void OnLeftClickInput(InputAction.CallbackContext context)
    {
        OnLeftClick = context.ReadValueAsButton();
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
