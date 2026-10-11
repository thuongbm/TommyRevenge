using UnityEngine;

public class BossAttacking : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float timeBetweenAttack = 1f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator animator;

    [Header("Attack sound")]
    [SerializeField] private AudioClip swordSound;
    private AudioSource audioSource;

    private float attackCoolDown;

    public bool isAttack = false;
    private FieldOfView2D fov;

    void Awake()
    {
        fov = GetComponent<FieldOfView2D>();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
        {
            if (attackCoolDown > 0)
            {
                attackCoolDown -= Time.deltaTime;
                
            }
                

            isAttack = InRange();
            if (fov != null && fov.canSeePlayer && InRange() && attackCoolDown <= 0f)
            {
                attackCoolDown = timeBetweenAttack;
            }
            
        }

        private bool InRange()
        {
            return playerTransform != null && !BodyMovement.Instance.isDead &&
                Vector2.Distance(transform.position, playerTransform.position) <= attackRange;
        }

        public void AttackNormal()
        {
            Debug.Log("Attacked");
            BodyMovement.Instance.isDead = true;
            audioSource.PlayOneShot(swordSound);
            BloodPlayerManage.Instance.BloodSpalsh();
        }
}
