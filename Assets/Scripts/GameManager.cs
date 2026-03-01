using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void Reintentar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VolverAlJuegoPrincipal()
    {
        PlayerPrefs.SetInt("VolverDesdeMinijuego", 1);
        SceneManager.LoadScene("EscenaPrincipal");
    }
}
