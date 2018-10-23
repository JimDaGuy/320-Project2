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

    //collision detection with wooden stakes
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Stake_Wood(Clone)")
        {
            //GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().woodenStakes.Remove(collision.gameObject);
            Destroy(collision.gameObject);
            //GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().score++;
            //GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().vampires.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
