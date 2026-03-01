using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private float returnDelay = 3f;
    [SerializeField] private float overloadDuration = 5f;
    [SerializeField] private float overloadNoteSpeedMultiplier = 2f;

    public static ScoreManager instance;

    private int notesHit;
    private bool isReturning;
    private float overloadTimer;
    public float NoteSpeedMultiplier => overloadTimer > 0 ? overloadNoteSpeedMultiplier : 1f;

    private int totalNotasDestruidas;
    private int notasFalladas;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (overloadTimer > 0)
            overloadTimer -= Time.deltaTime;
    }

    public void AddHit()
    {
        notesHit++;

        if (notesHit % 10 == 0)
            overloadTimer = overloadDuration;

        totalNotasDestruidas++;
        RevisarNotasDestruidas();
    }

    public void AddMiss()
    {
        totalNotasDestruidas++;
        RevisarNotasDestruidas();

        if (!isReturning)
            StartCoroutine(ShowMessageAndReturn("Fallaste"));
    }

    public void NotifyNoteDestroyed()
    {
        totalNotasDestruidas++;
        RevisarNotasDestruidas();
    }

    public void RegistrarNotaFallada()
    {
        notasFalladas++;

        if (notasFalladas >= 5 && !isReturning)
        {
            isReturning = true;
            StartCoroutine(ShowMessageAndReturn("Secuencia Fallida... Reiniciando"));
        }
    }

    private void RevisarNotasDestruidas()
    {
        if (totalNotasDestruidas >= 10 && !isReturning)
        {
            isReturning = true;
            StartCoroutine(ShowMessageAndReturn("¡Ganaste!"));
        }
    }

    private System.Collections.IEnumerator ShowMessageAndReturn(string message)
    {
        yield return new WaitForSeconds(returnDelay);
        SceneManager.LoadScene("EscenaPrincipal");
    }
}
