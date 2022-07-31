using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera: ")]
    [SerializeField] RectTransform _crosshairImage;
    [SerializeField] Transform _orientation;
    [SerializeField] float _sensibilityX;
    [SerializeField] float _sensibilityY;
    [SerializeField] LayerMask _uiLayermask;

    float _xRotation = 0.0f;
    float _yRotation = 0.0f;

    Transform _selection;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        HandleRotation();
        SelectObject();
    }
    public void HandleRotation()
    {
        float mouseX = InputManager.Instance.MouseX * _sensibilityX * Time.deltaTime;
        float mouseY = InputManager.Instance.MouseY * _sensibilityY * Time.deltaTime;

        _xRotation -= mouseY;
        _yRotation += mouseX;
        _xRotation = Mathf.Clamp(_xRotation, -80.0f, 80.0f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
    void SelectObject()
    {
        if(_selection != null)
        {
            ButtonBehaviour button = _selection.GetComponent<ButtonBehaviour>();
            button.NotOnMouse();
            _selection = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(GetCenterOfScreen());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3, _uiLayermask))
        {
            Transform selection = hit.transform;
            ButtonBehaviour button = selection.GetComponent<ButtonBehaviour>();
            if (button != null)
            {
                if (InputManager.Instance.OnLeftClick)
                    button.OnMouseClick();
                else
                    button.OnMouse();

                _selection = selection;
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 3, Color.yellow);
    }
    Vector3 GetCenterOfScreen()
    {
        int x = (Screen.width / 2) - (int)_crosshairImage.rect.width;
        int y = (Screen.height / 2) - (int)_crosshairImage.rect.height;
        Vector3 center = new Vector3(x, y, 0);
        return center;
    }
}
