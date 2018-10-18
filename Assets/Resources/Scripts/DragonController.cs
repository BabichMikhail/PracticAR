using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class DragonController : MonoBehaviour
{
	private NavMeshAgent agent;
    private Animator animator;
    private GameObject camera;

    private Vector3 initPersonPosition;
    private Vector3 initCameraPosition;

    public float Scale;

	private void Start ()
	{
		agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponentInChildren<Animator>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        Debug.Assert(animator != null);
        Debug.Assert(agent != null);
        Debug.Assert(camera != null);

        initCameraPosition = camera.transform.position;
        initPersonPosition = gameObject.transform.position;
	}

    private Vector3 HandleMove(string keyCode, Vector3 baseMove)
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(keyCode))
            move = baseMove * Scale;
        return move;
    }

	public void Update()
	{
        Vector3 deltaPosition = 
            HandleMove("w", new Vector3(1, 0, 0)) +
            HandleMove("a", new Vector3(0, 0, -1)) +
            HandleMove("s", new Vector3(-1, 0, 0)) +
            HandleMove("d", new Vector3(0, 0, 1));
        animator.SetBool("Run", deltaPosition != Vector3.zero);            
        agent.SetDestination(transform.position + deltaPosition);
        camera.transform.position = transform.position - initPersonPosition + initCameraPosition;
    }
}
