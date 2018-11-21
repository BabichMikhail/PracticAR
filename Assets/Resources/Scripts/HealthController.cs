using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int HealthPoints = 100;

    private bool destroyed = false;

    private void Start ()
    {
        Debug.Assert(HealthPoints > 0);
    }

    private void Update ()
    {
        if (destroyed)
            return;
        if (HealthPoints <= 0)
        {
            destroyed = true;
            Destroy(gameObject, 0.1f);
            return;
        }
    }
}
