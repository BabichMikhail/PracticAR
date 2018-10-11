using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class DragonController : MonoBehaviour
{
	private NavMeshAgent agent;
	public GameObject bullet;
	private bool shooted = false;
	public GameObject createdBullet;

	private void Start ()
	{
		agent = gameObject.GetComponent<NavMeshAgent>();
		agent.SetDestination(transform.position + new Vector3(0, 0, 50f));
	}

	public void Update()
	{
		
	}
}
