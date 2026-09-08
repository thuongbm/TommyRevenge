using System;
using UnityEngine;

public class LegAnimationController : MonoBehaviour
{
    [SerializeField] private Animator legsAnimation;
    [SerializeField] private Transform legsTransform;
    [SerializeField] private float rotationSpeed = 50f;

    [Header("Sound")]
    public AudioClip walkingSound;

    private AudioSource audioSource;

    private static readonly int isRunningHash = Animator.StringToHash("isRunning");

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = walkingSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        Vector2 movement = PlayerMovement.Instance.movementInput;
        bool isMoving = movement.sqrMagnitude > 0.01f;

        legsAnimation.SetBool(isRunningHash, isMoving);

        if (isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

            legsTransform.rotation = Quaternion.RotateTowards(
                legsTransform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}