using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game music. Lives for the whole game (DontDestroyOnLoad) so songs don't restart between scenes.
///
///  - Main menu scene "0" (menu, choose chapter, choose level): loops the menu music.
///  - Any gameplay scene: plays the playlist in order (song 1 -> song 2 -> song 1 ...) forever.
///  - Retrying a level keeps the playlist going; going back to the menu switches to menu music.
///
/// It spawns itself from the prefab Assets/Resources/MusicManager.prefab - no scene setup needed.
/// Change songs / volumes by editing that prefab.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private const string MenuScene = "0";
    private const string PrefabName = "MusicManager";

    public static MusicManager Instance { get; private set; }

    [Header("Menu / Choose Chapter / Choose Level")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField, Range(0f, 1f)] private float menuVolume = 0.5f;

    [Header("In-game playlist (plays in order, then repeats)")]
    [SerializeField] private AudioClip[] playingMusic;
    [SerializeField, Range(0f, 1f)] private float playingVolume = 0.4f;

    private enum Mode { None, Menu, Playing }

    private AudioSource source;
    private Mode mode = Mode.None;
    private int trackIndex;
    private float lastTime;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (Instance != null) return;

        var prefab = Resources.Load<GameObject>(PrefabName);
        if (prefab == null)
        {
            Debug.LogWarning("MusicManager: Assets/Resources/" + PrefabName + ".prefab not found - no music.");
            return;
        }

        var go = Instantiate(prefab);
        go.name = PrefabName;
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0f;                // plain 2D music
        source.ignoreListenerPause = true;       // keep playing while the pause menu is open

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (Instance == this) ApplyScene(SceneManager.GetActiveScene());
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        ApplyScene(scene);
    }

    private void ApplyScene(Scene scene)
    {
        if (scene.name == "SampleScene") return;
        SwitchTo(scene.name == MenuScene ? Mode.Menu : Mode.Playing);
    }

    private void SwitchTo(Mode newMode)
    {
        if (newMode == mode) return;   // already on the right music (menu panels, level retry...)
        mode = newMode;

        if (mode == Mode.Menu)
        {
            if (menuMusic == null) { source.Stop(); return; }
            source.clip = menuMusic;
            source.volume = menuVolume;
            source.loop = true;
            source.Play();
        }
        else
        {
            if (playingMusic == null || playingMusic.Length == 0) { source.Stop(); return; }
            trackIndex = 0;
            PlayTrack();
        }
    }

private void PlayTrack()
    {
        var clip = playingMusic[trackIndex];
        if (clip == null) return;

        lastTime = 0f;
        source.clip = clip;
        source.volume = playingVolume;
        source.loop = false;
        source.Play();
    }

private void Update()
    {
        if (mode != Mode.Playing || playingMusic == null || playingMusic.Length == 0) return;

        if (source.isPlaying)
        {
            lastTime = source.time;   // remember how far into the song we got
            return;
        }

        // Not playing AND we were at the very end of the song = it finished -> next song,
        // wrapping back to the first one (round and round). Works even if the window lost focus.
        if (source.clip != null && lastTime >= source.clip.length - 1.5f)
        {
            trackIndex = (trackIndex + 1) % playingMusic.Length;
            PlayTrack();
        }
    }
}
