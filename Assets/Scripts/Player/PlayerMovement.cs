using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputManager))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController _characterController;
    InputManager _inputManager;

    [Header("Movement:")]
    Transform _orientation;
    float _targetSpeed = 5.0f;
    float _newSpeed = 0.0f;
    Vector3 _appliedMovement;
    Vector3 _currentMovement;
    
    [Header("Sharpness:")]
    float _moveSharpness = 10.0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputManager = GetComponent<InputManager>();

        _orientation = this.gameObject.transform.GetChild(0);
    }
    private void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float verticalInput = _inputManager.PlayerMovementInput.y;
        float horizontalInput = _inputManager.PlayerMovementInput.x;
        _currentMovement = _orientation.forward * verticalInput + _orientation.right * horizontalInput;   

        _newSpeed = Mathf.Lerp(_newSpeed, _targetSpeed, Time.deltaTime * _moveSharpness);
        _appliedMovement = _currentMovement * _newSpeed;
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }
}
