using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject panelWin;
    [SerializeField] private GameObject panelLose;

    void Start()
    {
        string escena = SceneManager.GetActiveScene().name;

        if (escena == "Minijuego2")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            string escena = SceneManager.GetActiveScene().name;

            if (escena != "Minijuego2" && !EstaPanelActivo(panelWin) && !EstaPanelActivo(panelLose))
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
        }

        if (EstaPanelActivo(panelWin) || EstaPanelActivo(panelLose))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private bool EstaPanelActivo(GameObject panel)
    {
        return panel != null && panel.activeSelf;
    }
}
