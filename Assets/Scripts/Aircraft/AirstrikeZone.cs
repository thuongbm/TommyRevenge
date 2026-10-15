using System;
using System.Collections;
using UnityEngine;

public class AirstrikeZone : MonoBehaviour
{
    [Header("Settings")]
    public float strikeDelay = 1.2f;       // Time before bomb hits
    public float explosionRadius = 1.5f;   // Damage hitbox size
    public int damage = 50;
    public LayerMask playerLayer;

    [Header("Visuals & Audio")]
    public GameObject explosionVFX;        // Particle system prefab
    public GameObject aircraftShadowPrefab;// Optional: Shadow flying across screen
    public SpriteRenderer warningSprite;

    public AudioClip bloodSplashSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip aircraftlying;

    private void Start()
    {
        StartCoroutine(ExecuteStrike());
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator ExecuteStrike()
    {
        // Optional: Spawn aircraft shadow flying over this position
        if (aircraftShadowPrefab != null)
        {
            // Jet enters from the right edge of the camera, flies right -> left
            Camera cam = Camera.main;
            float startX = transform.position.x + 20f;
            float despawnX = transform.position.x - 20f;
            if (cam != null)
            {
                startX = cam.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, 0f)).x;
                despawnX = cam.ViewportToWorldPoint(new Vector3(-0.1f, 0.5f, 0f)).x;
            }

            // Jet passes over the target x exactly when the bomb explodes
            float flySpeed = Mathf.Max(1f, (startX - transform.position.x) / Mathf.Max(0.1f, strikeDelay));
            GameObject jet = Instantiate(aircraftShadowPrefab, new Vector3(startX, transform.position.y, transform.position.z), Quaternion.identity);
            AircraftFlyBy fly = jet.GetComponent<AircraftFlyBy>();
            if (fly == null) fly = jet.AddComponent<AircraftFlyBy>();
            fly.Init(flySpeed, despawnX);
            audioSource.PlayOneShot(aircraftlying);
        }

        // 1. Telegraph phase: Flash or scale the warning circle
        float timer = 0f;
        Vector3 initialScale = transform.localScale;
        
        while (timer < strikeDelay)
        {
            timer += Time.deltaTime;
            // Pulse effect (gets faster or brighter as timer runs out)
            float alpha = Mathf.PingPong(timer * 5f, 0.6f) + 0.2f;
            if (warningSprite != null)
            {
                Color c = warningSprite.color;
                c.a = alpha;
                warningSprite.color = c;
            }
            yield return null;
        }

        // 2. Boom phase: Spawn VFX
        if (explosionVFX != null)
        {
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 1.5f); // destroy the spawned copy, NOT the prefab asset
        }

        // 3. Check Hitbox (Use Physics.OverlapSphere if in 3D)
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayer);
        if (hitPlayer != null)
        {
            BodyMovement.Instance.isDead = true;
            BloodPlayerManage.Instance.BloodSpalsh();

            if (bloodSplashSound != null)
            {
                AudioSource.PlayClipAtPoint(bloodSplashSound, transform.position);
            }
        }

        // 4. Clean up the warning zone
        Destroy(gameObject);
    }

    // Draw the explosion radius in the Editor so you can match it to your sprite size
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
