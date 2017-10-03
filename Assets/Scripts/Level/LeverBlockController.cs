using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public class LeverBlockController : Trigger
{

    private enum Lever_Status {released,pressed,not_enough};
    public bool isSingle = true;
    public Color deactiveLight = new Color(218,46,46);
    public Color activeLight = new Color(9,238,20);


    private Animator animator;
    private Lever_Status status = Lever_Status.released;
    private Collider2D _collider2D;
    public LayerMask characterLayerMask;
    private SpriteRenderer light1;
    private SpriteRenderer light2;
    private SpriteRenderer light1Border;
    private SpriteRenderer light2Border;


    public void Start()
    {
        animator = GetComponent<Animator>();   
        _collider2D = GetComponent<Collider2D>();
        light1 = transform.Find("lever_light_1").GetComponent<SpriteRenderer>();
        light2 = transform.Find("lever_light_2").GetComponent<SpriteRenderer>();
        light1Border = transform.Find("lever_light_1_border").GetComponent<SpriteRenderer>();
        light2Border = transform.Find("lever_light_2_border").GetComponent<SpriteRenderer>();
        if (isSingle)
        {
            light1.enabled = false;
            light1Border.enabled = false;

            light2.transform.localPosition = new Vector3(0, -0.1f, -2);
            light2Border.transform.localPosition = new Vector3(0, -0.1f, -1);
            light2.transform.localScale = new Vector3(1, 1.5f, 1);
            light2Border.transform.localScale = new Vector3(1, 1.5f, 1);
        }
    }

    public void Update()
    {
        Vector2 offset = new Vector2(0, 0.2f);
        Collider2D collider = Physics2D.OverlapArea(
            (Vector2)(_collider2D.bounds.center - _collider2D.bounds.extents) ,
            (Vector2)(_collider2D.bounds.center + _collider2D.bounds.extents) + offset, //nach oben etwas höher
            characterLayerMask);
        Lever_Status oldStatus = status;



        if (collider == null)
        {
            status = Lever_Status.released;
            light2.color = deactiveLight;
            if (!isSingle)
                light1.color = deactiveLight;
        }
        else
        {
            bool isBox = collider.gameObject.layer == LayerMask.NameToLayer("Box");
            if (!isSingle && !isBox && (collider.GetComponent<CharacterController>().Type == CharacterType.Small))
            {
                status = Lever_Status.not_enough;
                light2.color = activeLight;
                light1.color = deactiveLight;
            }
            else
            {
                status = Lever_Status.pressed;
                light2.color = activeLight;
                if (!isSingle)
                    light1.color = activeLight;
            }
        }


        if (status == Lever_Status.released && oldStatus != Lever_Status.released)
            animator.SetTrigger("deactivate");
        else if ((status == Lever_Status.pressed || status == Lever_Status.not_enough) && oldStatus == Lever_Status.released)
            animator.SetTrigger("activate");

        if (oldStatus == Lever_Status.pressed && (status == Lever_Status.released || status == Lever_Status.not_enough))
        {
            foreach (Triggerable t in targets)
                t.performTriggerEvent(this);
        }
        else if (oldStatus != Lever_Status.pressed && status == Lever_Status.pressed)
        {
            foreach (Triggerable t in targets)
                t.performTriggerEvent(this);
        }            
    }

    public bool isPressed()
    {
        return status == Lever_Status.pressed;
    }
}
