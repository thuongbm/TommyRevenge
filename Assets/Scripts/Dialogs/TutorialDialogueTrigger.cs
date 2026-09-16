using UnityEngine;

/// <summary>
/// Tự động kích hoạt hộp thoại hướng dẫn khi scene Chapter1_Tutorial mở ra.
/// Gắn script này vào một GameObject trong scene và assign DialogueData phù hợp.
/// </summary>
public class TutorialDialogueTrigger : MonoBehaviour
{
    [Header("Tutorial Dialogue")]
    [Tooltip("ScriptableObject chứa nội dung hộp thoại hướng dẫn")]
    public DialogueData tutorialDialogueData;

    [Tooltip("Chỉ hiện hộp thoại nếu người chơi chưa xem (tùy chọn)")]
    public bool showOnlyOnce = true;

    private const string TutorialSeenKey = "TutorialSeen_Chapter1";

    void Start()
    {
        if (tutorialDialogueData == null)
        {
            Debug.LogWarning("[TutorialDialogueTrigger] Chưa gán DialogueData!");
            return;
        }

        if (showOnlyOnce && PlayerPrefs.GetInt(TutorialSeenKey, 0) == 1)
        {
            // Người chơi đã xem rồi, bỏ qua
            return;
        }

        // Trì hoãn nhỏ để đảm bảo DialogueManager đã Awake()
        Invoke(nameof(TriggerTutorial), 0.1f);
    }

    private void TriggerTutorial()
    {
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("[TutorialDialogueTrigger] Không tìm thấy DialogueManager trong scene!");
            return;
        }

        DialogueManager.Instance.StartDialogue(tutorialDialogueData, OnTutorialComplete);
    }

    private void OnTutorialComplete()
    {
        if (showOnlyOnce)
        {
            PlayerPrefs.SetInt(TutorialSeenKey, 1);
            PlayerPrefs.Save();
        }

        Debug.Log("[TutorialDialogueTrigger] Hộp thoại hướng dẫn đã hoàn thành.");
    }

    /// <summary>
    /// Gọi hàm này để reset trạng thái đã xem tutorial (dùng cho Debug / Settings).
    /// </summary>
    public void ResetTutorialSeen()
    {
        PlayerPrefs.DeleteKey(TutorialSeenKey);
    }
}
