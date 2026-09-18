using UnityEngine;

public class AimTrailTester : MonoBehaviour
{
    [SerializeField] private AimTrail _aimTrail;
    [SerializeField] private float _power;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private Rigidbody _rigidBody;

    [ContextMenu("Test Trail")]
    private void TestTrail()
    {
        var dir = _direction.normalized;
        _rigidBody.angularVelocity = Vector3.zero;
        _rigidBody.linearVelocity = Vector3.zero;
        _rigidBody.position = transform.position;
        _rigidBody.AddForce(dir * _power, ForceMode.VelocityChange);
    }

    private void Update()
    {
        var dir = _direction.normalized;
        _aimTrail.UpdateVariables(transform.position, dir, _power, 1);
    }
}
