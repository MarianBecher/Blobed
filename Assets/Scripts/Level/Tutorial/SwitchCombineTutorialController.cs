using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SwitchCombineTutorialController : Triggerable
{
    public CharacterController startingChar;
    public CharacterController secondChar;
    public GameObject finger;
    public Trigger showFingerTrigger;

    public int stage = 0;
    public bool hide = true;

	// Use this for initialization
    void Start()
    {
        finger.SetActive(false);
	}

    void Update()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("PlayerSelector"))
        {
            foreach (Image img in g.GetComponentsInChildren<Image>())
                img.enabled = !hide;
        }

        switch(stage)
        {
            case 0:
                //Do nothing finger is out of view
                break;
            case 1:
                finger.transform.position = Vector3.Lerp(finger.transform.position, secondChar.transform.position + new Vector3(2.25f,6.25f,0), 2.0f * Time.deltaTime);
                if (!startingChar.active)
                {
                    stage = 2;
                    finger.SetActive(false);
                    hide = false;
                }
                break;
        }
    }

    

    public override void performTriggerEvent(Trigger source)
    {
        if(source == showFingerTrigger && stage == 0)
        {
            finger.SetActive(true);
            stage = 1;
        }
    }
}
