using System.Collections.Generic;
using UnityEngine;

public class Geometry : MonoBehaviour
{
    public GameObject Prefab;
    public int CountRound;

    public Vector3 CircleCenter;
    public float Radius;
    public float Speed;

    private List<GameObject> RoundObjects = new List<GameObject>();

	private void Start ()
    {
        float step = 360 / (float)CountRound;
	    for (int i = 0; i < CountRound; ++i)
        {
            var obj = Instantiate(Prefab, gameObject.transform);
            var position = CircleCenter;
            position.x += Radius * Mathf.Cos(step * i);
            position.y += Radius * Mathf.Sin(step * i);
            obj.transform.position = CircleCenter;
            RoundObjects.Add(obj);
        }
	}
	
	private void Update ()
    {
		
	}
}
