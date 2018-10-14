using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// variables
	public GameObject stakePrefab;
	public Transform stakeSpawn;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		float xPos = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		float yPos = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;
		float zPos = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, xPos, 0);
		transform.Translate(0, 0, zPos);

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Fire();
		}
	}

	void Fire()
	{
		// create a stake from a bullet prefab
		var stake = (GameObject)Instantiate(stakePrefab, stakeSpawn.position, stakeSpawn.rotation);

		// add velocity to the stake
		stake.GetComponent<Rigidbody>().velocity = stake.transform.forward * 15;
		

		// remove stake after 2 seconds
		Destroy(stake, 2.0f);
	}
}
