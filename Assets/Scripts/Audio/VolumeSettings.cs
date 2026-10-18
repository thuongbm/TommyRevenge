using UnityEngine;

/// <summary>
/// Master volume for the whole game (music + sound effects), saved between sessions.
/// 0 = silent, 1 = full volume. Applied automatically when the game starts.
/// </summary>
public static class VolumeSettings
{
    private const string Key = "MasterVolume";

    public static float Volume
    {
        get { return PlayerPrefs.GetFloat(Key, 1f); }
    }

    public static void Set(float value)
    {
        value = Mathf.Clamp01(value);
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(Key, value);
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ApplySavedVolume()
    {
        AudioListener.volume = Volume;
    }
}
