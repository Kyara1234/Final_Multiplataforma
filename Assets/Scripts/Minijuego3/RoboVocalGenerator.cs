using UnityEngine;

public class RoboVocalGenerator : MonoBehaviour
{
    [SerializeField] private GameObject vowelSoundPrefab;

    public void PlayVowelFromString(string vowel)
    {
        if (string.IsNullOrEmpty(vowel)) return;

        char v = char.ToUpper(vowel[0]);
        float freq = v switch
        {
            'A' => 440f,
            'E' => 660f,
            'I' => 880f,
            'O' => 330f,
            _ => 0f
        };

        if (freq == 0f) return;

        GameObject go = Instantiate(vowelSoundPrefab);
        go.GetComponent<RoboVowelSound>().Initialize(freq);
    }
}