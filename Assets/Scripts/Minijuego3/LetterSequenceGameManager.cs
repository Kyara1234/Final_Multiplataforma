using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LetterSequenceGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sequenceText;
    [SerializeField] private float timePerLetter = 1.5f;
    [SerializeField] private int totalRounds = 10;
    [SerializeField] private float delayBetweenRounds = 2f;
    [SerializeField] private GlitchEffect glitchEffect;

    private readonly char[] vowels = { 'A', 'E', 'I', 'O' };
    private readonly List<char> currentSequence = new();
    private readonly List<char> playerInputs = new();

    private int currentRound;
    private int failCount;
    private int correctCount;
    private int failCountTotal;

    private bool isInputActive;
    private Coroutine inputTimerCoroutine;

    private void Start()
    {
        StartCoroutine(StartNextRound());
    }

    private IEnumerator StartNextRound()
    {
        if (currentRound >= totalRounds)
        {
            string finalMessage = failCountTotal >= 6
                ? "<color=red><b>Secuencias fallidas</b></color>"
                : correctCount == totalRounds
                    ? "<color=green><b>Secuencias correctas</b></color>"
                    : "<b>Felicidades!</b>";

            yield return ShowMessageAndReturn(finalMessage);
            yield break;
        }

        currentRound++;
        currentSequence.Clear();
        playerInputs.Clear();
        isInputActive = false;

        sequenceText.text = $"Ronda {currentRound}/{totalRounds}";
        yield return new WaitForSeconds(delayBetweenRounds);

        int sequenceLength = Random.Range(4, 9);
        for (int i = 0; i < sequenceLength; i++)
            currentSequence.Add(vowels[Random.Range(0, vowels.Length)]);

        DisplaySequence();
        isInputActive = true;
        inputTimerCoroutine = StartCoroutine(InputTimer(sequenceLength * timePerLetter));
    }

    private void DisplaySequence()
    {
        sequenceText.text = string.Join(" ", currentSequence.ConvertAll(c => $"<color=white>{c}</color>"));
    }

    public void OnButtonPressed(string input)
    {
        if (!isInputActive || playerInputs.Count >= currentSequence.Count) return;

        char pressed = char.ToUpper(input[0]);
        int index = playerInputs.Count;
        char expected = currentSequence[index];

        bool correct = pressed == expected;
        playerInputs.Add(pressed);

        sequenceText.text = string.Empty;
        for (int i = 0; i < currentSequence.Count; i++)
        {
            if (i < playerInputs.Count)
            {
                char entered = playerInputs[i];
                string color = entered == currentSequence[i] ? "green" : "red";
                sequenceText.text += $"<color={color}>{entered}</color> ";
            }
            else
            {
                sequenceText.text += $"<color=white>{currentSequence[i]}</color> ";
            }
        }

        if (!correct)
        {
            StopCoroutine(inputTimerCoroutine);
            isInputActive = false;
            failCount++;
            failCountTotal++;

            if (failCount >= 2)
            {
                failCount = 0;
                StartCoroutine(GlitchThenNextRound());
            }
            else
            {
                StartCoroutine(WaitBeforeNextRound(false));
            }
        }
        else if (playerInputs.Count == currentSequence.Count)
        {
            StopCoroutine(inputTimerCoroutine);
            isInputActive = false;
            correctCount++;
            failCount = 0;
            StartCoroutine(WaitBeforeNextRound(true));
        }
    }

    private IEnumerator InputTimer(float timeLimit)
    {
        float elapsed = 0f;
        while (elapsed < timeLimit)
        {
            if (!isInputActive) yield break;
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (playerInputs.Count < currentSequence.Count)
        {
            isInputActive = false;
            failCount++;
            failCountTotal++;

            if (failCount >= 2)
            {
                failCount = 0;
                StartCoroutine(GlitchThenNextRound());
            }
            else
            {
                StartCoroutine(WaitBeforeNextRound(false));
            }
        }
    }

    private IEnumerator WaitBeforeNextRound(bool wasCorrect)
    {
        sequenceText.text = wasCorrect ? "<color=green><b>CORRECT</b></color>" : "<color=red><b>ERROR</b></color>";
        yield return new WaitForSeconds(delayBetweenRounds);

        if (failCountTotal >= 6)
        {
            yield return ShowMessageAndReturn("<color=red><b>Secuencia fallida...\nReiniciando...</b></color>");
            yield break;
        }

        StartCoroutine(StartNextRound());
    }

    private IEnumerator GlitchThenNextRound()
    {
        yield return glitchEffect.PlayGlitch();
        yield return WaitBeforeNextRound(false);
    }

    private IEnumerator ShowMessageAndReturn(string message)
    {
        sequenceText.text = message;
        yield return new WaitForSeconds(2f);
        sequenceText.text = "<color=white><b>Regresando a sala...</b></color>";
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("EscenaPrincipal");
    }

    private void RestartGame()
    {
        currentRound = 0;
        correctCount = 0;
        failCount = 0;
        failCountTotal = 0;
        StartCoroutine(StartNextRound());
    }
}