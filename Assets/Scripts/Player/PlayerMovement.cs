using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputManager))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Components:")]
    CharacterController _characterController;

    [Header("Movement:")]
    Transform _orientation;
    readonly float ro_targetSpeed = 5.0f;
    float _newSpeed = 0.0f;
    Vector3 _appliedMovement;
    Vector3 _currentMovement;
    
    [Header("Sharpness:")]
    float _moveSharpness = 10.0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _orientation = this.gameObject.transform.GetChild(0);
    }
    private void Update()
    {
       MovePlayer();
    }

    void MovePlayer()
    {
        float verticalInput = InputManager.Instance.PlayerMovementInput.y;
        float horizontalInput = InputManager.Instance.PlayerMovementInput.x;
        _currentMovement = _orientation.forward * verticalInput + _orientation.right * horizontalInput;   

        _newSpeed = Mathf.Lerp(_newSpeed, ro_targetSpeed, Time.deltaTime * _moveSharpness);
        _appliedMovement = _currentMovement * _newSpeed;
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }
}
