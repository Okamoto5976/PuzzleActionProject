using UnityEngine;
using System.Collections;

public class AudioFader : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 2.0f;

    private Coroutine fadeCoroutine;

    // ★ フェードアウト後に別BGMを再生
    public void FadeOutAndPlay(AudioClip nextClip, float targetVolume = 1f)
    {
        if(audioSource.clip == nextClip) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutAndPlayCoroutine(nextClip, targetVolume));
    }

    private IEnumerator FadeOutAndPlayCoroutine(AudioClip nextClip, float targetVolume)
    {
        // --- フェードアウト ---
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();

        // --- BGM差し替え ---
        audioSource.clip = nextClip;
        audioSource.loop = true;
        audioSource.Play();

        // --- フェードイン ---
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public void InstancePlay(AudioClip nextClip, float targetVolume = 1f)
    {
        audioSource.clip = nextClip;
        audioSource.volume = targetVolume;
        audioSource.Play();
    }

    public void FadeOutAndStop(float duration = -1f)
    {
        if (duration < 0f) duration = fadeDuration;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutAndStopCoroutine(duration));
    }

    private IEnumerator FadeOutAndStopCoroutine(float duration)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        audioSource.clip = null;
        fadeCoroutine = null;
    }
}
