using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float lifetime = 10;
    private float createdAt;

    public Vector3 velocity;

    private void Start()
    {
        createdAt = Time.time;
    }

    private void Update()
    {
        if (Time.time - createdAt > lifetime)
            Destroy(gameObject);
        transform.position += Time.deltaTime * velocity;
    }
}
