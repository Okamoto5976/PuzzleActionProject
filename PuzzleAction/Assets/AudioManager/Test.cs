using UnityEngine;

public class Test : MonoBehaviour
{
    public AudioEventSO audioEvent;

    public AudioClip bgm1;
    public AudioClip bgm2;

    // 同じBGM（ループテスト）
    public void PlaySameBGM()
    {
        audioEvent.Raise(new AudioData
        {
            audioClip = bgm1,
            clipVolume = 1f,
            isLoop = true
        });
    }

    // 別BGM（切替テスト）
    public void ChangeBGM()
    {
        audioEvent.Raise(new AudioData
        {
            audioClip = bgm2,
            clipVolume = 1f,
            isLoop = true
        });
    }

    // SEテスト
    public void PlaySE(AudioClip se)
    {
        audioEvent.Raise(new AudioData
        {
            audioClip = se,
            clipVolume = 1f,
            isLoop = false
        });
    }
}