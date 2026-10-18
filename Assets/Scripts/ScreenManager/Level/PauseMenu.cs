using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Press ESC during a level to open the pause menu: Resume / Play Again / Back to Menu.
/// Press ESC again (or click Resume) to continue.
///
/// Nothing to set up: this spawns itself automatically (it builds its own UI) and works in
/// every gameplay scene. It stays asleep in the main menu scene ("0").
/// </summary>
public class PauseMenu : MonoBehaviour
{
    private const string MenuScene = "0";

    private static PauseMenu instance;

    private GameObject root;
    private GameObject helperEventSystem;
    private readonly List<Behaviour> frozen = new List<Behaviour>();

    private bool paused;
    private bool prevCursorVisible;
    private CursorLockMode prevCursorLock;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (instance != null) return;
        var go = new GameObject("PauseMenu");
        DontDestroyOnLoad(go);
        instance = go.AddComponent<PauseMenu>();
    }

    private void Awake()
    {
        BuildUI();
        root.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ForceClose();
    }

    private static bool IsGameplayScene()
    {
        string n = SceneManager.GetActiveScene().name;
        return n != MenuScene && n != "SampleScene";
    }

    private void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;
        if (!IsGameplayScene()) return;

        if (paused) Resume();
        else Pause();
    }

    // ------------------------------------------------------------------ pause / resume

    private void Pause()
    {
        // Time already stopped = win / lose panel (or something else) owns the screen.
        if (Time.timeScale == 0f) return;

        paused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;

        // Same idea as GameFreeze, but remember what we turned off so Resume can turn it back on
        Freeze<PlayerShooting>();
        Freeze<BodyMovement>();
        Freeze<PlayerMovement>();

        prevCursorVisible = Cursor.visible;
        prevCursorLock = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        EnsureEventSystem();
        root.SetActive(true);
    }

    private void Resume()
    {
        if (!paused) return;

        paused = false;
        root.SetActive(false);
        DestroyHelperEventSystem();

        foreach (var b in frozen)
            if (b != null) b.enabled = true;
        frozen.Clear();

        Time.timeScale = 1f;
        AudioListener.pause = false;
        Cursor.visible = prevCursorVisible;
        Cursor.lockState = prevCursorLock;
    }

    // Called when a new scene loads: the old scene's objects are gone, just clean up.
    private void ForceClose()
    {
        bool wasPaused = paused;
        paused = false;
        frozen.Clear();
        if (root != null) root.SetActive(false);
        DestroyHelperEventSystem();
        AudioListener.pause = false;
        if (wasPaused)
        {
            Time.timeScale = 1f;
            Cursor.visible = prevCursorVisible;
            Cursor.lockState = prevCursorLock;
        }
    }

    private void Freeze<T>() where T : Behaviour
    {
        foreach (var c in FindObjectsByType<T>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (!c.enabled) continue;
            c.enabled = false;
            frozen.Add(c);
        }
    }

    // ------------------------------------------------------------------ button actions

    private void PlayAgain()
    {
        LevelLoader.LoadTheLevel(SceneManager.GetActiveScene().name, () =>
        {
            // Reset the run (kill counter, timer, score, combo) right before reloading
            LevelStatus.ResetRun();
            LevelManager.TotalEnemies(LevelManager.totalEnemies, LevelManager.parTime);
        });
    }

    private void BackToMenu()
    {
        LevelLoader.LoadTheLevel(MenuScene);
    }

    // ------------------------------------------------------------------ event system

    private void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null) return;

        helperEventSystem = new GameObject("PauseEventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        helperEventSystem.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
    }

    private void DestroyHelperEventSystem()
    {
        if (helperEventSystem != null) Destroy(helperEventSystem);
        helperEventSystem = null;
    }

    // ------------------------------------------------------------------ UI (built from code)

    private void BuildUI()
    {
        root = new GameObject("PauseCanvas");
        root.transform.SetParent(transform, false);

        var canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 500;

        var scaler = root.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        root.AddComponent<GraphicRaycaster>();

        var dim = NewImage("Dim", root.transform, new Color(0f, 0f, 0f, 0.75f));
        Stretch(dim.rectTransform);

        var title = NewText("Title", dim.transform, "PAUSED", 110, FontStyles.Bold);
        title.rectTransform.anchoredPosition = new Vector2(0f, 300f);
        title.rectTransform.sizeDelta = new Vector2(1000f, 160f);

        MakeButton(dim.transform, "RESUME", new Vector2(0f, 110f), Resume);
        MakeButton(dim.transform, "PLAY AGAIN", new Vector2(0f, -30f), PlayAgain);
        MakeButton(dim.transform, "BACK TO MENU", new Vector2(0f, -170f), BackToMenu);

        var hint = NewText("Hint", dim.transform, "Press ESC to resume", 34, FontStyles.Normal);
        hint.color = new Color(1f, 1f, 1f, 0.6f);
        hint.rectTransform.anchoredPosition = new Vector2(0f, -330f);
        hint.rectTransform.sizeDelta = new Vector2(1000f, 60f);
    }

    private static Image NewImage(string name, Transform parent, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent, false);
        var img = go.GetComponent<Image>();
        img.color = color;
        var rt = img.rectTransform;
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        return img;
    }

    private static TextMeshProUGUI NewText(string name, Transform parent, string text, float size, FontStyles style)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        var t = go.GetComponent<TextMeshProUGUI>();
        t.text = text;
        t.fontSize = size;
        t.fontStyle = style;
        t.alignment = TextAlignmentOptions.Center;
        t.color = Color.white;
        t.raycastTarget = false;
        var rt = t.rectTransform;
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        return t;
    }

    private static void MakeButton(Transform parent, string label, Vector2 pos, UnityAction onClick)
    {
        var img = NewImage(label + " Button", parent, Color.white);
        img.rectTransform.sizeDelta = new Vector2(560f, 110f);
        img.rectTransform.anchoredPosition = pos;

        var btn = img.gameObject.AddComponent<Button>();
        btn.targetGraphic = img;
        var colors = btn.colors;
        colors.normalColor = new Color(0.16f, 0.16f, 0.2f, 1f);
        colors.highlightedColor = new Color(0.38f, 0.38f, 0.5f, 1f);
        colors.selectedColor = colors.highlightedColor;
        colors.pressedColor = new Color(0.25f, 0.25f, 0.35f, 1f);
        btn.colors = colors;
        btn.onClick.AddListener(onClick);

        var txt = NewText("Label", img.transform, label, 52, FontStyles.Bold);
        Stretch(txt.rectTransform);
    }

    private static void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
