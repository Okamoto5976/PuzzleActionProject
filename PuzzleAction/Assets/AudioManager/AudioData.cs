using UnityEngine;
[System.Serializable] 
public class AudioData
{
   public AudioClip audioClip; //public float volume; public float pitch;
    public float clipVolume;
    public bool isLoop;

    public Vector3 position;
    public Quaternion rotation;
    public float lifeTime;
}
