using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RoboVowelSound : MonoBehaviour
{
    private AudioSource audioSource;
    private float frequency;
    private float increment;
    private float phase;
    private float sampleRate;

    public void Initialize(float freq)
    {
        frequency = freq;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        sampleRate = AudioSettings.outputSampleRate;
        audioSource.Play();
        Invoke(nameof(DestroySelf), 0.3f);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2 * Mathf.PI / sampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            float sample = Mathf.Sin(phase) > 0 ? 0.5f : -0.5f;
            float mod = Mathf.Sin(phase * 0.25f) * 0.3f;
            float output = (sample + mod) * 0.3f;

            for (int j = 0; j < channels; j++)
                data[i + j] = output;

            phase += increment;
            if (phase > 2 * Mathf.PI) phase -= 2 * Mathf.PI;
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}