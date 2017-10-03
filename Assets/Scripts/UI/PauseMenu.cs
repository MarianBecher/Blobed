using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    GameObject menu;
    Image pauseButton;
    InputController input;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        
        menu = this.transform.Find("PauseMenu").gameObject;
        pauseButton = this.transform.Find("HUD").Find("PauseButton").GetComponent<Image>();
        input = InputController.getController();
        
        hidePaused();
	}
	
	// Update is called once per frame
	void Update () {

        if (input.isClickedOnUIElement(pauseButton.rectTransform.position, pauseButton.rectTransform.rect.width, pauseButton.rectTransform.rect.height) && Time.timeScale == 1)
        {
            pauseControl();
        }
	}

    //Reloads the Level
    public void Reload()
    {
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevel);
    }

    //controls the pausing of the scene
    public void pauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    //shows objects with ShowOnPause tag
    public void showPaused()
    {
        menu.SetActive(true);

        for(int i = 0; i < this.transform.childCount; i++)
        {
            if(this.transform.GetChild(i) != menu.transform)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        pauseButton.enabled = false;
    }

    //hides objects with ShowOnPause tag
    public void hidePaused()
    {
        menu.SetActive(false);            

        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i) != menu.transform)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        pauseButton.enabled = true;
    }

    //loads inputted level
    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        Application.LoadLevel("MainScreen");
    }
}
