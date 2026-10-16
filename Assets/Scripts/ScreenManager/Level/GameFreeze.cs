using UnityEngine;

/// <summary>
/// Called when the win / lose panel opens: freezes the game and cuts the player's
/// controls, so the only thing left to operate is the panel's buttons.
/// (Retry / Back to Menu put Time.timeScale back to 1 and reload the scene.)
/// </summary>
public static class GameFreeze
{
    public static void Freeze()
    {
        Time.timeScale = 0f;

        // Clicking Retry / Back to Menu must not fire bullets, aim or trigger the E skill
        Disable<PlayerShooting>();
        Disable<BodyMovement>();
        Disable<PlayerMovement>();
    }

    private static void Disable<T>() where T : Behaviour
    {
        foreach (var c in Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            c.enabled = false;
    }
}
