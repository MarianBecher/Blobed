using UnityEngine;
using System.Collections;

public class EndOfLevel : MonoBehaviour {
    private int nextLvl;

    void Start()
    {
        Config c = Config.getConfig();
        nextLvl = c.getCurrentLvl() + 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Config c = Config.getConfig();
        c.unlockLevel(nextLvl);
        Application.LoadLevel(0);
    }
}
