using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float lifetime = 1.2f;
    [SerializeField] private float popScale = 1.5f;
    [SerializeField] private float popDuration = 0.12f;

    private TextMeshPro tmp;
    private float timer;
    private Color baseColor;
    private Vector3 floatDir;

    public static ScorePopup Create(Vector3 worldPosition, int points, Color color)
    {
        var go = new GameObject("ScorePopup");

        float ox = Random.Range(-0.4f, 0.4f);
        float oy = Random.Range(-0.2f, 0.3f);
        go.transform.position = worldPosition + new Vector3(ox, oy, 0f);

        float tilt = Random.Range(-20f, 20f);
        go.transform.rotation = Quaternion.Euler(0f, 0f, tilt);

        var tmp = go.AddComponent<TextMeshPro>();

        // Assign font để text không bị invisible khi tạo dynamic
        var fontAsset = Resources.Load<TMPro.TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        if (fontAsset == null)
            fontAsset = TMPro.TMP_Settings.defaultFontAsset;
        if (fontAsset != null)
            tmp.font = fontAsset;

        tmp.text = "+" + points + "PTS";
        tmp.fontSize = 5.5f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = color;
        tmp.fontStyle = FontStyles.Bold;
        tmp.outlineWidth = 0.25f;
        tmp.outlineColor = Color.black;

        var renderer = go.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = "Wall";
            renderer.sortingOrder = 200;
        }


        var popup = go.AddComponent<ScorePopup>();
        popup.tmp = tmp;
        popup.baseColor = color;
        popup.transform.localScale = Vector3.zero;

        float angle = Random.Range(60f, 120f) * Mathf.Deg2Rad;
        popup.floatDir = new Vector3(Mathf.Cos(angle) * 0.5f, Mathf.Sin(angle), 0f).normalized;
        return popup;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Pop scale animation
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

        // Float lên
        transform.position += floatDir * floatSpeed * Time.deltaTime;

        // Fade out
        float fadeStart = lifetime * 0.5f;
        if (tmp != null && timer > fadeStart)
        {
            float fadeT = Mathf.Clamp01((timer - fadeStart) / (lifetime - fadeStart));
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
