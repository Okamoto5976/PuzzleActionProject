using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private Vector3Asset _playerPosition;
    [SerializeField] private float _heightOffset = 15;

    private void Update()
    {
        transform.position = _playerPosition.Value + Vector3.up * _heightOffset;
    }
}
