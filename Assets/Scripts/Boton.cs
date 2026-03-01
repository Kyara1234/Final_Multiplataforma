using UnityEngine;
using System.Collections;

public class Boton : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float distancia = 0.1f;
    [SerializeField] private float duracion = 0.1f;

    private Vector3 posicionOriginal;
    private bool presionando = false;

    void Start()
    {
        posicionOriginal = transform.localPosition;
    }

    void OnMouseDown()
    {
        if (!presionando)
            StartCoroutine(PresionarBoton());
    }

    IEnumerator PresionarBoton()
    {
        presionando = true;
        Vector3 destino = posicionOriginal - transform.forward * distancia;

        float t = 0f;
        while (t < duracion)
        {
            transform.localPosition = Vector3.Lerp(posicionOriginal, destino, t / duracion);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = destino;

        yield return new WaitForSeconds(0.05f);

        t = 0f;
        while (t < duracion)
        {
            transform.localPosition = Vector3.Lerp(destino, posicionOriginal, t / duracion);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = posicionOriginal;
        presionando = false;
    }
}
