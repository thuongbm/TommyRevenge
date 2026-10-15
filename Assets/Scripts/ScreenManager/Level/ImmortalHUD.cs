using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shield HUD for the immortal skill.
/// - backgroundImage: dim shield, always visible (shows the skill exists)
/// - fillImage:       bright shield, drains from top to bottom while the skill is active
/// </summary>
public class ImmortalHUD : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Image backgroundImage;

    void Update()
    {
        if (fillImage == null || BodyMovement.Instance == null) return;
        fillImage.fillAmount = BodyMovement.Instance.ImmortalRemaining01;
    }
}
