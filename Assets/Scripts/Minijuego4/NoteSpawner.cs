using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Notas por carril")]
    [SerializeField] public GameObject[] notePrefabs;

    [Header("Posiciones de aparición")]
    [SerializeField] public Transform[] spawnPoints;

    [Header("Zonas de impacto")]
    [SerializeField] public Transform[] hitZones;

    [Header("Teclas correspondientes")]
    [SerializeField] public KeyCode[] keys = new KeyCode[]
    {
        KeyCode.LeftArrow,
        KeyCode.DownArrow,
        KeyCode.UpArrow,
        KeyCode.RightArrow
    };

    [SerializeField] private float spawnInterval = 1f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnNote(Random.Range(0, spawnPoints.Length));
            timer = 0f;
        }
    }

    private void SpawnNote(int index)
    {
        if (index < 0 || index >= spawnPoints.Length) return;

        var obj = Instantiate(notePrefabs[index], spawnPoints[index].position, Quaternion.identity);
        var note = obj.GetComponent<Note>();

        if (note != null)
        {
            note.keyToPress = keys[index];
            note.spawner = this;
            note.assignedIndex = index;
        }
    }

    public void MobilePress(int index)
    {
        if (index < 0 || index >= keys.Length) return;
        KeyCode simulatedKey = keys[index];
        foreach (var note in FindObjectsByType<Note>(FindObjectsSortMode.None))
            note.SimulateKeyPress(simulatedKey);
    }
}