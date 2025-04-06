using UnityEngine;

public class AudioAnim : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audio;
    public void PlayMing(AudioClip clip)
    {
        _audio.PlayOneShot(clip,Random.Range(0.9f,1.0f));
    }
}
