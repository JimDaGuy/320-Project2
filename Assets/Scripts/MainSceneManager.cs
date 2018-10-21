using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public enum GameStates { Menu, Ingame, Paused, Ended };
    public GameStates currentState = GameStates.Ingame;

    public GameObject controlsCanvas;
    public float controlsFadeDelay;
    public float controlsFadeDuration;
    public float wtf;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        wtf = controlsCanvas.GetComponent<CanvasGroup>().alpha;


        if (currentState == GameStates.Ingame)
        {
            if (controlsFadeDelay > 0)
                controlsFadeDelay -= Time.deltaTime;
            else
            {
                controlsCanvas.GetComponent<CanvasGroup>().alpha -= 0;

                if (controlsCanvas.GetComponent<CanvasGroup>().alpha > 0)
                    controlsCanvas.GetComponent<CanvasGroup>().alpha -= controlsFadeDuration / 100;
            }

            // Make Controls visible on C pressed
            if(Input.GetKey(KeyCode.C))
            {
                controlsCanvas.GetComponent<CanvasGroup>().alpha = 1;
            }
        }
    }
}
