using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParticleController : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private uint maxValue = 9999;
    [SerializeField] private int testCount = 3;

    private Coroutine coroutine;

    [ContextMenu("Fire DamageParticle")]
    private void EditorTestParticle()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(DamageCoroutine());
    }

    private IEnumerator DamageCoroutine()
    {
        for (int i = 0; i < testCount; i++)
        {
            var randomInt = Random.Range(0, (int)maxValue);
            Debug.Log($"Firing Particle with value {randomInt}");
            DoDamageParticle((uint)randomInt);
            yield return new WaitForSeconds(0.1f);
        }
    }
#endif

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private int _maxDigits = 4;
    [SerializeField] private int _maxParticleCount = 10;

    private List<ParticleSystem> _particleSystems;
    private int _currentParticleSystemIndex = 0;

    private void Start()
    {
        _particleSystems = new List<ParticleSystem>
        {
            _particleSystem
        };
        for (int i = 1; i < _maxParticleCount; i++)
        {
            _particleSystems.Add(Instantiate(_particleSystem, transform));
        }
    }

    public void DoDamageParticle(uint damage)
    {
        var customData = _particleSystems[_currentParticleSystemIndex].customData;
        customData.enabled = true;

        int len = damage.ToString().Length;
        ParticleSystem.MinMaxCurve lengthData = new(len);
        ParticleSystem.MinMaxCurve numberData = new(damage / (Mathf.Pow(10, _maxDigits)));

        customData.SetVector(ParticleSystemCustomData.Custom1, 0, lengthData);
        customData.SetVector(ParticleSystemCustomData.Custom1, 1, numberData);
        _particleSystems[_currentParticleSystemIndex].Emit(1);
        _currentParticleSystemIndex = (_currentParticleSystemIndex + 1) % _maxParticleCount;
    }
}
