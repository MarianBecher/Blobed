using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void JoyStickEvent(Vector2 direction);

public class DigitalJoyStick : MonoBehaviour {

    [SerializeField]
    private Image cursor;

    private Rect rect;
    private Vector2 center;
    private Vector3 canvasScale;
    private bool isMouseDown = false;

    public JoyStickEvent onJoyStickkMove;

    void Awake()
    {
        this.rect = this.GetComponent<RectTransform>().rect;
    }

    void Start()
    {
        Canvas c = this.getCanvas(this.gameObject);
        canvasScale = c.GetComponent<RectTransform>().localScale;
    }

    public void Update()
    {
        Vector2 mp = Input.mousePosition;
        mp.x /= canvasScale.x;
        mp.y /= canvasScale.y;

        if (Input.GetMouseButtonDown(0)
            && mp.x >= rect.xMin
            && mp.x <= rect.xMax
            && mp.y >= rect.yMin
            && mp.y <= rect.yMax)
        {
            isMouseDown = true;
        }
        else if(isMouseDown && !Input.GetMouseButton(0))
        {
            isMouseDown = false;
        }

        
        if(isMouseDown)
        { 
            Vector2 halfSize = new Vector2(rect.size.x / 2.0f, rect.size.y / 2.0f);
            Vector2 direction = new Vector2(mp.x - rect.center.x, mp.y - rect.center.y);
            if (direction.magnitude > halfSize.magnitude)
                direction = direction.normalized * halfSize.magnitude;

            cursor.transform.localPosition = direction * 0.5f + rect.center;

            direction /= halfSize.magnitude;
            if (onJoyStickkMove != null)
                onJoyStickkMove(direction);
        }
        else
        {
            cursor.transform.localPosition = rect.center;
        }
    }
    

    private Canvas getCanvas(GameObject g)
    {
        Canvas c = g.GetComponent<Canvas>();
        if (c != null)
            return c;

        return getCanvas(g.transform.parent.gameObject);
    }
}
