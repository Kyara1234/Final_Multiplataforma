using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Note : MonoBehaviour
{
    [SerializeField] public KeyCode keyToPress;
    [SerializeField] private AudioClip hitSound;

    [HideInInspector] public NoteSpawner spawner;
    [HideInInspector] public int assignedIndex;

    private bool canBePressed = false;
    private bool wasInHitZone = false;
    private float baseSpeed = 2.5f;

    private void Update()
    {
        transform.Translate(Vector3.down * (baseSpeed * Time.deltaTime * ScoreManager.instance.NoteSpeedMultiplier));

        if (canBePressed && Input.GetKeyDown(keyToPress))
            HandleHit();
    }

    private void HandleHit()
    {
        if (wasInHitZone && hitSound)
            AudioSource.PlayClipAtPoint(hitSound, transform.position);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HitZone"))
        {
            canBePressed = true;
            wasInHitZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HitZone"))
            canBePressed = false;
    }

    public void CheckHitFromMobile()
    {
        if (!canBePressed) return;

        float distance = Mathf.Abs(transform.position.y - spawner.hitZones[assignedIndex].position.y);
        if (distance <= 0.5f)
            HandleHit();
    }

    private void OnBecameInvisible()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.NotifyNoteDestroyed();
            ScoreManager.instance.RegistrarNotaFallada();
        }

        Destroy(gameObject);
    }

    public void SimulateKeyPress(KeyCode simulatedKey)
    {
        if (canBePressed && simulatedKey == keyToPress)
            HandleHit();
    }
}