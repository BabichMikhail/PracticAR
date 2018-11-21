using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject targetObject;

    public float FireDistance;

    private void Start ()
    {
        // Получаем Animator у объекта, агента и находим цель с тэгом Person.
        // Person - тэг на нашем персонаже.
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator != null);
        targetObject = GameObject.FindGameObjectWithTag("Person"); // На самом деле так делать не очень.
        Debug.Assert(targetObject != null);
        agent = GetComponent<NavMeshAgent>();
        Debug.Assert(agent != null);
    }

    private void Update ()
    {
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
            // TODO
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
