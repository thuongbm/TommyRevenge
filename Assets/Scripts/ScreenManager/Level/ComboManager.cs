using UnityEngine;
using System;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    [SerializeField] private float comboWindow = 3f;
    [SerializeField] private int baseKillScore = 100;

    private static readonly Color[] PopupPalette = new Color[]
    {
        new Color(1f, 0.15f, 0.75f),
        new Color(0.1f, 0.9f, 0.75f),
        new Color(1f, 0.85f, 0.1f),
        new Color(1f, 0.45f, 0.1f),
    };
    private int popupColorIndex = 0;


    public int ComboMultiplier { get; private set; } = 1;
    public float ComboTimeRemaining { get; private set; } = 0f;
    public float ComboWindow => comboWindow;

    public static event Action<int> OnComboIncreased;
    public static event Action OnComboBroken;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        if (ComboTimeRemaining > 0f)
        {
            ComboTimeRemaining -= Time.deltaTime;
            if (ComboTimeRemaining <= 0f)
            {
                BreakCombo();
            }
        }
    }

public void RegisterKill(Vector3 killPosition)
    {
        if (ComboTimeRemaining > 0f)
        {
            ComboMultiplier++;
        }
        else
        {
            ComboMultiplier = 1;
        }

        ComboTimeRemaining = comboWindow;

        int points = baseKillScore * ComboMultiplier;
        ScoreManager.AddScore(points);

        Color popupColor = PopupPalette[popupColorIndex % PopupPalette.Length];
        popupColorIndex++;
        ScorePopup.Create(killPosition, points, popupColor);
        OnComboIncreased?.Invoke(ComboMultiplier);
    }

    private void BreakCombo()
    {
        ComboMultiplier = 1;
        ComboTimeRemaining = 0f;
        OnComboBroken?.Invoke();
    }

    public void ResetCombo()
    {
        ComboMultiplier = 1;
        ComboTimeRemaining = 0f;
    }
}
