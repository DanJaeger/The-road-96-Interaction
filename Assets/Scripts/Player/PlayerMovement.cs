using UnityEngine;

/// <summary>
/// Handles player movement using a CharacterController.
/// Movement direction is based on an orientation transform (usually the camera).
/// Requires: CharacterController + InputManager
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputManager))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    private CharacterController _characterController;

    [Tooltip("Transform used to determine movement direction (typically a child with camera orientation).")]
    [SerializeField] private Transform _orientation;

    [Header("Movement Settings")]
    [Range(1f, 15f)]
    private float _targetSpeed = 5.0f;

    [Range(1f, 30f)]
    private float _accelerationSharpness = 10.0f;

    private float _currentSpeed;
    private Vector3 _movementInput;
    private Vector3 _appliedMovement;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        // If no orientation is assigned, take the first child by default
        if (_orientation == null && transform.childCount > 0)
        {
            _orientation = transform.GetChild(0);
            Debug.LogWarning($"{nameof(PlayerMovement)}: No orientation assigned. Using first child as orientation.");
        }
    }

    private void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// Reads input, calculates movement direction, smooths speed, and moves the CharacterController.
    /// </summary>
    private void HandleMovement()
    {
        // --- Get input from InputManager ---
        Vector2 input = InputManager.Instance.PlayerMovementInput;
        _movementInput = _orientation.forward * input.y + _orientation.right * input.x;
        _movementInput.Normalize(); // normalize so diagonal movement isn’t faster

        // --- Smooth speed towards target ---
        _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, Time.deltaTime * _accelerationSharpness);

        // --- Apply movement ---
        _appliedMovement = _movementInput * _currentSpeed;
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }
}
