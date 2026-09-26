using UnityEngine;
using System;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    [SerializeField] private float comboWindow = 3f;
    [SerializeField] private int baseKillScore = 100;

    public int ComboMultiplier { get; private set; } = 1;
    public float ComboTimeRemaining { get; private set; } = 0f;
    public float ComboWindow => comboWindow;

    public static event Action<int> OnComboIncreased;
    public static event Action OnComboBroken;

    void Awake()
    {
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

    public void RegisterKill()
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
        ScoreManager.AddScore(baseKillScore * ComboMultiplier);
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
