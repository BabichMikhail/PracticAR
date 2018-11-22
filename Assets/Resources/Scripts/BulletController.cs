using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float lifetime = 10;
    private float createdAt;

    public Vector3 velocity;
    public float Speed = 1;
    public int Damage = 1;
    public GameObject creator;

    private void Start()
    {
        createdAt = Time.time;
    }

    private void Update()
    {
        if (Time.time - createdAt > lifetime)
            Destroy(gameObject);
        transform.position += Time.deltaTime * velocity * Speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == creator)
            return;
        var healthController = other.gameObject.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.HealthPoints -= Damage;
            Destroy(gameObject);
        }
    }
}
