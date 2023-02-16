using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Begin()
    {
        SceneManager.LoadScene("Testing");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
