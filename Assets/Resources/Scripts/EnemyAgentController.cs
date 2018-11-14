using UnityEngine;

public class EnemyAgentController : MonoBehaviour
{
    public int Health = 100;
    public int Radius = 5;
    public int Damage = 1;
    public int FireRate = 5;

    private GameObject target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Person");
        Debug.Assert(target != null);
    }

    private void Update ()
    {
        var distance = (target.transform.position - transform.position).magnitude;
        if (distance < Radius)
        {
            Debug.Log("Assert");
        }
	}
}
