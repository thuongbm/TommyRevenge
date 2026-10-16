using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shield HUD for the immortal skill (3 states):
/// - Ready:    full bright shield
/// - Active:   shield drains top -> bottom (the short immortal window)
/// - Cooldown: dim shield refills bottom -> top until the skill is ready again
/// </summary>
public class ImmortalHUD : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image backgroundImage;

    [Header("Colors")]
    [SerializeField] private Color readyColor = Color.white;
    [SerializeField] private Color activeColor = new Color(0.3f, 0.9f, 1f, 1f);
    [SerializeField] private Color cooldownColor = new Color(1f, 1f, 1f, 0.45f);

    void Update()
    {
        var body = BodyMovement.Instance;
        if (fillImage == null || body == null) return;

        if (body.isImmortal)
        {
            fillImage.color = activeColor;
            fillImage.fillAmount = body.ImmortalRemaining01;
        }
        else if (body.IsReady)
        {
            fillImage.color = readyColor;
            fillImage.fillAmount = 1f;
        }
        else
        {
            fillImage.color = cooldownColor;
            fillImage.fillAmount = 1f - body.CooldownRemaining01;
        }
    }
}
