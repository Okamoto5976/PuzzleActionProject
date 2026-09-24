using UnityEngine;

[System.Serializable]
public class GolemController
{
    [Header("AttackRange")]
    public float ShortRange = 4f;

    public float LongRange = 10f;

    [Header("Stomp")]
    public float JumpHeight = 4f;

    public float JumpDuration = 0.5f;

    public float ShockWaveLifeTime = 1.5f;

    public StompShockWave ShockWavePrefab;
}