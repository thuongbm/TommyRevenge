using UnityEngine;
using TMPro;
using System.Collections;

public class ComboPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private RectTransform comboTimerBarFill;
    [SerializeField] private float popScale = 1.4f;
    [SerializeField] private float popDuration = 0.25f;

    private Coroutine popRoutine;

    void OnEnable()
    {
        ComboManager.OnComboIncreased += HandleComboIncreased;
        ComboManager.OnComboBroken += HandleComboBroken;
        if (comboText != null) comboText.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        ComboManager.OnComboIncreased -= HandleComboIncreased;
        ComboManager.OnComboBroken -= HandleComboBroken;
    }

    void HandleComboIncreased(int multiplier)
    {
        if (comboText == null) return;

        if (multiplier < 2)
        {
            comboText.gameObject.SetActive(false);
            return;
        }

        comboText.gameObject.SetActive(true);
        comboText.text = "COMBO x" + multiplier + "!";

        if (popRoutine != null) StopCoroutine(popRoutine);
        popRoutine = StartCoroutine(PopAnim());
    }

    void HandleComboBroken()
    {
        if (comboText != null) comboText.gameObject.SetActive(false);
    }

    IEnumerator PopAnim()
    {
        float t = 0f;
        comboText.rectTransform.localScale = Vector3.one * popScale;
        while (t < popDuration)
        {
            t += Time.deltaTime;
            float s = Mathf.Lerp(popScale, 1f, t / popDuration);
            comboText.rectTransform.localScale = Vector3.one * s;
            yield return null;
        }
        comboText.rectTransform.localScale = Vector3.one;
    }

    void Update()
    {
        if (comboTimerBarFill != null && ComboManager.Instance != null)
        {
            float pct = Mathf.Clamp01(ComboManager.Instance.ComboTimeRemaining / ComboManager.Instance.ComboWindow);
            Vector3 scale = comboTimerBarFill.localScale;
            scale.x = pct;
            comboTimerBarFill.localScale = scale;
        }
    }
}
