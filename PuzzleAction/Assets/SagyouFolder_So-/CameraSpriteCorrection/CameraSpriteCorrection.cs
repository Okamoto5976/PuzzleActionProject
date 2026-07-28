using UnityEngine;

/// <summary>
/// Created by: So-
/// Correct sprite rotation by Camera angle, does not compensate y rotation
/// </summary>
public class CameraSpriteCorrection : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        if (_targetCamera == null) _targetCamera = Camera.main;
    }

    private void Update()
    {
        CorrectRotation();
    }

    private void CorrectRotation()
    {
        Vector3 cameraRotation = _targetCamera.transform.localRotation.eulerAngles;
        Vector3 rotation = _transform.localRotation.eulerAngles;
        rotation.x = cameraRotation.x;
        _transform.localRotation = Quaternion.Euler(rotation);
    }
}
