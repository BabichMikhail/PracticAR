using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EmemyController : MonoBehaviour
{
    public GameObject EnemyPositionContainer;
    public GameObject EnemyPrefab;
    public int EnemyCount;
    public GameObject Target;

    private List<GameObject> respawns = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();

	void Start ()
    {
        if (EnemyPrefab.GetComponent<NavMeshAgent>() == null)
            Debug.LogWarning("Missing NavMeshAgent");

        for (var i = 0; i < EnemyPositionContainer.transform.childCount; ++i)
        {
            respawns.Add(EnemyPositionContainer.transform.GetChild(i).gameObject);
        }

        for (var i = 0; i < EnemyCount; ++i)
        {
            int idx = Random.Range(0, respawns.Count);
            Debug.Log(idx);
            var respawn = respawns[idx];
            enemies.Add(Instantiate(EnemyPrefab, respawn.transform));
        }
    }
	
	void Update ()
    {
        for (var i = 0; i < EnemyCount; ++i)
        {
            enemies[i].GetComponent<NavMeshAgent>().SetDestination(Target.transform.position);

        }
    }
}
