using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject targetObject;
    private GameObject bulletContainer;
    private float lastFireTime;

    public float FireDistance;
    public GameObject BulletObject;
    public float fireSpeed = 3;

    private void Start()
    {
        // Получаем Animator у объекта, агента и находим цель с тэгом Person.
        // Person - тэг на нашем персонаже.
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator != null);
        targetObject = GameObject.FindGameObjectWithTag("Person"); // На самом деле так делать не очень.
        Debug.Assert(targetObject != null);
        agent = GetComponent<NavMeshAgent>();
        Debug.Assert(agent != null);
        bulletContainer = GameObject.FindGameObjectWithTag("BulletContainer");
        Debug.Assert(bulletContainer != null);
    }

    private void Update()
    {
        if (targetObject == null)
            return;

        // Сбрасываем все флаги аниматора и бега.
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        animator.SetBool("Shoot", false);

        var enableAgent = false;
        // Если расстояние от позиции противника больше расстояния до цели, то Бег.
        if ((transform.position - targetObject.transform.position).sqrMagnitude > FireDistance * FireDistance)
        {
            animator.SetBool("Run", true);
            enableAgent = true;
        }
        // Иначе Стрельба.
        else
        {
            animator.SetBool("Shoot", true);
            if (lastFireTime + 1 / fireSpeed < Time.time)
            {
                lastFireTime = Time.time;
                var velocity = targetObject.transform.position - transform.position;
                var bullet = Instantiate(BulletObject, bulletContainer.transform);
                bullet.transform.position = transform.position;
                
                bullet.transform.LookAt(targetObject.transform);
                var bulletController = bullet.GetComponent<BulletController>();
                bulletController.velocity = velocity.normalized;
                bulletController.creator = gameObject;
            }
        }

        if (enableAgent)
            agent.SetDestination(targetObject.transform.position);
        else
        {
            transform.LookAt(targetObject.transform);
            agent.SetDestination(transform.position);
        }
    }
}
