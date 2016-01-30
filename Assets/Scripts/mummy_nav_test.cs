using UnityEngine;
using System.Collections;

public class mummy_nav_test : MonoBehaviour {
	public Transform target1, target2;
	public float lookAroundDuration;
	float timer;
	Animator anim;
	Transform transform;
	Transform currentTarget;
	NavMeshAgent agent;
	int inMovementHash;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		transform = GetComponent<Transform> ();
		anim = GetComponent<Animator> ();
		inMovementHash = Animator.StringToHash ("in_movement");
		currentTarget = target1;
	}
	
	// Update is called once per frame
	void Update () {
		// Switch targets when target reached
		if (!agent.enabled) {
			timer -= Time.deltaTime;
			if (timer < 0) {
				agent.enabled = true;
				anim.SetBool (inMovementHash, true);
			}
		}
		else if (transform.position == currentTarget.position) {
			if (currentTarget == target1)
				currentTarget = target2;
			else
				currentTarget = target1;

			// Start looking around for the specified duration, then move to next waypoint
			anim.SetBool (inMovementHash, false);
			agent.enabled = false;
			timer = lookAroundDuration;
		}
		agent.SetDestination (currentTarget.position);
	}
}
