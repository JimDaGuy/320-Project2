using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	// variables
	public GameObject ironStakePrefab;	
	public GameObject woodStakePrefab;	
	public GameObject meatStakePrefab;

	public GameObject ironStakeUIPrefab;
	public GameObject woodStakeUIPrefab;
	public GameObject meatStakeUIPrefab;
	
	public Transform stakeSpawn;
	public GameObject playerCharacter;
	
	public GameObject heldItemParent;
	public GameObject heldStake;
	public GameObject heldStakeUIParent;
	public GameObject heldStakeUI;
	public Text heldStakeText;
	
	public GameObject SceneManager;
	public string stateString;

	private float lastFire;

	public enum Weapons 
	{
	    Iron = 1,
	    Wood = 2,
	    Meat = 3
	};
	public Weapons currentWeapon;
	private int numWeapons;
	public int health;
	public int score;

	private float ironFireTime = 0.5f;
	private float woodFireTime = 0.5f;
	private float meatFireTime = 0.5f;

	void Start ()
	{
	    stateString = SceneManager.GetComponent<MainSceneManager>().currentState.ToString();
	    numWeapons = System.Enum.GetNames(typeof(Weapons)).Length;
	    currentWeapon = Weapons.Iron;
	    health = 5;
		
		lastFire = 0;
	}
	
	void Update () 
	{
	    stateString = SceneManager.GetComponent<MainSceneManager>().currentState.ToString();
	
	    if (stateString == "Ingame")
	    {
	        float xPos = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
	        float yPos = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;
	        float zPos = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

	        transform.Rotate(0, xPos, 0);
	        transform.Translate(0, 0, zPos);

            // Shooting Input - Left Click
            if (Input.GetMouseButton(0))
	        {
	            switch (currentWeapon)
	            {
	                case Weapons.Iron:
				if(Time.time > ironFireTime + lastFire)
					Fire(ironStakePrefab);
				break;
	                case Weapons.Wood:
				if (Time.time > woodFireTime + lastFire)
					Fire(woodStakePrefab);
				break;
	                case Weapons.Meat:
				if (Time.time > meatFireTime + lastFire)
					Fire(meatStakePrefab);
				break;
	                default:
				break;
	            }
	        }
	
	        // Cycle weapons on scroll
	        if (Input.mouseScrollDelta.y > 0f)
	            CycleWeapons(true);
	        else if (Input.mouseScrollDelta.y < 0f)
	            CycleWeapons(false);
	    }
	}

	private void Fire(GameObject stakePrefab)
	{
		// create a transform from the stakeSpawn and the playerCharacter
		Transform stakeRot = stakeSpawn;
		stakeRot.rotation = playerCharacter.transform.rotation;
	
		// create a stake from a bullet prefab
		var stake = (GameObject)Instantiate(stakePrefab, stakeSpawn.position, stakeRot.rotation);
	
		// add velocity to the stake
		stake.GetComponent<Rigidbody>().velocity = stake.transform.forward * 20;

		// mark the time the stake was created
		lastFire = Time.time;

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
	    UpdateWeaponViews();
	}
	
	void UpdateWeaponViews()
	{
	    Transform prevStakeTransform = heldStake.transform;
	    Transform uiStakeTransform = heldStakeUI.transform;
	    Destroy(heldStake);
	    Destroy(heldStakeUI);
	
	    switch (currentWeapon)
	    {
	        case Weapons.Iron:
	            // Update held item stake
	            GameObject newIronStake = (GameObject)Instantiate(ironStakePrefab, prevStakeTransform.position, prevStakeTransform.rotation);
                newIronStake.gameObject.GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                Destroy(newIronStake.GetComponent<Rigidbody>());
	            newIronStake.transform.parent = heldItemParent.transform;
                heldStake = newIronStake;


                //Update UI Text
                heldStakeText.text = "Iron Stakes (Zombies)";
	
	            //Update UI Stake
	            GameObject newIronStakeUI = (GameObject)Instantiate(ironStakeUIPrefab, uiStakeTransform.localPosition, uiStakeTransform.localRotation);
	            newIronStakeUI.transform.SetParent(heldStakeUIParent.transform, false);
	            heldStakeUI = newIronStakeUI;
	            break;
	        case Weapons.Wood:
	            GameObject newWoodStake = (GameObject)Instantiate(woodStakePrefab, prevStakeTransform.position, prevStakeTransform.rotation);
                newWoodStake.gameObject.GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                Destroy(newWoodStake.GetComponent<Rigidbody>());
	            newWoodStake.transform.parent = heldItemParent.transform;
                heldStake = newWoodStake;

                heldStakeText.text = "Wood Stakes (Vampires)";
	
	            GameObject newWoodStakeUI = (GameObject)Instantiate(woodStakeUIPrefab, uiStakeTransform.localPosition, uiStakeTransform.localRotation);
	            newWoodStakeUI.transform.SetParent(heldStakeUIParent.transform, false);
	            heldStakeUI = newWoodStakeUI;
	            break;
	        case Weapons.Meat:
	            GameObject newMeatStake = (GameObject)Instantiate(meatStakePrefab, prevStakeTransform.position, prevStakeTransform.rotation);
                newMeatStake.gameObject.GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                Destroy(newMeatStake.GetComponent<Rigidbody>());
	            newMeatStake.transform.parent = heldItemParent.transform;
                heldStake = newMeatStake;

                heldStakeText.text = "Meat Steaks (Ghosts)";
	
	            GameObject newMeatStakeUI = (GameObject)Instantiate(meatStakeUIPrefab, uiStakeTransform.localPosition, uiStakeTransform.localRotation);
	            newMeatStakeUI.transform.SetParent(heldStakeUIParent.transform, false);
	            heldStakeUI = newMeatStakeUI;
	
	            break;
	        default:
	            break;
	    }
	}
	
	private void OnTriggerEnter(Collider other)
	{
	    if (stateString == "Ingame")
	    {
	        // Kill player on collision with water
	        if (other.gameObject.name == "Water4Advanced")
	        {
	            health = 0;
	        }

        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (stateString == "Ingame")
        {
            // decrement player health when a ghost or vampire is "hitting" them
            if (other.gameObject.name == "Ghost" || other.gameObject.name == "Ghost(Clone)" || other.gameObject.name == "Vampire" || other.gameObject.name == "Vampire(Clone)")
            {
                health--;
            }
        }
    }
}
