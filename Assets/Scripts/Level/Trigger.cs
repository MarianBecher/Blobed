using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public abstract class Trigger : TilePrefab {

    public List<Triggerable> targets;

    public override void configure(TileSettings settings, LevelLoader loader)
    {
        foreach(int targetID in settings.triggerTargets)
        {
            Debug.Log(targetID);
            TilePrefab target = loader.getTileInstance(targetID);
            Debug.Log(target);
            Debug.Log(target.GetType());
            if (typeof(Triggerable).IsAssignableFrom(target.GetType()))
                targets.Add((Triggerable) target);
        }
    }

    protected void informTargets()
    {
        foreach(Triggerable t in targets)
            t.performTriggerEvent(this);
    }
}
