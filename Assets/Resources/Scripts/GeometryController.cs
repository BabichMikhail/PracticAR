using System.Collections.Generic;
using UnityEngine;

public class GeometryController : MonoBehaviour
{
    public GameObject Prefab;
    public int CountRound;

    public Vector3 CircleCenter;
    public float Radius;
    public float Speed;

    private List<GameObject> RoundObjects = new List<GameObject>();
    private List<float> Angles = new List<float>();

    private void Start ()
    {
        var step = 360 / (float)CountRound;
        for (var i = 0; i < CountRound; ++i)
        {
            var obj = Instantiate(Prefab, gameObject.transform);
            var angle = step * i + Random.Range(0, (float)step / 4);
            var position = CircleCenter;
            position.x += Radius * Mathf.Cos(angle);
            position.z += Radius * Mathf.Sin(angle);
            obj.transform.position = position;
            RoundObjects.Add(obj);
            Angles.Add(angle);
        }
    }

    private void Update ()
    {
        var deltaAngle = Time.deltaTime * Speed;
        for (var i = 0; i < CountRound; ++i)
        {
            Angles[i] += deltaAngle;
            var position = CircleCenter;
            position.x += Radius * Mathf.Cos(Angles[i]);
            position.z += Radius * Mathf.Sin(Angles[i]);
            RoundObjects[i].transform.position = position;
        }
    }
}
