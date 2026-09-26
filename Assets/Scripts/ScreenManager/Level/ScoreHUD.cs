using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float shakeMagnitude = 8f;
    [SerializeField] private float shakeDuration = 0.25f;
    [SerializeField] private float punchScale = 1.3f;

    private RectTransform rt;
    private Vector2 basePos;
    private Coroutine shakeRoutine;

    void Awake()
    {
        if (scoreText != null)
        {
            rt = scoreText.rectTransform;
            basePos = rt.anchoredPosition;
        }
    }

    void OnEnable()
    {
        ComboManager.OnComboIncreased += HandleComboIncreased;
        UpdateScoreText();
    }

    void OnDisable()
    {
        ComboManager.OnComboIncreased -= HandleComboIncreased;
    }

    void HandleComboIncreased(int multiplier)
    {
        UpdateScoreText();

        if (multiplier >= 2)
        {
            if (shakeRoutine != null) StopCoroutine(shakeRoutine);
            shakeRoutine = StartCoroutine(ShakeAndPunch());
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + ScoreManager.TotalScore;
        }
    }

    IEnumerator ShakeAndPunch()
    {
        float t = 0f;
        while (t < shakeDuration)
        {
            t += Time.deltaTime;
            float remaining = 1f - (t / shakeDuration);

            Vector2 shakeOffset = Random.insideUnitCircle * shakeMagnitude * remaining;
            rt.anchoredPosition = basePos + shakeOffset;

            float scale = Mathf.Lerp(punchScale, 1f, t / shakeDuration);
            rt.localScale = Vector3.one * scale;

            yield return null;
        }

        rt.anchoredPosition = basePos;
        rt.localScale = Vector3.one;
    }
}
