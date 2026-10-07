using UnityEngine;

public class AudioTest : MonoBehaviour
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
            volume = 1f,
            isLoop = true
        });
    }

    // 別BGM（切替テスト）
    public void ChangeBGM()
    {
        audioEvent.Raise(new AudioData
        {
            audioClip = bgm2,
            volume = 1f,
            isLoop = true
        });
    }

    // SEテスト
    public void PlaySE(AudioClip se)
    {
        audioEvent.Raise(new AudioData
        {
            audioClip = se,
            volume = 1f,
            isLoop = false
        });
    }

    // BGM停止テスト(フェードアウト)
    public void StopBGM()
    {
        AudioManager.Instance.StopBGM();      // 2秒かけて停止
    }
    public void StopBGMFast()
    {
        AudioManager.Instance.StopBGM(0.5f);  // 0.5秒で停止
    }
}