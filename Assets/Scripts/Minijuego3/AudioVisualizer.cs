using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioVisualizer : MonoBehaviour
{
    [SerializeField] private RectTransform targetImage;
    [SerializeField] private float scaleAmount = 3f;
    [SerializeField] private float scaleDuration = 0.5f;

    private Vector3 originalScale;

    private void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<RectTransform>();

        originalScale = targetImage.localScale;
    }

    public void AnimateScale()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleUpDown());
    }

    private IEnumerator ScaleUpDown()
    {
        Vector3 targetScale = new Vector3(originalScale.x, scaleAmount, originalScale.z);
        float elapsed = 0f;

        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            targetImage.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / scaleDuration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            targetImage.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / scaleDuration);
            yield return null;
        }

        targetImage.localScale = originalScale;
    }
}