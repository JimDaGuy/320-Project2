using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //manager reference shortcut: use Manager.Instance. instead of GameObject.Find("Manager").GetComponent<Manager>().
    private static Manager instance;

    public static Manager Instance
    {
        get { return instance; }
    }


    //attributes
    public GameObject player;
    public GameObject vampire;
    public GameObject stake;
    public int score;
    public List<GameObject> vampires;
    public List<GameObject> woodenStakes;
    public int startVampires;
    public Vector3 spawnBoxMin;
    public Vector3 spawnBoxMax;
    public bool debugLines;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Two managers in a scene!");
        }
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < startVampires; i++)
        {
            Instantiate(vampire, new Vector3(Random.Range(spawnBoxMin.x, spawnBoxMax.x), Random.Range(spawnBoxMin.y, spawnBoxMax.y), Random.Range(spawnBoxMin.z, spawnBoxMax.z)), new Quaternion());
        }
        foreach (GameObject vampireObj in GameObject.FindGameObjectsWithTag("VampireObj"))
        {
            vampires.Add(vampireObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //spawn vampires
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    vampires.Add(Instantiate(vampire, new Vector3(Random.Range(-8, 9), 1, Random.Range(-8, 9)), new Quaternion()));
        //}
    }
}
