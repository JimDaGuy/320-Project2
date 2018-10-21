using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostMovement : MonoBehaviour {
    public GameObject targetPlayer;
    public Vector3 destination;
    public NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        destination = targetPlayer.transform.position;
        agent.SetDestination(targetPlayer.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        destination = targetPlayer.transform.position;
        agent.SetDestination(destination);
	}
}
