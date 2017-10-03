using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartScreenController : MonoBehaviour {

    public string lvlName;
    public float endOfAnimation = 3f;
    public LevelLoader loader;
    private InputController input;
    private GameObject startScreen;

    void Awake()
    {
        Map m = Serializer.ReadFromXmlFile<Map>(Application.persistentDataPath + "/test.xml");
        loader.loadMap(m);
    }


	// Use this for initialization
	void Start () {
        input = InputController.getController();
        input.stopInput = true;
        startScreen = GameObject.Find("StartScreen");
        startScreen.GetComponentInChildren<Text>().text = lvlName;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.timeSinceLevelLoad > endOfAnimation)
        {
            input.stopInput = false;
            startScreen.SetActive(false);
        }
	}
}
