using UnityEngine;
using System.Collections;

public abstract class CharacterSettings {

    public abstract Vector3 getScale();
    public abstract bool canWallJump();
    public abstract float getMoveSpeed();
    public abstract int getJumpPower();
    public abstract float getMass();
}


public class SmallCharacterSettings : CharacterSettings {
    
    public override Vector3 getScale() { return new Vector3(1f, 1f, 1f); }
    public override bool canWallJump() { return false; }
    public override float getMoveSpeed() { return 6; }
    public override int getJumpPower() { return 600; }
    public override float getMass() { return 1; }
}

public class MediumCharacterSettings : CharacterSettings
{

    public override Vector3 getScale() { return new Vector3(1.3f, 1.35f, 1f); }
    public override bool canWallJump() { return false; }
    public override float getMoveSpeed() { return 4; }
    public override int getJumpPower() { return 4800; }
    public override float getMass() { return 8; }
}

public class LargeCharacterSettings : CharacterSettings
{

    public override Vector3 getScale() { return new Vector3(1.8f, 2.1f, 1f); }
    public override bool canWallJump() { return true; }
    public override float getMoveSpeed() { return 4; }
    public override int getJumpPower() { return 4800; }
    public override float getMass() { return 8; }
}