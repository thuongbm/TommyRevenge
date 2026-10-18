using UnityEngine;

/// <summary>
/// The volume screen opened by the "Volume" button on the main menu.
/// Put it on the volume panel: Open() shows it, Close() hides it (same fade as the other menu screens).
/// </summary>
public class VolumeMenu : MonoBehaviour
{
    public void Open()
    {
        LevelLoader.PlayTransition(() => gameObject.SetActive(true));
    }

    public void Close()
    {
        LevelLoader.PlayTransition(() => gameObject.SetActive(false));
    }
}
