using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GlitchEffect : MonoBehaviour
{
    [SerializeField] private Image glitchImage;
    [SerializeField] private float glitchDuration = 1f;
    [SerializeField] private float shakeIntensity = 10f;

    private Vector3 originalPosition;

    private void Start()
    {
        glitchImage.gameObject.SetActive(true);
        glitchImage.color = new Color(1, 0, 0, 0.8f);
        originalPosition = glitchImage.rectTransform.localPosition;
    }

    public IEnumerator PlayGlitch()
    {
        glitchImage.gameObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < glitchDuration)
        {
            elapsed += Time.deltaTime;

            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);
            glitchImage.rectTransform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
            glitchImage.color = new Color(1, 1, 1, Random.Range(0.3f, 0.8f));

            yield return new WaitForSeconds(0.05f);
        }

        glitchImage.rectTransform.localPosition = originalPosition;
        glitchImage.color = new Color(1, 1, 1, 0);
        glitchImage.gameObject.SetActive(false);
    }
}