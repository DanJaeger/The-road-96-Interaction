using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform _cameraPosition;
    void Update()
    {
        transform.position = _cameraPosition.position;
    }
}
