using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioClip soundClip;

    private void OnTriggerEnter(Collider other)
    {
        if (soundClip != null)
        {
            AudioSource.PlayClipAtPoint(soundClip, transform.position);
        }
    }
}