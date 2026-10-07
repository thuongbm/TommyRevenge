using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private float lifetime = 0.9f;
    [SerializeField] private float popScale = 1.3f;
    [SerializeField] private float popDuration = 0.15f;

    private TextMeshPro tmp;
    private float timer;
    private Color baseColor;

    public static ScorePopup Create(Vector3 worldPosition, int points, Color color)
    {
        var go = new GameObject("ScorePopup");
        go.transform.position = worldPosition;

        var tmp = go.AddComponent<TextMeshPro>();
        tmp.text = "+" + points + "PTS";
        tmp.fontSize = 4;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = color;
        tmp.fontStyle = FontStyles.Bold;
        tmp.outlineWidth = 0.2f;
        tmp.outlineColor = Color.black;
        tmp.sortingOrder = 100;

        var popup = go.AddComponent<ScorePopup>();
        popup.tmp = tmp;
        popup.baseColor = color;
        popup.transform.localScale = Vector3.zero;
        return popup;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer <= popDuration)
        {
            float t = timer / popDuration;
            transform.localScale = Vector3.one * Mathf.Lerp(0f, popScale, t);
        }
        else
        {
            float settleT = Mathf.Clamp01((timer - popDuration) / 0.1f);
            transform.localScale = Vector3.one * Mathf.Lerp(popScale, 1f, settleT);
        }

        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        float fadeT = Mathf.Clamp01(timer / lifetime);
        if (tmp != null)
        {
            Color c = baseColor;
            c.a = Mathf.Lerp(1f, 0f, fadeT);
            tmp.color = c;
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
