using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Vector3 shootDir;
    [SerializeField] private float speed;

    public void SetUp(Vector3 shootDir)
    {
        this.shootDir = shootDir;
    }

    void Update()
    {
        transform.position = shootDir * speed * Time.deltaTime;
    }
}
