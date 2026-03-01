using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float knockbackForce = 300f;
    [SerializeField] private float maxTime = 60f;

    [Header("Referencias")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private AudioSource audioRebote;
    [SerializeField] private Text textoTiempo;
    [SerializeField] private GameObject contador;
    [SerializeField] private Transform puntoInicial; 

    private Rigidbody rb;
    private float timer;
    private bool juegoTerminado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = maxTime;

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        if (puntoInicial != null)
            transform.position = puntoInicial.position; 
    }

    void Update()
    {
        if (juegoTerminado) return;

        timer -= Time.deltaTime;

        if (textoTiempo != null)
            textoTiempo.text = "Tiempo: " + Mathf.CeilToInt(timer).ToString();

        if (timer <= 0f)
        {
            contador.SetActive(false);
            MostrarPanel(losePanel, false);
        }
    }

    void FixedUpdate()
    {
        if (juegoTerminado) return;

#if UNITY_ANDROID
        Vector3 input = new Vector3(Input.acceleration.x, 0f, Input.acceleration.y);
#else
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(moveHorizontal, 0f, moveVertical);
#endif

        rb.AddForce(input * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (juegoTerminado) return;

        if (collision.gameObject.CompareTag("Pared"))
        {
            Vector3 direccionRebote = collision.contacts[0].normal;
            rb.AddForce(direccionRebote * knockbackForce);
            if (audioRebote != null) audioRebote.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (juegoTerminado) return;

        if (other.CompareTag("Meta"))
        {
            contador.SetActive(false);
            MostrarPanel(winPanel, true);
        }
        else if (other.CompareTag("Precipicio"))
        {
            ReiniciarPosicion(); 
        }
    }


    void ReiniciarPosicion()
    {
        if (puntoInicial == null) return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        transform.position = puntoInicial.position;

        rb.isKinematic = false;
    }

    void MostrarPanel(GameObject panel, bool ganado)
    {
        juegoTerminado = true;

        if (panel != null)
            panel.SetActive(true);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (ganado)
        {
            PlayerPrefs.SetInt("Minijuego1Ganado", 1);
            PlayerPrefs.Save();
        }
    }
}
