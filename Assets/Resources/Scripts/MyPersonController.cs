using UnityEngine;
using UnityEngine.AI;

public class MyPersonController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject mainCamera;
    private GameObject bulletContainer;

    public GameObject Bullet;

    private Vector3 initPersonPosition;
    private Vector3 initCameraPosition;

    public float Scale;

    private void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");

        Debug.Assert(bulletContainer != null);
        Debug.Assert(animator != null);
        Debug.Assert(agent != null);
        Debug.Assert(mainCamera != null);

        initCameraPosition = mainCamera.transform.position;
        initPersonPosition = gameObject.transform.position;
    }

    private Vector3 HandleMove(string keyCode, Vector3 baseMove)
    {
        var move = Vector3.zero;
        if (Input.GetKey(keyCode))
        {
            // Переводим все координаты в плоскость Oxz и считаем угол, на который нам нужно повернуть вектор "движения".
            var flatCameraVector = initCameraPosition - initPersonPosition;
            flatCameraVector.y = 0;
            // Применение классической матрицы поворота: x' = x * cos(a) - y * sin(a); y' = y * cos(a) + x * sin(a);
            var angle = Mathf.Deg2Rad * Vector3.Angle(flatCameraVector.normalized, Vector3.forward);
            move.x = baseMove.z * Mathf.Cos(angle) + baseMove.x * Mathf.Sin(angle);
            move.z = baseMove.x * Mathf.Cos(angle) - baseMove.z * Mathf.Sin(angle);
            move *= Scale;
        }
        return move;
    }

    public void Update()
    {
        var deltaPosition =
            HandleMove("w", new Vector3(-1, 0, 0)) +
            HandleMove("a", new Vector3(0, 0, 1)) +
            HandleMove("s", new Vector3(1, 0, 0)) +
            HandleMove("d", new Vector3(0, 0, -1));

        if (Input.GetKey("q"))
        {
            var newBullet = Instantiate(Bullet, bulletContainer.transform);
            newBullet.transform.position = transform.position;
            newBullet.SetActive(true);
            var bulletController = newBullet.GetComponent<BulletController>();
            Debug.Assert(bulletController != null);
            bulletController.velocity = transform.forward;
        }

        animator.SetBool("Run", deltaPosition != Vector3.zero);
        agent.SetDestination(transform.position + deltaPosition);
        mainCamera.transform.position = transform.position - initPersonPosition + initCameraPosition;
    }
}
