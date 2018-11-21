using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyIntTuple
{
    public int first;
    public int second;
    public int third;

    public MyIntTuple(int first, int second, int third) { this.first = first; this.second = second; this.third = third; }
}

public class EmemiesController : MonoBehaviour
{
    public GameObject EnemyPositionContainer;
    public GameObject[] EnemyPrefabs;
    public int EnemyCount;
    public GameObject Target;

    private readonly List<GameObject> respawns = new List<GameObject>();
    private readonly List<GameObject> enemies = new List<GameObject>();

    private void Start ()
    {
        for (int i = 0; i < EnemyPrefabs.Length; ++i)
            if (EnemyPrefabs[i].GetComponent<NavMeshAgent>() == null)
                Debug.LogWarning("Missing NavMeshAgent");

        for (var i = 0; i < EnemyPositionContainer.transform.childCount; ++i)
        {
            respawns.Add(EnemyPositionContainer.transform.GetChild(i).gameObject);
        }
    }

    // time in milliseconds, unit count, respawn index
    private readonly List<MyIntTuple> UNIT_WAVES = new List<MyIntTuple>() {
        new MyIntTuple(1, 1, 1),
        new MyIntTuple(3000, 2, 0),
        new MyIntTuple(23000, 1, 1),
        new MyIntTuple(24000, 1, 0),
    };

    private float startedAt;
    private float lastIncreaseMoneyTime;
    private int unitWaveIndex = 0;
    private int lastSendUnitTime = -1000;
    private const int SEND_UNIT_INTERVAL = 200;

    private void Update()
    {
        int now = (int)((Time.time - startedAt) * 1000);
        if (lastSendUnitTime + SEND_UNIT_INTERVAL < now && unitWaveIndex < UNIT_WAVES.Count && UNIT_WAVES[unitWaveIndex].first <= now)
        {
            SendUnit(UNIT_WAVES[unitWaveIndex].third);
            --UNIT_WAVES[unitWaveIndex].second;
            if (UNIT_WAVES[unitWaveIndex].second == 0)
                ++unitWaveIndex;
            lastSendUnitTime = now - Random.Range(0, SEND_UNIT_INTERVAL / 3);
        }

        for (var i = 0; i < enemies.Count; ++i)
            enemies[i].GetComponent<NavMeshAgent>().SetDestination(Target.transform.position);
    }

    private void SendUnit(int respawnIndex)
    {
        Debug.Log(UNIT_WAVES[unitWaveIndex].second);
        for (var i = 0; i < UNIT_WAVES[unitWaveIndex].second; ++i)
        {
            var respawn = respawns[respawnIndex];
            var enemyIndex = Random.Range(0, EnemyPrefabs.Length);
            enemies.Add(Instantiate(EnemyPrefabs[enemyIndex], respawn.transform));
        }
    }
}
