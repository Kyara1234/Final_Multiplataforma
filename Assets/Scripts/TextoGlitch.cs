using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextoGlitch : MonoBehaviour
{
    [Header("Configuración Glitch")]
    [SerializeField] private Text textoObjetivo;
    [SerializeField] private float intervalo = 0.1f;
    [SerializeField] private int letrasGlitcheadas = 2;

    private string textoActual = "";
    private bool forzarActualizacion = false;

    void Start()
    {
        if (textoObjetivo == null) return;
        textoActual = textoObjetivo.text;
        StartCoroutine(AnimarGlitch());
    }

    public void ForzarActualizarTexto()
    {
        textoActual = textoObjetivo.text;
        forzarActualizacion = true;
    }

    IEnumerator AnimarGlitch()
    {
        yield return new WaitForSeconds(0.7f);

        while (true)
        {
            string original = textoObjetivo.text;

            if (original != textoActual || forzarActualizacion)
            {
                textoActual = original;
                forzarActualizacion = false;
            }

            char[] letras = textoActual.ToCharArray();
            int cambios = 0;

            for (int i = 0; i < letras.Length && cambios < letrasGlitcheadas; i++)
            {
                if (Random.value < 0.3f)
                {
                    letras[i] = (char)Random.Range(33, 126);
                    cambios++;
                }
            }

            textoObjetivo.text = new string(letras);
            yield return new WaitForSeconds(intervalo);
            textoObjetivo.text = textoActual;
            yield return new WaitForSeconds(intervalo);
        }
    }
}
