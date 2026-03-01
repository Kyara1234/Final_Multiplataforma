using UnityEngine;
using UnityEngine.SceneManagement;

public class Activador : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string nombreEscenaMinijuego;
    [SerializeField] private string clavePlayerPrefs = "Minijuego1Ganado";

    [Header("Referencias")]
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Renderer objetoVisual;
    [SerializeField] private Material materialVerde;

    private bool jugadorCerca = false;
    private bool minijuegoYaGanado = false;

    void Start()
    {
        if (PlayerPrefs.GetInt(clavePlayerPrefs, 0) == 1)
        {
            minijuegoYaGanado = true;
            if (objetoVisual != null && materialVerde != null)
                objetoVisual.material = materialVerde;
        }
    }

    void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && !minijuegoYaGanado)
        {
            GuardarPosicionJugador();
            SceneManager.LoadScene(nombreEscenaMinijuego);
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && jugadorCerca && !minijuegoYaGanado)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == this.gameObject)
            {
                GuardarPosicionJugador();
                SceneManager.LoadScene(nombreEscenaMinijuego);
            }
        }
#endif
    }

    void GuardarPosicionJugador()
    {
        Vector3 pos = playerObject.transform.position;
        float rotY = playerObject.transform.eulerAngles.y;

        PlayerPrefs.SetFloat("returnPosX", pos.x);
        PlayerPrefs.SetFloat("returnPosY", pos.y);
        PlayerPrefs.SetFloat("returnPosZ", pos.z);
        PlayerPrefs.SetFloat("returnRotY", rotY);
        PlayerPrefs.SetInt("VolverDesdeMinijuego", 1);
        PlayerPrefs.Save();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = false;
    }
}
