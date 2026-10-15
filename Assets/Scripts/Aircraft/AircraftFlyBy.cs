using System;
using Unity.VisualScripting;
using UnityEngine;

// Moves the aircraft from right to left, then destroys it once it is off-screen.
public class AircraftFlyBy : MonoBehaviour
{
    public float speed = 25f;       // world units / second
    public float maxLifeTime = 8f;  // safety net

    private float despawnX = float.NegativeInfinity;

    public void Init(float newSpeed, float despawnAtX)
    {
        speed = newSpeed;
        despawnX = despawnAtX;
    }

    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < despawnX)
            Destroy(gameObject);
    }
}
