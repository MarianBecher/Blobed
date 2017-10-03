using UnityEngine;
using System.Collections;

public class MovementTutorialController : Triggerable
{
    public GameObject jumpTutorial;
    public GameObject moveTutorial;

    public Trigger moveHideTrigger = null;
    public Trigger jumpShowTrigger = null;
    public Trigger jumpHideTrigger = null;

    void Start()
    {
        Config c = Config.getConfig();
        
        //Reposition Elements
        moveTutorial.transform.Find("line_1").GetComponent<RectTransform>().position = new Vector3(Screen.width * c.getInputMoveSize() / 2f, Screen.height / 2f, 1);
        moveTutorial.transform.Find("line_2").GetComponent<RectTransform>().position = new Vector3(Screen.width * c.getInputMoveSize(), Screen.height / 2f, 1);
        moveTutorial.transform.Find("arrow_left").GetComponent<RectTransform>().position = new Vector3(Screen.width * c.getInputMoveSize() / 4f, Screen.height / 2f, 1);
        moveTutorial.transform.Find("arrow_right").GetComponent<RectTransform>().position = new Vector3(Screen.width * c.getInputMoveSize() / 4f * 3f, Screen.height / 2f, 1);
        moveTutorial.transform.Find("Finger").GetComponent<RectTransform>().position = new Vector3(Screen.width * c.getInputMoveSize() / 2f - 70, -75, 1);
        jumpTutorial.transform.Find("line_1").GetComponent<RectTransform>().position = new Vector3(Screen.width * c.getInputMoveSize(), Screen.height / 2f, 1);

        //Rescale Elements
        float scale = c.getUIScale();
        moveTutorial.transform.Find("arrow_left").GetComponent<RectTransform>().localScale = new Vector3(scale, 1, 1);
        moveTutorial.transform.Find("arrow_right").GetComponent<RectTransform>().localScale = new Vector3(scale, 1, 1);
        moveTutorial.transform.Find("Finger").GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
        jumpTutorial.transform.Find("Finger").GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
        jumpTutorial.transform.Find("Finger_Shadow").GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
        //jumpTutorial.transform.FindChild("jump-container").GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);

        jumpTutorial.SetActive(false);
    }

    void Update()
    {
        if(Time.timeScale == 0)
        {

        }
        else
        {

        }
    }

    public override void performTriggerEvent(Trigger source)
    {
        if(source == moveHideTrigger)
        {
            moveTutorial.SetActive(false);
            return;
        }

        if (source == jumpShowTrigger)
        {
            jumpTutorial.SetActive(true);
            return;
        }

        if (source == jumpHideTrigger)
        {
            jumpTutorial.SetActive(false);
            return;
        }
    }
}
