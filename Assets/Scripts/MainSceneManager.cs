using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    public enum GameStates { Ingame, Paused, Ended };
    public GameStates currentState;

    public GameObject controlsCanvas;
    public float controlsFadeDelay;
    public float controlsFadeDuration;
    public GameObject HUDCanvas;
    public GameObject weaponCanvas;
    public GameObject deathCanvas;
    public GameObject pauseCanvas;

    public Vector3 ghostSpawnBoxMin;
    public Vector3 ghostSpawnBoxMax;
    public Vector3 vampireSpawnBoxMin;
    public Vector3 vampireSpawnBoxMax;
    public GameObject vampire;
    public GameObject ghost;
    public int startVampires;
    public int startGhosts;
    public float timeBetweenGhostSpawns = 5.0f;
    private float ghostSpawnTimer = 0.0f;
    public float timeBetweenVampireSpawns = 5.0f;
    private float vampireSpawnTimer = 0.0f;

    public GameObject player;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController controller;
    public float playerHealth;

    public Text scoreText;

    // Use this for initialization
    void Start()
    {
        currentState = GameStates.Ingame;
        Time.timeScale = 1;

        for (int i = 0; i < startVampires; i++)
        {
            Instantiate(vampire, new Vector3(Random.Range(vampireSpawnBoxMin.x, vampireSpawnBoxMax.x), Random.Range(vampireSpawnBoxMin.y, vampireSpawnBoxMax.y), Random.Range(vampireSpawnBoxMin.z, vampireSpawnBoxMax.z)), new Quaternion());
        }
        for (int i = 0; i < startGhosts; i++)
        {
            Instantiate(ghost, new Vector3(Random.Range(ghostSpawnBoxMin.x, ghostSpawnBoxMax.x), Random.Range(ghostSpawnBoxMin.y, ghostSpawnBoxMax.y), Random.Range(ghostSpawnBoxMin.z, ghostSpawnBoxMax.z)), new Quaternion());
        }
        //foreach (GameObject vampireObj in GameObject.FindGameObjectsWithTag("VampireObj"))
        //{
        //    vampires.Add(vampireObj);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = player.GetComponent<PlayerController>().health;

        switch (currentState)
        {
            case GameStates.Ingame:
                // End the game when health reaches 0
                if (playerHealth <= 0)
                {
                    HUDCanvas.SetActive(false);
                    controlsCanvas.SetActive(false);
                    weaponCanvas.SetActive(false);

                    deathCanvas.SetActive(true);
                    scoreText.text = "Score: " + player.GetComponent<PlayerController>().score;

                    controller.enabled = false;

                    currentState = GameStates.Ended;
                    Time.timeScale = 0;
                }

                // Fade controls UI over time
                if (controlsFadeDelay > 0)
                    controlsFadeDelay -= Time.deltaTime;
                else
                {
                    controlsCanvas.GetComponent<CanvasGroup>().alpha -= 0;

                    if (controlsCanvas.GetComponent<CanvasGroup>().alpha > 0)
                        controlsCanvas.GetComponent<CanvasGroup>().alpha -= controlsFadeDuration / 100;
                }

                // Make Controls visible on C pressed
                if (Input.GetKey(KeyCode.C))
                {
                    controlsCanvas.GetComponent<CanvasGroup>().alpha = 1;
                }

                // Pause game on P pressed
                if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                {
                    currentState = GameStates.Paused;

                    controlsCanvas.GetComponent<CanvasGroup>().alpha = 1;

                    controller.enabled = false;
                    Time.timeScale = 0;

                    // Enable pause canvas
                    pauseCanvas.SetActive(true);

                    // Enable Cursor
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }

                //add a ghost every five seconds
                if (ghostSpawnTimer > 5.0f)
                {
                    Instantiate(ghost, new Vector3(Random.Range(ghostSpawnBoxMin.x, ghostSpawnBoxMax.x), Random.Range(ghostSpawnBoxMin.y, ghostSpawnBoxMax.y), Random.Range(ghostSpawnBoxMin.z, ghostSpawnBoxMax.z)), new Quaternion());
                    ghostSpawnTimer = 0.0f;
                }
                else if(vampireSpawnTimer > timeBetweenVampireSpawns)
                {
                    Instantiate(vampire, new Vector3(Random.Range(vampireSpawnBoxMin.x, vampireSpawnBoxMax.x), Random.Range(vampireSpawnBoxMin.y, vampireSpawnBoxMax.y), Random.Range(vampireSpawnBoxMin.z, vampireSpawnBoxMax.z)), new Quaternion());
                    vampireSpawnTimer = 0.0f;
                }
                ghostSpawnTimer += Time.deltaTime;
                vampireSpawnTimer += Time.deltaTime;

                break;
            case GameStates.Paused:
                // Unpause game on P pressed
                if (Input.GetKeyDown(KeyCode.P))
                {
                    // Set game state to ingame
                    currentState = GameStates.Ingame;
                    // Hide controls
                    controlsCanvas.GetComponent<CanvasGroup>().alpha = 0;
                    //Disable pause menu
                    pauseCanvas.SetActive(false);

                    //Enable controls
                    controller.enabled = true;
                    Time.timeScale = 1;

                    //Disable cursor
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;
                }

                // Return to menu
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene(0);
                }

                // Restart Game
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene(1);
                }
                break;
            case GameStates.Ended:
                // Return to main menu on escape pressed
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = 1;

                    // Enable Cursor
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    SceneManager.LoadScene(0);
                }

                // Restart Game
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene(1);
                }
                break;
            default:
                break;
        }
    }
}
