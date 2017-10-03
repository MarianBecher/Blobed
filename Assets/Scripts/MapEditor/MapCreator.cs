using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapCreator : MonoBehaviour {

    public const int MAP_WIDTH = 80;
    public const int MAP_HEIGHT = 20;

    public Map Level { get; private set; }
    public CommandStack Commands { get; private set; }

    [SerializeField]
    private EditorLevelLoader loader;
    public Text temp;
    
    public void Awake()
    {
        Commands = new CommandStack();
        Level = new Map(MAP_WIDTH, MAP_HEIGHT);
        loader.loadMap(Level);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            play();
    }
       
    private void saveMap()
    {
        Serializer.WriteToXmlFile<Map>(Application.persistentDataPath + "/test.xml", Level);
    }

    public void play()
    {
        try {
            saveMap();
            SceneManager.LoadScene("Level");
        }
        catch(Exception e)
        {
            temp.text += e.Message+"\n";
        }
    }

    public void undo()
    {
        Commands.undo();
    }
}
