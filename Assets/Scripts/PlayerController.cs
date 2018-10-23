using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// variables
	public GameObject stakePrefab_a;	// wood
	public GameObject stakePrefab_b;	// silver
	public GameObject stakePrefab_c;	// meat
	public Transform stakeSpawn;
	public GameObject playerCharacter;
	public string weaponType;		// can be equal to 'iron' 'wood' or 'meat'

	// Use this for initialization
	void Start ()
	{
		weaponType = "wood";
	}
	
	// Update is called once per frame
	void Update () 
	{
		float xPos = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		float yPos = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;
		float zPos = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, xPos, 0);
		transform.Translate(0, 0, zPos);

		if (Input.GetMouseButtonDown(0)) //Input.GetKeyDown(KeyCode.Tab)
		{
			if(weaponType == "iron")
			{
				Fire(stakePrefab_a);
			}
			else if (weaponType == "wood")
			{
				Fire(stakePrefab_b);
			}
			else if (weaponType == "meat")
			{
				Fire(stakePrefab_c);
			}

		}

		// check if mouse scrollwheel has moved up or down and adjust weapon accordingly
		if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f || Input.GetButtonDown("ChangeUp"))
		//if(Input.GetButtonDown("ChangeUp"))
		{
			if (weaponType == "iron")
			{
				weaponType = "meat";
			}
			else if (weaponType == "wood")
			{
				weaponType = "iron";
			}
			else if (weaponType == "meat")
			{
				weaponType = "wood";
			}
		}
		else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
		//if (Input.GetButtonDown("ChangeDown"))
		{
			if (weaponType == "iron")
			{
				weaponType = "wood";
			}
			else if (weaponType == "wood")
			{
				weaponType = "meat";
			}
			else if (weaponType == "meat")
			{
				weaponType = "iron";
			}
		}
	}

	void Fire(GameObject stakePrefab)
	{
		// create a transform from the stakeSpawn and the playerCharacter
		Transform stakeRot = stakeSpawn;
		stakeRot.rotation = playerCharacter.transform.rotation;

		// create a stake from a bullet prefab
		var stake = (GameObject)Instantiate(stakePrefab, stakeSpawn.position, stakeRot.rotation);
        GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().woodenStakes.Add(stake);

		// add velocity to the stake
		stake.GetComponent<Rigidbody>().velocity = stake.transform.forward * 20;


        // remove stake after 2 seconds
        GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>().woodenStakes.Remove(stake);
        Destroy(stake, 2.0f);
    }

    // change which model is displayed on the camera, depending on the current weaponType
}
