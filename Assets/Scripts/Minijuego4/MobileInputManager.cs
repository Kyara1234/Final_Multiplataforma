using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    [Header("Contenedor UI de controles móviles")]
    [SerializeField] private GameObject mobileControls;

    private void Start()
    {
#if UNITY_ANDROID
        mobileControls?.SetActive(true);
#else
        mobileControls?.SetActive(false);
#endif
    }

    public void PressLeft() => PressArrow(KeyCode.LeftArrow);
    public void PressDown() => PressArrow(KeyCode.DownArrow);
    public void PressUp() => PressArrow(KeyCode.UpArrow);
    public void PressRight() => PressArrow(KeyCode.RightArrow);

    private void PressArrow(KeyCode key)
    {
        foreach (var note in FindObjectsByType<Note>(FindObjectsSortMode.None))
            note.SimulateKeyPress(key);
    }
}