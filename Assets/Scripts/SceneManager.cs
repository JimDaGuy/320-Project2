using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    public enum GameStates { Menu, Ingame, Paused, Ended };
    public GameStates currentState = GameStates.Ingame;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
