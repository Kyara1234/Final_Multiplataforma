using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Lenguaje : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text textoCorruptoUI;
    [SerializeField] private GameObject panelGanar;
    [SerializeField] private GameObject panelPerder;
    [SerializeField] private GameObject panelOpciones;
    [SerializeField] private Text contadorTiempoUI;
    [SerializeField] private Slider barraErrores;
    [SerializeField] private Text pistaTextoUI;
    [SerializeField] private AudioSource sonidoClick;

    [Header("Configuración")]
    [SerializeField] private float tiempoMaximo = 30f;

    private List<Pregunta> preguntas = new List<Pregunta>();
    private Pregunta preguntaActual;
    private int aciertos = 0;
    private int errores = 0;
    private float tiempoRestante;
    private bool minijuegoActivo = true;
    private GameObject[] botones3D;

    void Start()
    {
        tiempoRestante = tiempoMaximo;
        barraErrores.maxValue = 3;
        barraErrores.value = 0;
        botones3D = GameObject.FindGameObjectsWithTag("Boton");

        CargarPreguntas();
        MostrarNuevaPregunta();
    }

    void Update()
    {
        if (!minijuegoActivo) return;

        tiempoRestante -= Time.deltaTime;
        contadorTiempoUI.text = "Tiempo: " + Mathf.CeilToInt(tiempoRestante).ToString();

        if (tiempoRestante <= 0)
        {
            FinalizarMinijuego(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Boton"))
            {
                TextMeshPro texto = hit.collider.GetComponentInChildren<TextMeshPro>();
                if (texto != null)
                {
                    if (sonidoClick != null) sonidoClick.Play();
                    RevisarRespuesta(texto.text.Trim());
                }
            }
        }
    }

    void CargarPreguntas()
    {
        preguntas.Clear();
        preguntas.Add(new Pregunta("Trnslr", "Traducir", new[] { "Tramisar", "Tranzlir" }));
        preguntas.Add(new Pregunta("Hol4", "Hola", new[] { "Olaa", "Hóla" }));
        preguntas.Add(new Pregunta("Anlss", "Análisis", new[] { "Analiss", "Anlisis" }));
        preguntas.Add(new Pregunta("Err0r", "Error", new[] { "Heror", "Errar" }));
        preguntas.Add(new Pregunta("Msg", "Mensaje", new[] { "Mesanje", "Mensage" }));
    }

    string ObtenerPista(string palabra)
    {
        switch (palabra)
        {
            case "Traducir": return "Tiene que ver con convertir idiomas.";
            case "Hola": return "Es lo primero que dices al saludar.";
            case "Análisis": return "Es lo que haces cuando estudias algo a fondo.";
            case "Error": return "Ocurre cuando algo sale mal.";
            case "Mensaje": return "Es algo que envías o recibes al comunicarte.";
            default: return "Busca la opción más lógica.";
        }
    }

    void MostrarNuevaPregunta()
    {
        if (preguntas.Count == 0)
        {
            FinalizarMinijuego(true);
            return;
        }

        int index = Random.Range(0, preguntas.Count);
        preguntaActual = preguntas[index];
        preguntas.RemoveAt(index);

        textoCorruptoUI.text = preguntaActual.textoCorrupto;
        pistaTextoUI.text = "Pista: " + ObtenerPista(preguntaActual.respuestaCorrecta);

        List<string> opciones = new List<string> { preguntaActual.respuestaCorrecta };
        opciones.AddRange(preguntaActual.respuestasIncorrectas);
        opciones.Sort((a, b) => Random.Range(-1, 2));

        for (int i = 0; i < botones3D.Length && i < opciones.Count; i++)
        {
            TextMeshPro texto = botones3D[i].GetComponentInChildren<TextMeshPro>();
            if (texto != null)
                texto.text = opciones[i];
        }

        TextoGlitch glitch = textoCorruptoUI.GetComponent<TextoGlitch>();
        if (glitch != null)
            glitch.ForzarActualizarTexto();
    }

    void RevisarRespuesta(string seleccion)
    {
        if (!minijuegoActivo) return;

        if (seleccion == preguntaActual.respuestaCorrecta)
            aciertos++;
        else
        {
            errores++;
            barraErrores.value = errores;
        }

        if (aciertos >= 3)
            FinalizarMinijuego(true);
        else if (errores >= 3)
            FinalizarMinijuego(false);
        else
            MostrarNuevaPregunta();
    }

    void FinalizarMinijuego(bool ganado)
    {
        minijuegoActivo = false;
        panelOpciones.SetActive(false);

        if (ganado)
        {
            panelGanar.SetActive(true);
            PlayerPrefs.SetInt("Minijuego2Ganado", 1);
        }
        else
        {
            panelPerder.SetActive(true);
        }

        PlayerPrefs.Save();
    }

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
