using UnityEngine;
using System.Collections;

public class mummy_aggro : MonoBehaviour {
	public Transform target1, target2;
	public GameObject player;
	public float lookAroundDuration, viewDistance, fov;

	bool aggro;
	float timer;
	Animator anim;
	Vector3 position, currentTarget, nextTarget;
	NavMeshAgent agent;
	int inMovementHash;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		position = GetComponent<Transform> ().position;
		anim = GetComponent<Animator> ();
		inMovementHash = Animator.StringToHash ("in_movement");
		currentTarget = target1.position;
		nextTarget = target2.position;
		aggro = false;

		anim.SetBool (inMovementHash, true);
		agent.SetDestination (currentTarget);
	}
	
	// Update is called once per frame
	void Update () {
		position = GetComponent<Transform> ().position;
		if (aggro) {
			if (scanPlayer ()) {
				updatePlayerTarget ();
			} else if (!agent.enabled) {
				waitLookAround ();
			}
			// Former player position reached, but player was lost.
			// Look around for a couple seconds and resume patrol.
			else if ((position - currentTarget).magnitude < 1f) {
				Debug.Log ("Player lost...");
				aggro = false;
				switchTargets ();
				beginLookAround ();
			}
		}
		// Else, the mummy is patrolling
		else {
			// First, look for player
			if (scanPlayer ()) {
				// Player found! Set new target, enter aggro mode and wait for next update
				Debug.Log ("Player found! Entering aggro mode.");
				updatePlayerTarget();
				aggro = true;
				return;
			}
			// Switch targets when target reached
			if (!agent.enabled) {
				waitLookAround ();
			}
			// Target reached : start looking around for the specified duration, then move to next waypoint
			else if ((position - currentTarget).magnitude < 0.05f) {
				Debug.Log ("Hit patrol target.");
				switchTargets ();
				beginLookAround ();
			} 
		}
	}

	// Test if the player is in view
	bool scanPlayer() {
		Vector3 dir = player.GetComponent<Transform>().position - transform.position;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, dir, out hit, viewDistance)) {
			if (hit.collider.gameObject == player && Vector3.Angle(transform.forward, dir) <= fov) {
				return true;
			}
		}
		return false;
	}

	// Stay in lookaround mode until timer is elapsed
	void beginLookAround() {
		anim.SetBool (inMovementHash, false);
		agent.enabled = false;
		timer = lookAroundDuration;
	}

	// Decrease the lookaround timer and set the mob back in motion when timer is up
	void waitLookAround() {
		timer -= Time.deltaTime;
		if (timer < 0) {
			anim.SetBool (inMovementHash, true);
			agent.enabled = true;
			agent.SetDestination (currentTarget);
		}
	}

	// Update current destination with player position
	void updatePlayerTarget() {
		currentTarget = player.GetComponent<Transform>().position;
		agent.SetDestination (currentTarget);
		Debug.Log ("Targeting player at " + currentTarget.ToString());
	}

	// Set the target to the next patrol waypoint
	void switchTargets() {
		currentTarget = nextTarget;
		if (nextTarget == target1.position) {
			nextTarget = target2.position;
			Debug.Log ("Switching to target 1 at " + currentTarget.ToString());
		}
		else {
			nextTarget = target1.position;
			Debug.Log ("Switching to target 2 at " + currentTarget.ToString());
		}
	}
}
