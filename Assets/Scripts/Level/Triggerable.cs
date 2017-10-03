using UnityEngine;
using System.Collections;

public abstract class Triggerable : TilePrefab {
    public abstract void performTriggerEvent(Trigger source);
}
