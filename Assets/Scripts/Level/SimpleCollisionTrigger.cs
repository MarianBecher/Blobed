using UnityEngine;
using System.Collections;

public class SimpleCollisionTrigger : Trigger {

    void OnTriggerEnter2D(Collider2D c)
    {
        this.informTargets();
    }
}
