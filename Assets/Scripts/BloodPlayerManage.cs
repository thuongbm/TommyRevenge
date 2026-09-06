using Unity.VisualScripting;
using UnityEngine;

public class BloodPlayerManage : MonoBehaviour
{
    public static BloodPlayerManage Instance { get; set; }
    [SerializeField] private GameObject blood;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    public void BloodSpalsh()
    {
        if (BodyMovement.Instance.isDead)
        {
            Debug.Log("Dead");
            GameObject bloodInstance = Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(bloodInstance, 1.0f);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject, 1.0f);
    }
}
