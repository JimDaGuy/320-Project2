using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VampireMovement : MonoBehaviour {
    public GameObject targetPlayer;
    public Vector3 destination;
    public NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        destination = targetPlayer.transform.position;
        agent.SetDestination(targetPlayer.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        destination = targetPlayer.transform.position;
        agent.SetDestination(destination);
	}

    private void OnTriggerEnter(Collider other)
    {
        // decrement player health when a ghost or vampire is "hitting" them
        if (other.transform.parent != null && other.transform.parent.gameObject.tag == "Wood")
        {
            Destroy(other.gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().score++;
            Destroy(gameObject);
        }

    }
}
