using System.Collections;
using UnityEngine;

public class BossAirstrikeSkill : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;
    public Rigidbody2D playerRb;
    public GameObject airstrikeZonePrefab;

    [Header("Cooldown")]
    public float skillCooldown = 7f;
    public float skillCounter;

    [Header("Skill Balance")]
    public float predictionTime = 1.0f;
    public int bombCount = 3;
    public float timeBetweenBombs = 0.4f;
    public float randomSpread = 0.5f;

    public bool isCalling { get; private set; }
    public bool IsReady => !isCalling && skillCounter <= 0f;

    private bool strikeStarted;

    void Awake()
    {
        skillCounter = skillCooldown;
    }

    void Update()
    {
        if (!isCalling && skillCounter > 0f)
            skillCounter -= Time.deltaTime;
    }

    // Called by BossAnimationController when IsReady -> makes Animator enter bossCalling
    public void StartCall()
    {
        isCalling = true;
        strikeStarted = false;
        skillCounter = skillCooldown;
    }

    // Called by the Animation Event in the bossCalling clip
    public void TriggerAirstrike()
    {
        if (!isCalling || strikeStarted) return; // ignore duplicate/stray events
        strikeStarted = true;
        StartCoroutine(AirstrikeRoutine());
    }

    private IEnumerator AirstrikeRoutine()
    {
        Debug.Log("Boss calling in airstrike!");
        for (int i = 0; i < bombCount; i++)
        {
            SpawnPredictedBomb();
            yield return new WaitForSeconds(timeBetweenBombs);
        }
        isCalling = false;
        strikeStarted = false;
    }

    private void SpawnPredictedBomb()
    {
        if (playerTransform == null) return;

        Vector2 velocity = playerRb != null ? playerRb.linearVelocity : Vector2.zero;
        Vector2 predictedPos = (Vector2)playerTransform.position + velocity * predictionTime;
        Vector2 finalSpawnPos = predictedPos + Random.insideUnitCircle * randomSpread;

        Instantiate(airstrikeZonePrefab, finalSpawnPos, Quaternion.identity);
    }
}