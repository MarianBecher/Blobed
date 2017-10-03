using System; 

[Serializable]
public class TileSettings {
    //Trigger Settings
    public CharacterType minTriggerSize;
    public int[] triggerTargets;

    public TileSettings()
    {
        minTriggerSize = CharacterType.Medium;
        triggerTargets = new int[0];
    }

    public TileSettings(TileSettings clone) {
        this.minTriggerSize = clone.minTriggerSize;
        triggerTargets = new int[clone.triggerTargets.Length];

        for (int i = 0; i < clone.triggerTargets.Length; i++)
            this.triggerTargets[i] = clone.triggerTargets[i];
    }
}
