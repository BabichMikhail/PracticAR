using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MyPersonController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject mainCamera;
    private GameObject bulletContainer;
    private float lastFireTime;

    private Vector3 initPersonPosition;
    private Vector3 initCameraPosition;

    private LeftJoystick leftJoystickController;
    private RightJoystick rightJoystickController;

    public float Scale;
    public float FireSpeed = 5;
    public GameObject Bullet;
    public Image LeftJoystick;
    public Image RightJoystick;

    private void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Assert(agent != null);

        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator != null);

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Debug.Assert(mainCamera != null);

        bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");
        Debug.Assert(bulletContainer != null);

        leftJoystickController = LeftJoystick.GetComponent<LeftJoystick>();
        Debug.Assert(leftJoystickController != null);

        rightJoystickController = RightJoystick.GetComponent<RightJoystick>();
        Debug.Assert(rightJoystickController != null);

        initCameraPosition = mainCamera.transform.position;
        initPersonPosition = gameObject.transform.position;
    }

    private Vector3 TransformMovementVector(Vector3 baseMove)
    {
        baseMove = baseMove.normalized;

        // TODO тут есть баг.
        // Переводим все координаты в плоскость Oxz и считаем угол, на который нам нужно повернуть вектор "движения".
        var flatCameraVector = initCameraPosition - initPersonPosition;
        flatCameraVector.y = 0;
        // Применение классической матрицы поворота: x' = x * cos(a) - y * sin(a); y' = y * cos(a) + x * sin(a);
        var angle = Mathf.Deg2Rad * Vector3.Angle(flatCameraVector.normalized, Vector3.forward);
        var move = Vector3.zero;
        move.x = baseMove.z * Mathf.Cos(angle) + baseMove.x * Mathf.Sin(angle);
        move.z = baseMove.x * Mathf.Cos(angle) - baseMove.z * Mathf.Sin(angle);
        return move * Scale;
    }

    private Vector3 HandleMove(string keyCode, Vector3 baseMove)
    {
        var move = Vector3.zero;
        if (Input.GetKey(keyCode))
            move = baseMove;
        return move;
    }

    private Vector3 HandleJoystickMove()
    {
        var leftJoystickInput = leftJoystickController.GetInputDirection();
        var leftJoystickMove = new Vector3(-leftJoystickInput.y, 0, -leftJoystickInput.x);
        return leftJoystickMove.normalized;
    }

    private void FixedUpdate()
    {
        var deltaPosition = TransformMovementVector(
            HandleMove("w", new Vector3(-1, 0, 0)) +
            HandleMove("a", new Vector3(0, 0, 1)) +
            HandleMove("s", new Vector3(1, 0, 0)) +
            HandleMove("d", new Vector3(0, 0, -1)) +
            HandleJoystickMove()
        );

        if (Input.GetKey("q") && lastFireTime + 1 / FireSpeed < Time.time)
        {
            lastFireTime = Time.time;
            var velocity = transform.forward;
            var bullet = Instantiate(Bullet, bulletContainer.transform);
            bullet.transform.position = transform.position;

            var bulletController = bullet.GetComponent<BulletController>();
            bulletController.velocity = velocity.normalized;
            bulletController.creator = gameObject;
        }

        animator.SetBool("Run", deltaPosition != Vector3.zero);
        agent.SetDestination(transform.position + deltaPosition);
        mainCamera.transform.position = transform.position - initPersonPosition + initCameraPosition;
    }
}
