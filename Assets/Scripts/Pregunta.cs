[System.Serializable]
public class Pregunta
{
    public string textoCorrupto;
    public string respuestaCorrecta;
    public string[] respuestasIncorrectas;

    public Pregunta(string texto, string correcta, string[] incorrectas)
    {
        textoCorrupto = texto;
        respuestaCorrecta = correcta;
        respuestasIncorrectas = incorrectas;
    }
}
