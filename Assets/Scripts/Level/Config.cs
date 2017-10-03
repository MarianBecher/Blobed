using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class Config {

    private const string fileName = "/config.txt";

    private int levelUnlocked = 1; //max unlocked lvl
    private float inputMoveSize = 1/3f; //size for move area. Jump area is 1-movearea
    private int currentLvl = 0;  //current played lvl
    
    private static Config instance;
    public static Config getConfig()
    {
        if (instance == null)
        {
            instance = new Config();
        }
        return instance;
    }

    public float getUIScale()
    {
        return Screen.width / 800f;
    }

    private Config()
    {
        if(!File.Exists(Application.persistentDataPath + fileName))
        {
            writeConfig();
        }

        StreamReader theReader = new StreamReader(Application.persistentDataPath + fileName, Encoding.Default);
        string line;
        using (theReader)
        {
             do
             {
                 line = theReader.ReadLine();
                     
                 if (line != null)
                 {
                     string[] entries = line.Split('=');
                     if (entries.Length != 2)
                         continue;
                     switch(entries[0])
                     {
                         case "lvl":
                             int.TryParse(entries[1], out levelUnlocked);
                             break;
                         case "c-lvl":
                             int.TryParse(entries[1], out currentLvl);
                             break;
                         case "input":
                             float.TryParse(entries[1], out inputMoveSize);
                             break;
                     }
                 }
             }
             while (line != null);
             theReader.Close();
        }
        writeConfig();
    }

    public void writeConfig()
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + fileName, false);
        using (writer)
        {
            writer.WriteLine("lvl" + "=" + levelUnlocked);
            writer.WriteLine("c-lvl" + "=" + currentLvl);
            writer.WriteLine("input" + "=" + inputMoveSize);
            writer.Close();
        }
    }

    //Current Max unlocked Lvl
    public int getUnlockedLevelCount()
    {
        return levelUnlocked;
    }

    public void unlockLevel(int lvl)
    {
        if (lvl > levelUnlocked)
        {
            levelUnlocked = lvl;
            writeConfig();
        }
    }
   
    public float getInputMoveSize()
    {
        return inputMoveSize;
    }

    public void setCurrentLvl(int lvl)
    {
        currentLvl = lvl;
        writeConfig();
    }

    public int getCurrentLvl()
    {
        return currentLvl;
    }
}
