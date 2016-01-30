using UnityEngine;
using System.Collections;

public class mummy_nav_test : MonoBehaviour {
	public Transform target1, target2, transform;
	Transform currentTarget;
	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		transform = GetComponent<Transform> ();
		currentTarget = target1;
	}
	
	// Update is called once per frame
	void Update () {
		// Switch targets when target reached
		if ((transform.position - currentTarget.position).magnitude < 0.5f) {
			if (currentTarget == target1)
				currentTarget = target2;
			else
				currentTarget = target1;
		}
		agent.SetDestination (currentTarget.position);
	}
}
