using UnityEngine;

public class PlayOnEnableSound : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnEnable()
    {
        audioSource.Play();
    }
}
