using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NoteSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip destroySound;

    private AudioSource audioSource;
    private bool isInsideHitZone = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HitZone"))
            isInsideHitZone = true;
    }

    private void OnDestroy()
    {
        if (Application.isPlaying && destroySound && isInsideHitZone)
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
    }
}