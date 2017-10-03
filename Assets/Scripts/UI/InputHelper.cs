using UnityEngine;
using System.Collections;

public class InputHelper : MonoBehaviour {
    GameObject container;
    InputController input;
    Animator animator;

	// Use this for initialization
	void Start () {
        input = InputController.getController();
        Config c = Config.getConfig();

        //Reposition Elements
        container = transform.Find("container").gameObject;
        RectTransform line1 = container.transform.Find("line_1").GetComponent<RectTransform>();
        RectTransform line2 = container.transform.Find("line_2").GetComponent<RectTransform>();
        line1.position = new Vector3(Screen.width * c.getInputMoveSize() / 2f, Screen.height / 2f, 1);
        line2.position = new Vector3(Screen.width * c.getInputMoveSize(), Screen.height / 2f, 1);
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (input.currentlyTouched())
        {
            animator.SetBool("show", false);
        }
        else
        {
            animator.SetBool("show", true);
        }
	}
}
