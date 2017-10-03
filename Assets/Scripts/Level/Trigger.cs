using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public abstract class Trigger : TilePrefab {

    public Action onConfigChanged;
    public List<Triggerable> targets;

    public override void configure(TileSettings settings)
    {
        Debug.Log(settings.triggerTargets.Length);
        if (onConfigChanged != null)
            onConfigChanged();
    }

    protected void informTargets()
    {
        foreach(Triggerable t in targets)
            t.performTriggerEvent(this);
    }
}
