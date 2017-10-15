using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNavigator : MonoBehaviour {
    
    [SerializeField]
    float speed;
    [SerializeField]
    DigitalJoyStick joystick;

    void Awake()
    {
        joystick.onJoyStickkMove += move;
    }


	public void move(Vector2 direction)
    {
        this.gameObject.transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
    }
}
