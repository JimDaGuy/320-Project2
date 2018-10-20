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

    public enum Weapons {
        Iron = 1,
        Wood = 2,
        Meat = 3
    };
    public Weapons currentWeapon;
    private int numWeapons;
    public int health;
    public float test;

    // Use this for initialization
    void Start ()
	{
        numWeapons = System.Enum.GetNames(typeof(Weapons)).Length;
        currentWeapon = Weapons.Iron;
    }
	
	// Update is called once per frame
	void Update () 
	{
		float xPos = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		float yPos = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;
		float zPos = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, xPos, 0);
		transform.Translate(0, 0, zPos);

        // Shooting Input - Left Click
		if (Input.GetMouseButtonDown(0))
		{
            switch (currentWeapon)
            {
                case Weapons.Iron:
                    Fire(stakePrefab_a);
                    break;
                case Weapons.Wood:
                    Fire(stakePrefab_b);
                    break;
                case Weapons.Meat:
                    Fire(stakePrefab_c);
                    break;
                default:
                    break;
            }
		}

        // Weapon Cycle Input - CapsLock
        if(Input.GetKeyDown(KeyCode.CapsLock))
        {
            CycleWeapons(true);
        }

        // check if mouse scrollwheel has moved up or down and adjust weapon accordingly
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f || Input.GetButtonDown("ChangeUp"))
            CycleWeapons(true);
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            CycleWeapons(false);
	}

	void Fire(GameObject stakePrefab)
	{
		// create a transform from the stakeSpawn and the playerCharacter
		Transform stakeRot = stakeSpawn;
		stakeRot.rotation = playerCharacter.transform.rotation;

		// create a stake from a bullet prefab
		var stake = (GameObject)Instantiate(stakePrefab, stakeSpawn.position, stakeRot.rotation);

		// add velocity to the stake
		stake.GetComponent<Rigidbody>().velocity = stake.transform.forward * 20;
		

		// remove stake after 2 seconds
		Destroy(stake, 2.0f);
	}

    void CycleWeapons(bool forward)
    {
        int currentIndex = (int)currentWeapon;
        int increment = (forward) ? 1 : -1;

        currentIndex += increment;

        // Fix out of bounds
        if (currentIndex < 1)
            currentIndex = numWeapons;
        else if (currentIndex > numWeapons)
            currentIndex = 1;

        currentWeapon = (Weapons)currentIndex;
    }

	// change which model is displayed on the camera, depending on the current weaponType
}
