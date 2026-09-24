using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimTrail : MonoBehaviour
{
    [SerializeField] private int _initializedTrailCount = 20;
    [SerializeField] private float _trailResolution = 1;
    private int TrailCount
    {
        get => _initializedTrailCount;
        set
        {
            _initializedTrailCount = value;
            _points = new Vector3[_initializedTrailCount];
            _lineRenderer.positionCount = _initializedTrailCount;
        }
    }
    private Vector3 _initialPosition;
    private Vector3 _direction;
    private float _velocity;
    private float _gravityMultiplier;
    private Vector3[] _points;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        TrailCount = _initializedTrailCount;
        gameObject.SetActive(false);
    }

    public void UpdateVariables(Vector3 initialPosition, Vector3 direction, float velocity, float gravityMultiplier = 1.0f)
    {
        _initialPosition = initialPosition;
        _direction = direction;
        _velocity = velocity;
        _gravityMultiplier = gravityMultiplier;
    }

    private void Update()
    {
        DoTrails();
    }

    private void DoTrails()
    {
        _points[0] = _initialPosition;
        _lineRenderer.SetPosition(0, _points[0]);
        Vector3 startVelocity = _direction * _velocity;

        for (int i = 1; i < TrailCount; i++)
        {
            float timeOffset = (i * Time.fixedDeltaTime * _trailResolution);
            Vector3 gravityOffset = _gravityMultiplier * 0.5f * Mathf.Pow(timeOffset, 2) * Physics.gravity;
            _points[i] = _points[0] + startVelocity * timeOffset + gravityOffset;
            _lineRenderer.SetPosition(i, _points[i]);
        }
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_lineRenderer == null) return;
        TrailCount = _initializedTrailCount;
    }
#endif
}
