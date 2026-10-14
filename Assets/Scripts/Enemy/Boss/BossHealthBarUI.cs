using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Shows the boss health at the top of the screen. Listens to BossHealth events.
public class BossHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private RectTransform fill;      // pivot X = 0 (left) so it shrinks to the left
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("Look")]
    [SerializeField] private string bossName = "PINKMAN";
    [SerializeField] private Color fullColor = new Color(0.9f, 0.15f, 0.15f, 1f);
    [SerializeField] private Color lowColor  = new Color(0.55f, 0f, 0f, 1f);
    [SerializeField] private float smoothSpeed = 8f;          // higher = snappier
    [SerializeField] private float hideDelayAfterDeath = 1.5f;

    private float targetPercent = 1f;
    private float shownPercent = 1f;

    void Start()
    {
        if (nameText != null) nameText.text = bossName;

        if (bossHealth == null)
        {
            Debug.LogWarning("BossHealthBarUI: bossHealth is not assigned.", this);
            return;
        }

        bossHealth.OnHealthChanged += HandleHealthChanged;
        bossHealth.OnDied += HandleDied;

        // Start in the correct state (BossHealth already set its health in Awake)
        HandleHealthChanged(bossHealth.CurrentHealth, bossHealth.MaxHealth);
        shownPercent = targetPercent;
        Apply();
    }

    void OnDestroy()
    {
        if (bossHealth == null) return;
        bossHealth.OnHealthChanged -= HandleHealthChanged;
        bossHealth.OnDied -= HandleDied;
    }

    void Update()
    {
        // Smooth drain instead of jumping
        float t = 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime);
        shownPercent = Mathf.Lerp(shownPercent, targetPercent, t);
        Apply();
    }

    private void HandleHealthChanged(int current, int max)
    {
        targetPercent = max > 0 ? Mathf.Clamp01((float)current / max) : 0f;
    }

    private void HandleDied()
    {
        targetPercent = 0f;
        Invoke(nameof(Hide), hideDelayAfterDeath);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Apply()
    {
        if (fill != null) fill.localScale = new Vector3(shownPercent, 1f, 1f);
        if (fillImage != null) fillImage.color = Color.Lerp(lowColor, fullColor, shownPercent);
    }
}
