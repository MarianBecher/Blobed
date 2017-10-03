using UnityEngine;
using System.Collections;

public class DoorControler : Triggerable {

    private int opend = 0;
    protected Animator animator;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void performTriggerEvent(Trigger source)
    {
        if (source.GetType() != typeof(LeverBlockController))
            return;
        
        LeverBlockController lever = (LeverBlockController)source;
        bool shouldBeOpen = lever.isPressed();

        if (shouldBeOpen)
            opend++;
        else
            opend--;
        
        animator.SetInteger("trigger", opend);
    }
}
