using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton Input Manager using the new Unity Input System.
/// Provides access to player movement, camera look, and click input.
/// </summary>
public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;

    // --- Singleton ---
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("No InputManager instance found in the scene!");
            return _instance;
        }
    }

    // --- Input Values ---
    public Vector2 PlayerMovementInput { get; private set; }
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public bool OnLeftClick { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Multiple InputManager instances detected. Destroying the new one.");
            Destroy(gameObject);
            return;
        }
        _instance = this;

        SetupInput();
    }

    /// <summary>
    /// Initializes the PlayerInput and subscribes to input actions.
    /// </summary>
    private void SetupInput()
    {
        _playerInput = new PlayerInput();

        // Movement
        _playerInput.PlayerControls.MovePlayer.started += OnPlayerMovementInput;
        _playerInput.PlayerControls.MovePlayer.performed += OnPlayerMovementInput;
        _playerInput.PlayerControls.MovePlayer.canceled += OnPlayerMovementInput;

        // Camera look
        _playerInput.PlayerControls.MoveCamera.started += OnCameraMovementInput;
        _playerInput.PlayerControls.MoveCamera.performed += OnCameraMovementInput;
        _playerInput.PlayerControls.MoveCamera.canceled += OnCameraMovementInput;

        // Left click
        _playerInput.PlayerControls.Click.started += OnLeftClickInput;
        _playerInput.PlayerControls.Click.canceled += OnLeftClickInput;
    }

    /// <summary>
    /// Handles movement input (WASD or analog stick).
    /// </summary>
    private void OnPlayerMovementInput(InputAction.CallbackContext context)
    {
        PlayerMovementInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Handles camera look input (mouse delta or right stick).
    /// </summary>
    private void OnCameraMovementInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        MouseX = input.x;
        MouseY = input.y;
    }

    /// <summary>
    /// Handles left click (mouse or gamepad button).
    /// </summary>
    private void OnLeftClickInput(InputAction.CallbackContext context)
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
