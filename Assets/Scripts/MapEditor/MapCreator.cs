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
    
    void Awake()
    {
        Commands = new CommandStack();
        string path = Application.persistentDataPath + "/test.xml";
        if(System.IO.File.Exists(path))
        {
            Level = Serializer.ReadFromXmlFile<Map>(path);
        }
        else
        {
            Level = new Map(MAP_WIDTH, MAP_HEIGHT);
        }
    }

    void Start()
    {
        loader.loadMap(Level);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            play();
    }
       
    private void saveMap()
    {
        Serializer.WriteToXmlFile<Map>(Application.persistentDataPath + "/test.xml", Level, false);
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
