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

    //collision detection with wooden stakes
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Stake_Wood(Clone)")
        {
            Destroy(collision.gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().score++;
            Destroy(gameObject);
        }
    }
}
