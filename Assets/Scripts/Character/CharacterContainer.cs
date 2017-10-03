using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterContainer : MonoBehaviour {
    private List<CharacterController> currentCharacters;
    private List<GameObject> switchCharacterButtons; 
    private Camera camera;
    private InputController inputController;

	// Use this for initialization
	void Start () {
        initialize();
	}

    private void initialize()
    {
        camera = GameObject.Find("Camera").GetComponent<Camera>();
        if(currentCharacters == null)
        {
            currentCharacters = new List<CharacterController>();
            switchCharacterButtons = new List<GameObject>();
            inputController = InputController.getController();
        }
    }

    public void removeCharacter(CharacterController ch)
    {
        currentCharacters.Remove(ch);
        updateSwitchPlayerUI();
    }

    public void addCharacter(CharacterController ch)
    {
        initialize();
        currentCharacters.Add(ch);
        updateSwitchPlayerUI();
    }
	
	// Update is called once per frame
	void Update () {
        updateSwitchPlayerUI();
	}

    public void setNewActivePlayer(CharacterController c)
    {
        this.switchCharacter(currentCharacters.IndexOf(c));
    }

    public void fixActivePlayer()
    {
        bool hasActivePlayer = false;
        foreach(CharacterController c in currentCharacters)
        {
            if(c.active)
            {
                hasActivePlayer = true;
                break;
            }
        }

        if(!hasActivePlayer)
        {
            Debug.Log("FIXED");
            CameraController cam = gameObject.GetComponentInChildren<CameraController>();
            cam.target.GetComponent<CharacterController>().active = true;
        }
    }

    private void switchCharacter(int newCharacter)
    {
        //Temp player switch
        for (int i = 0; i < currentCharacters.Count; i++)
        {
            this.currentCharacters[i].active = false;
        }

        int newActivePlayer = newCharacter;

        CameraController cam = gameObject.GetComponentInChildren<CameraController>();
        EndOfAnimationCalback calback = delegate() { currentCharacters[newActivePlayer].active = true; };
        cam.newTarget(currentCharacters[newActivePlayer].transform, calback);
    }

    private void updateSwitchPlayerUI()
    {
        int deltaCount = currentCharacters.Count - switchCharacterButtons.Count;
        if(deltaCount > 0)
        {
            //Create missings Pointers
            for (int i = currentCharacters.Count - deltaCount; i < currentCharacters.Count; i++)
            {
                GameObject newPointer = (GameObject) Instantiate(Resources.Load("Character-Pointer"));
                newPointer.transform.SetParent(GameObject.Find("UI").transform);
                newPointer.transform.localScale = new Vector3(1, 1, 1);
                newPointer.transform.localPosition = new Vector3(-Screen.width, -Screen.height, 1);
                newPointer.transform.SetSiblingIndex(0);
                switchCharacterButtons.Add(newPointer);
            }
        }


        Vector2 cameraSize = new Vector2(2 * camera.orthographicSize * camera.pixelWidth / camera.pixelHeight, 2* camera.orthographicSize);
        Vector2 cameraPosition = camera.transform.position;
        Rect cameraRect = new Rect(cameraPosition - 0.5f * cameraSize, cameraSize);

        for(int i = 0; i < switchCharacterButtons.Count; i++)
        {

            switchCharacterButtons[i].transform.localPosition = new Vector3(-Screen.width, -Screen.height, 1);
            if (i < currentCharacters.Count)
            {
                Vector2 distance = ((Vector2)(currentCharacters[i].transform.position - camera.transform.position));

                if (cameraRect.Contains(currentCharacters[i].transform.position))
                {
                    if (!currentCharacters[i].active && inputController.isClickedOnCharacter(currentCharacters[i]))
                    {
                        this.switchCharacter(i);
                        inputController.clickActionPerformed();
                    }
                    continue; //Dont show pointer
                }

                Vector2 direction = distance.normalized;
                float angle = Mathf.Atan2(direction.y, direction.x);

                float length = Mathf.Abs(camera.pixelHeight / (2 * Mathf.Sin(angle)));
                
                if(Mathf.Abs(direction.x) > (new Vector2(camera.pixelWidth, camera.pixelHeight).normalized).x)
                {
                    length = Mathf.Abs(camera.pixelWidth / (2 * Mathf.Cos(angle)));    
                }

                Vector3 newPosition = new Vector3(direction.x * length, direction.y * length, 1);
                
                
                RectTransform HUD = GameObject.Find("HUD").GetComponent<RectTransform>();
                Vector2 hudPosition = (Vector2)(HUD.position) - new Vector2(HUD.rect.width, HUD.rect.height) - new Vector2(Screen.width / 2f, Screen.height / 2f);
                
                //Inside Pause HUD
                if(newPosition.x > hudPosition.x && newPosition.y > hudPosition.y)
                {
                    float xFactor = (hudPosition.x - newPosition.x) / direction.x;
                    float yFactor = (hudPosition.y - newPosition.y) / direction.y;
                    float factor = xFactor > yFactor ? xFactor : yFactor;
                    newPosition += new Vector3(direction.x, direction.y, 0) * factor;
                }

                //Sledged Edge of Pause HUD


                //subtract own length
                newPosition -= new Vector3(direction.x * 0.5f * switchCharacterButtons[i].GetComponent<RectTransform>().rect.width, direction.y * 0.5f * switchCharacterButtons[i].GetComponent<RectTransform>().rect.height);

                switchCharacterButtons[i].transform.localPosition = newPosition;
                switchCharacterButtons[i].transform.localRotation = Quaternion.AngleAxis(angle * 180 / Mathf.PI + 90, new Vector3(0, 0, 1));
                switchCharacterButtons[i].transform.Find("color_1").GetComponent<Image>().color = currentCharacters[i].transform.Find("color-container").GetComponent<SpriteRenderer>().color;
                switchCharacterButtons[i].transform.Find("color_2").GetComponent<Image>().color = currentCharacters[i].transform.Find("color-container").GetComponent<SpriteRenderer>().color;
                switchCharacterButtons[i].transform.Find("color_3").GetComponent<Image>().color = currentCharacters[i].transform.Find("color-container").GetComponent<SpriteRenderer>().color;
                switchCharacterButtons[i].transform.Find("color_4").GetComponent<Image>().color = currentCharacters[i].transform.Find("color-container").GetComponent<SpriteRenderer>().color;

                if(inputController.isClickedOnUIElement(switchCharacterButtons[i].GetComponent<RectTransform>().position, switchCharacterButtons[i].GetComponent<RectTransform>().rect.width, switchCharacterButtons[i].GetComponent<RectTransform>().rect.height))
                {
                    this.switchCharacter(i);
                    inputController.clickActionPerformed();
                }
            }
        }
    }
}
