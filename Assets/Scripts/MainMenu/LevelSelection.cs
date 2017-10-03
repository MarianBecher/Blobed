using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelection : MonoBehaviour {

    public int lvl_index;

    public bool unlocked = false;
    public bool status = false;
    private Animator animator;
    private InputController input;
    private bool valueChanged = true;
    

    void Start()
    {
        input = InputController.getController();
        animator = this.GetComponent<Animator>();
    }

    public void updateAnimation()
    {
        if (!unlocked)
        {
            animator.SetTrigger("Disabled");       
        }
        else if (status)
            animator.SetTrigger("Pressed");
        else
        {
            animator.SetTrigger("Normal");
        }

    }

	// Use this for initialization
	public void setStatus(bool newStatus) {
        this.status = newStatus;
        valueChanged = true;
	}

    public void setUnlocked(bool newUnlockedStatus)
    {
        this.unlocked = newUnlockedStatus;
        valueChanged = true;
    }

	// Update is called once per frame
	void Update () {
        RectTransform rt = this.GetComponent<RectTransform>();
        Vector2 position = rt.position;
        bool clicked = input.isClickedOnUIElement(position, rt.rect.width, rt.rect.height);

        if (unlocked && clicked)
        {
            setStatus(true);
            GameObject.Find("MenuHandler").GetComponent<MainMenuController>().setSelection(lvl_index);
        }

        if (valueChanged)
            updateAnimation();
	}
}
