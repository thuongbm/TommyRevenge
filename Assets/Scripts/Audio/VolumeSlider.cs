using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Put this on a UI Slider: it controls the master volume (see VolumeSettings).
/// The slider starts at the saved volume; dragging changes the sound immediately.
/// </summary>
[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [Tooltip("Optional label that shows the volume as a percentage")]
    [SerializeField] private TMPro.TMP_Text percentText;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.wholeNumbers = false;
    }

    private void OnEnable()
    {
        slider.SetValueWithoutNotify(VolumeSettings.Volume);
        Refresh();
        slider.onValueChanged.AddListener(OnChanged);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnChanged);
        VolumeSettings.Save();
    }

    private void OnChanged(float value)
    {
        VolumeSettings.Set(value);
        Refresh();
    }

    private void Refresh()
    {
        if (percentText != null)
            percentText.text = Mathf.RoundToInt(slider.value * 100f) + "%";
    }
}
